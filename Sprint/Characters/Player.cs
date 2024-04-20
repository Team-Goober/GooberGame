using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;
using Sprint.Projectile;
using Sprint.Levels;
using Sprint.Collision;
using Sprint.Music.Sfx;
using Sprint.Items;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Sprint.Characters
{

    internal class Player : Character
    {
        private DungeonState dungeon;
        private Inventory inventory;

        private SfxFactory sfxFactory;

        // Player sprites
        private ISprite sprite; // Current sprite that's drawn
        private SpriteLoader spriteLoader;

        // Events to signal hearts changed
        public delegate void HealthUpdateDelegate(double prev, double next);
        public event HealthUpdateDelegate OnPlayerHealthChange;
        public delegate void MaxHealthUpdateDelegate(int prev, int next, double health);
        public event MaxHealthUpdateDelegate OnPlayerMaxHealthChange;
        // Event that signals room change
        public delegate void RoomChangeDelegate(Room newRoom);
        public event RoomChangeDelegate OnPlayerRoomChange;

        private Physics physics;
        private Room room;

        // Constants and initial values
        private float sideLength = CharacterConstants.DEFAULT_SIDE_LENGTH * CharacterConstants.COLLIDER_SCALE;
        private int maxHealth = CharacterConstants.STARTING_HEALTH;
        private double health = CharacterConstants.STARTING_HEALTH;

        // Weapons
        private SimpleProjectileFactory secondaryItems;
        private SwordCollision swordCollision;
        private const int swordWidth = CharacterConstants.SWORD_WIDTH, swordLength = CharacterConstants.SWORD_LENGTH;
        private bool shielded; // Shield is up, dont take damage from the front
        private bool noclip; // Able to walk through walls


        private Vector2 _facing = Directions.DOWN;
        public Vector2 Facing // Direction that the player is facing
        {
            get { return _facing; }
            private set
            {
                _facing = value;
                SyncAnimation(); // Update animation
            }
        }
        // Translate direction vector to animation spring
        private readonly Dictionary<Vector2, string> facingToAnim = new() {
            {Directions.UP, "Up" },
            {Directions.RIGHT, "Right" },
            {Directions.DOWN, "Down" },
            {Directions.LEFT, "Left" }
            };

        private string _mode = STAND;
        public string Mode // Input state that the player is in
        {
            get { return _mode; }
            private set
            {
                _mode = value;
                SyncAnimation(); // Update animation
            }
        }
        // Constants for the animation strings for each mode
        private const string CAST = "cast", WALK = "walk", STAND = "stand", SWORD = "sword";

        private bool _damaged = false;
        public bool Damaged // Whether the player is in damage state
        {
            get { return _damaged; }
            set
            {
                _damaged = value;
                SyncAnimation(); // Update animation
            }
        }
        private const string DAMAGE_ANIMS = "Damage";

        public override Rectangle BoundingBox => new((int)(physics.Position.X - sideLength / 2.0),
                (int) (physics.Position.Y - sideLength / 2.0),
                (int)sideLength,
                (int)sideLength);

        public override CollisionTypes[] CollisionType {
            get
            {
                // Length of the collision types array is based on state of player
                int len = 2 + (shielded?1:0) + (noclip?1:0);
                CollisionTypes[] types = new CollisionTypes[len];
                int i = 0;

                if (shielded)
                    types[i++] = CollisionTypes.SHIELD; // Collide as shield if shield is up
                if (noclip)
                    types[i++] = CollisionTypes.FLYING; // Phase through walls if noclip
                types[i++] = CollisionTypes.PLAYER; // Act as player by default
                types[i++] = CollisionTypes.CHARACTER; // Ac as character if no player interaction
                return types;
            }
        }

        // Timers for animations and cooldowns
        private Timer attackTimer;
        private Timer castTimer;
        private Timer damageTimer;

        // Acceleration vector for player movement
        private Vector2 accelerationDirection = Vector2.Zero;
        private float accelerationRate = CharacterConstants.ACCELERATION_RATE;
        private float speed = CharacterConstants.PLAYER_SPEED;

        //declares the move systems for the main character sprite
        public Player(SpriteLoader spriteLoader, DungeonState dungeon)
        {
            this.dungeon = dungeon;
            //Initialize SFX player
            sfxFactory = SfxFactory.GetInstance();

            //Initialize physics and objectManager
            physics = new Physics(Vector2.Zero);
            this.spriteLoader = spriteLoader;

            inventory = new Inventory();

            //Loads sprite for link
            sprite = spriteLoader.BuildSprite("playerAnims" , "player");

            // Duration of one sword swing or item use
            attackTimer = new Timer(0.25);
            castTimer = new Timer(0.25);
            // Duration of the damage state
            damageTimer = new Timer(0.5);

            room = null;

            // Initial state
            Facing = Directions.DOWN;
            Mode = STAND;
            Damaged = false;

            // Set up projectile factory
            secondaryItems = new SimpleProjectileFactory(spriteLoader, CharacterConstants.PROJECTILE_SPAWN_DISTANCE, false, null);         
            
        }

        public SimpleProjectileFactory GetProjectileFactory()
        {
            return secondaryItems;
        }

        public Inventory GetInventory()
        {
            return inventory;
        }

        public Room GetCurrentRoom()
        {
            return room;
        }

        public SpriteLoader GetSpriteLoader()
        {
            return spriteLoader;
        }

        // Updates animation to match internal state
        public void SyncAnimation()
        {
            // Animation based on mode, direction, and damage
            sprite.SetAnimation(Mode + facingToAnim[Facing] + (Damaged?DAMAGE_ANIMS:""));
        }

        // Moves the player from current scene into a new one
        public void SetRoom(Room room)
        {
            if (this.room != null)
            {
                this.room.GetScene().Remove(this);
            }

            this.room = room;
            secondaryItems.SetRoom(this.room);
            this.room.GetScene().Add(this);
            StopMoving();
            OnPlayerRoomChange?.Invoke(room);
        }

        // Grow or shrink the sprite and collider for the player
        public void SetScale(float scale)
        {
            sideLength = CharacterConstants.DEFAULT_SIDE_LENGTH * scale * CharacterConstants.COLLIDER_SCALE;
            float spriteScale = scale * CharacterConstants.SPRITE_SCALE;
            sprite.SetScale(spriteScale);
        }

        // Create melee attack according to facing direction and with given damage value
        public void Attack(float dmg)
        {
            // Only attack if not already attacking
            if (!attackTimer.Ended)
            {
                
                return;
            }

            // Player shouldn't move while attacking
            StopMoving();

            // Start timer for attack
            attackTimer.Start();
            castTimer.Start();

            //Creates animations and bounds for the sword for collision
            Mode = SWORD;

            // Get offsets for sword top left corner
            double xOffset = (Facing.X == 0) ? - swordWidth / 2 : swordLength / 2 * (-1 + Facing.X);
            double yOffset = (Facing.Y == 0) ? - swordWidth / 2 : swordLength / 2 * (-1 + Facing.Y);
            int xDim = (Facing.X == 0) ? swordWidth : swordLength;
            int yDim = (Facing.Y == 0) ? swordWidth : swordLength;
            // Create bounding rectangle for sword
            Rectangle swordRec = new Rectangle((int)(physics.Position.X + xOffset), (int)(physics.Position.Y + yOffset), xDim, yDim);

            swordCollision = new SwordCollision(swordRec, this, dmg);
            
            room.GetScene().Add(swordCollision);
                    
        }

        // Cast according to direction
        public void Cast()
        {

            // Only attack if not already attacking
            if (!castTimer.Ended)
            {
                return;
            }

            // Start timer for attack
            castTimer.Start();

            Mode = CAST;
            
        }

        // Removes velocity and changes animation to match lack of movement
        public void StopMoving()
        {
            accelerationDirection = Vector2.Zero;
            Mode = STAND;
        }

        public void Walk(Vector2 direction)
        {
            // Don't move while shielding
            if (shielded)
                return;
            // Update facing direction
            Facing = direction;
            // Accelerate towards direction
            accelerationDirection += direction;
        }

        public void EndWalk(Vector2 direction)
        {
            // Remove acceleration towards direction
            accelerationDirection -= direction;
        }
    

        public override Vector2 GetPosition()
        {
            return physics.Position;
        }

        public override void Update(GameTime gameTime)
        {
            // Update the acceleration based on the current acceleration direction
            Vector2 normalizedDir = (accelerationDirection.LengthSquared() > 0) ? Vector2.Normalize(accelerationDirection) : Vector2.Zero;
            physics.SetAcceleration(normalizedDir * accelerationRate);

            // Update the velocity using the Physics component
            physics.UpdateVelocity(speed, gameTime);


            // Update projectile factory positioning
            if (Mode == WALK)
            {
                // If walking, use accel direction to allow diagonals
                secondaryItems.SetDirection(accelerationDirection);
            }
            else
            {
                // If standing, use facing direction
                secondaryItems.SetDirection(Facing);
            }
            secondaryItems.SetStartPosition(physics.Position);

            // Move between walk and stand modes based on if the player is attempting to accelerate
            if (accelerationDirection != Vector2.Zero)
            {
                if (Mode == STAND)
                    Mode = WALK;
            }
            else
            {
                if (Mode == WALK)
                    Mode = STAND;
            }

            // Check for end of sword swing
            attackTimer.Update(gameTime);
            if (attackTimer.JustEnded)
            {
                room.GetScene().Remove(swordCollision);
                Mode = STAND;
            }
            // Check for end of cast animation
            castTimer.Update(gameTime);
            if (castTimer.JustEnded)
            {
                Mode = STAND;
            }

            // Checks for damage state ending
            damageTimer.Update(gameTime);
            if (damageTimer.JustEnded)
            {
                Damaged = false;
            }



            // Die when health is zero
            // Must be in update instead of TakeDamage so items can intervene in death
            if(health <= 0.0)
            {
                Die();
            }

            physics.Update(gameTime);
            sprite.Update(gameTime);
        }


        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //Draws sprite animation using AnimationSprite class
            sprite.Draw(spriteBatch, physics.Position, gameTime);

        }

        // Moves the player by a set distance
        public override void Move(Vector2 distance)
        {
            // teleport player in displacement specified
            physics.SetPosition(physics.Position + distance);
        }
        
        // Moves player to set position
        public void MoveTo(Vector2 pos)
        {
            physics.SetPosition(pos);
        }

        /// <summary>
        /// Pickup Item off the ground
        /// </summary>
        /// <param name="item"> ItemType to pickup</param>
        /// <return>true if item picked up</return>
        public bool PickupItem(Item item)
        {
            // Ask item if it can be added to inventory
            if(item.CanPickup(inventory))
            {
                // Run item's apply behavior to add to inventory
                item.GetPowerup().Apply(this);
                // Remove item game object
                room.GetScene().Remove(item);
                return true;
            }
            return false;
        }

        // Send to a game over
        public override void Die()
        {
            dungeon.DeathScreen();
        }

        // Send to win screen
        public void Win()
        {
            StopMoving();
            sprite.SetAnimation("holdItem");
            dungeon.WinScreen();
        }

        public MapModel GetMap()
        {
            return dungeon.GetMap();
        }

        // Adds to player base speed
        public void AddSpeed(float addition)
        {
            speed += addition;
        }

        public void Heal(double amt)
        {
            // Don't reduce health during heal
            Debug.Assert(amt >= 0);
            double prevHealth = health;
            health += amt;
            // Don't overfill hearts
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            OnPlayerHealthChange?.Invoke(prevHealth, health);
        }

        public void IncreaseHearts()
        {
            int prevMax = maxHealth;
            // Don't go over what the HUD can display
            if (maxHealth < CharacterConstants.MAX_HEARTS)
                maxHealth += 1;
            OnPlayerMaxHealthChange?.Invoke(prevMax, maxHealth, health);
        }

        // Reduce health
        public override void TakeDamage(double dmg)
        {
            // Don't allow negative damage
            Debug.Assert(dmg >= 0);
            // Invincible until timer goes down
            if (!damageTimer.Ended)
            {
                return;
            }
            // sound playing
            sfxFactory.PlaySoundEffect("Player Hurt");
            // become damaged
            Damaged = true;
            // timer to turn sprite back
            damageTimer.Start();
            // update health
            double prevHealth = health;
            health -= dmg;

            // Trigger death when health is at or below 0
            if (health < 0.0)
            {
                health = 0.0;
            }

            // broadcast health change
            OnPlayerHealthChange?.Invoke(prevHealth, health);

        }

        public void SetShielded(bool active)
        {
            shielded = active;
        }

        public void SetNoclip(bool active)
        {
            noclip = active;
        }
    }
}

