using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;
using Sprint.Projectile;
using Sprint.Levels;
using Sprint.Collision;
using System;
using Sprint.Music.Sfx;
using Sprint.Items;
using Sprint.Functions.States;
using System.Diagnostics;

namespace Sprint.Characters
{

    internal class Player : Character, IMovingCollidable
    {
        private DungeonState dungeon;
        private Inventory inventory;

        private SfxFactory sfxFactory;

        // Player sprites
        private ISprite sprite;
        private SpriteLoader spriteLoader;
        private ISprite damagedSprite;

        // Events to signal hearts changed
        public delegate void HealthUpdateDelegate(double prev, double next);
        public event HealthUpdateDelegate OnPlayerHealthChange;
        public delegate void MaxHealthUpdateDelegate(int prev, int next, double health);
        public event MaxHealthUpdateDelegate OnPlayerMaxHealthChange;

        private Physics physics;
        private Room room;

        // Constants and initial values
        private int sideLength = CharacterConstants.DEFAULT_SIDE_LENGTH * CharacterConstants.COLLIDER_SCALE;
        private int maxHealth = CharacterConstants.STARTING_HEALTH;
        private double health = CharacterConstants.STARTING_HEALTH;
        private float speed = CharacterConstants.PLAYER_SPEED;

        // Weapons
        private SimpleProjectileFactory secondaryItems;
        private SwordCollision swordCollision;
        private const int swordWidth = CharacterConstants.SWORD_WIDTH, swordLength = CharacterConstants.SWORD_LENGTH;
        private bool shielded;

        // Direction that the player is facing
        public Vector2 Facing { get; private set; }

        public Rectangle BoundingBox => new((int)(physics.Position.X - sideLength / 2.0),
                (int) (physics.Position.Y - sideLength / 2.0),
                sideLength,
                sideLength);

        public CollisionTypes[] CollisionType {
            get
            {
                // Collide as shield if shield is up
                if (shielded)
                {
                    return new CollisionTypes[] { CollisionTypes.SHIELD, CollisionTypes.PLAYER, CollisionTypes.CHARACTER };
                }
                else
                {
                    return new CollisionTypes[] { CollisionTypes.PLAYER, CollisionTypes.CHARACTER };
                }
            }
        }

        // Timers for animations and cooldowns
        private Timer attackTimer;
        private Timer castTimer;
        private Timer damageTimer;

        // Animation to return to as base after a played animation ends
        private enum AnimationCycle
        {
            Idle,
            Walk
        }
        private AnimationCycle baseAnim;

        // Acceleration vector for player movement
        private Vector2 acceleration = Vector2.Zero;
        private float speedLimit = CharacterConstants.PLAYER_SPEED;

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
            damagedSprite = spriteLoader.BuildSprite("playerDamagedAnims" , "player");

            // Duration of one sword swing or item use
            attackTimer = new Timer(0.25);
            castTimer = new Timer(0.25);
            // Duration of the damage state
            damageTimer = new Timer(0.5);

            room = null;

            // Start out idle
            Facing = Directions.STILL;
            baseAnim = AnimationCycle.Idle;

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
        }

        // Create melee attack according to facing direction and with given damage value
        public void Attack(float dmg)
        {
            Rectangle swordRec  = new Rectangle();


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
            if (Facing == Directions.DOWN)
            {
                sprite.SetAnimation("swordDown");
                damagedSprite.SetAnimation("swordDown");
                swordRec = new Rectangle((int)physics.Position.X - swordWidth / 2, (int)physics.Position.Y, swordWidth, swordLength);
            }
            else if (Facing == Directions.LEFT)
            {
                sprite.SetAnimation("swordLeft");
                damagedSprite.SetAnimation("swordLeft");
                swordRec = new Rectangle((int)physics.Position.X - swordLength, (int)physics.Position.Y - swordWidth / 2, swordLength, swordWidth);
            }
            else if (Facing == Directions.UP)
            {
                sprite.SetAnimation("swordUp");
                damagedSprite.SetAnimation("swordUp");
                swordRec = new Rectangle((int)physics.Position.X - swordWidth / 2, (int)physics.Position.Y - swordLength, swordWidth, swordLength);
            }
            else if (Facing == Directions.RIGHT)
            {
                sprite.SetAnimation("swordRight");
                damagedSprite.SetAnimation("swordRight");
                swordRec = new Rectangle((int)physics.Position.X, (int)physics.Position.Y - swordWidth / 2, swordLength, swordWidth);
            }

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

            // Player shouldn't move while attacking
            StopMoving();

            // Start timer for attack
            castTimer.Start();

            if (Facing == Directions.DOWN)
            {
                sprite.SetAnimation("castDown");
                damagedSprite.SetAnimation("castDown");
            }
            else if (Facing == Directions.LEFT)
            {
                sprite.SetAnimation("castLeft");
                damagedSprite.SetAnimation("castLeft");
            }
            else if (Facing == Directions.UP)
            {
                sprite.SetAnimation("castUp");
                damagedSprite.SetAnimation("castUp");
            }
            else if (Facing == Directions.RIGHT)
            {
                sprite.SetAnimation("castRight");
                damagedSprite.SetAnimation("castRight");
            }
        }

        // Removes velocity and changes animation to match lack of movement
        public void StopMoving()
        {
            physics.SetAcceleration(Vector2.Zero);
            physics.SetVelocity(Vector2.Zero);
            returnToBaseAnim();
        }

        // Return to base animation cycle based on states and facing dir
        private void returnToBaseAnim()
        {
            if (baseAnim == AnimationCycle.Idle)
            {
                if (Facing == Directions.DOWN)
                {
                    sprite.SetAnimation("downStill");
                    damagedSprite.SetAnimation("downStill");
                }
                else if (Facing == Directions.LEFT)
                {
                    sprite.SetAnimation("leftStill");
                    damagedSprite.SetAnimation("leftStill");
                }
                else if (Facing == Directions.UP)
                {
                    sprite.SetAnimation("upStill");
                    damagedSprite.SetAnimation("upStill");
                }
                else if (Facing == Directions.RIGHT)
                {
                    sprite.SetAnimation("rightStill");
                    damagedSprite.SetAnimation("rightStill");
                }
            }
            else if (baseAnim == AnimationCycle.Walk)
            {
                if (Facing == Directions.DOWN)
                {
                    sprite.SetAnimation("down");
                    damagedSprite.SetAnimation("down");
                }
                else if (Facing == Directions.LEFT)
                {
                    sprite.SetAnimation("left");
                    damagedSprite.SetAnimation("left");
                }
                else if (Facing == Directions.UP)
                {
                    sprite.SetAnimation("up");
                    damagedSprite.SetAnimation("up");
                }
                else if (Facing == Directions.RIGHT)
                {
                    sprite.SetAnimation("right");
                    damagedSprite.SetAnimation("right");
                }
            }

        }

        public void MoveLeft()
        {
            // Don't move while shielding
            
            Vector2 newAcceleration = new Vector2(-CharacterConstants.accelerationRate, 0);
            physics.SetAcceleration(newAcceleration);
            sprite.SetAnimation("left");
            Facing = Directions.LEFT;
            baseAnim = AnimationCycle.Walk;
        }

        public void MoveRight()
        {
            // Don't move while shielding
           
            Vector2 newAcceleration = new Vector2(CharacterConstants.accelerationRate, 0);
            physics.SetAcceleration(newAcceleration);
            sprite.SetAnimation("right");
            Facing = Directions.RIGHT;
            baseAnim = AnimationCycle.Walk;
        }

        public void MoveUp()
        {
            // Don't move while shielding
           
            Vector2 newAcceleration = new Vector2(0, -CharacterConstants.accelerationRate);
            physics.SetAcceleration(newAcceleration);
            sprite.SetAnimation("up");
            Facing = Directions.UP;
            baseAnim = AnimationCycle.Walk;
        }

        public void MoveDown()
        {
            // Don't move while shielding
           
            Vector2 newAcceleration = new Vector2(0, CharacterConstants.accelerationRate);
            physics.SetAcceleration(newAcceleration);
            sprite.SetAnimation("down");
            Facing = Directions.DOWN;
            baseAnim = AnimationCycle.Walk;
        }

        public void MoveDiagonal(Vector2 direction)
        {
            // Don't move while shielding
           
            float diagonalSpeed = CharacterConstants.PLAYER_SPEED / (float)(Math.Sqrt(2) * 64); // Diagonal movement speed
            Vector2 newAcceleration = direction * CharacterConstants.accelerationRate;
            physics.SetAcceleration(newAcceleration);
        }


        public Physics GetPhysic()
        {
            return physics;
        }

        public override void Update(GameTime gameTime)
        {

            // Update the velocity using the Physics component
            physics.UpdateVelocity(CharacterConstants.stillFriction, speedLimit, gameTime);

            // Check if the player is not moving to return to the base animation
            if (physics.Velocity == Vector2.Zero)
            {
                sprite.SetAnimation("down");
                damagedSprite.SetAnimation("down");
            }

            // Determine the direction of movement to set the correct walking animation

            if (physics.Velocity.X > 0)
            {
                Facing = Directions.RIGHT;
            }
            else if (physics.Velocity.X < 0)
            {
                Facing = Directions.LEFT;
            }
            else if (physics.Velocity.Y > 0)
            {
                Facing = Directions.DOWN;
            }
            else if (physics.Velocity.Y < 0)
            {
                Facing = Directions.UP;
            }


            // Check for end of sword swing
            attackTimer.Update(gameTime);
            if (attackTimer.JustEnded)
            {
                room.GetScene().Remove(swordCollision);
                returnToBaseAnim();
            }
            // Check for end of cast animation
            castTimer.Update(gameTime);
            if (castTimer.JustEnded)
            {
                returnToBaseAnim();
            }

            // Update projectile factory positioning
            secondaryItems.SetDirection(Facing);
            secondaryItems.SetStartPosition(physics.Position);

            // Checks for damage state
            damageTimer.Update(gameTime);
            if (damageTimer.JustEnded)
            {
                sprite = spriteLoader.BuildSprite("playerAnims", "player");
                returnToBaseAnim();

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
        public void Move(Vector2 distance)
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

        public void Heal(float amt)
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
            // switching sprites
            sprite = damagedSprite;
            // timer to turn sprite back
            damageTimer.Start();
            // update health
            double prevHealth = health;
            health -= dmg;

            // broadcast health change
            OnPlayerHealthChange?.Invoke(prevHealth, health);

            // Trigger death when health is at or below 0
            if (health <= 0.0)
            {
                Die();
            }

        }

        public void SetShielded(bool active)
        {
            shielded = active;
        }
    }
}

