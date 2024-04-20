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
        private ISprite normalSprite; // Version of sprite without damage
        private ISprite damagedSprite; // Version of sprite taking damage

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

        // Direction that the player is facing
        public Vector2 Facing { get; private set; }

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

        // Animation to return to as base after a played animation ends
        private enum AnimationCycle
        {
            Idle,
            Walk
        }
        private AnimationCycle baseAnim;

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
            normalSprite = spriteLoader.BuildSprite("playerAnims" , "player");
            damagedSprite = spriteLoader.BuildSprite("playerDamagedAnims" , "player");
            sprite = normalSprite; // Start out undamaged

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

        public SpriteLoader GetSpriteLoader()
        {
            return spriteLoader;
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
            normalSprite.SetScale(spriteScale);
            damagedSprite.SetScale(spriteScale);
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
                normalSprite.SetAnimation("swordDown");
                damagedSprite.SetAnimation("swordDown");
                swordRec = new Rectangle((int)physics.Position.X - swordWidth / 2, (int)physics.Position.Y, swordWidth, swordLength);
            }
            else if (Facing == Directions.LEFT)
            {
                normalSprite.SetAnimation("swordLeft");
                damagedSprite.SetAnimation("swordLeft");
                swordRec = new Rectangle((int)physics.Position.X - swordLength, (int)physics.Position.Y - swordWidth / 2, swordLength, swordWidth);
            }
            else if (Facing == Directions.UP)
            {
                normalSprite.SetAnimation("swordUp");
                damagedSprite.SetAnimation("swordUp");
                swordRec = new Rectangle((int)physics.Position.X - swordWidth / 2, (int)physics.Position.Y - swordLength, swordWidth, swordLength);
            }
            else if (Facing == Directions.RIGHT)
            {
                normalSprite.SetAnimation("swordRight");
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
                normalSprite.SetAnimation("castDown");
                damagedSprite.SetAnimation("castDown");
            }
            else if (Facing == Directions.LEFT)
            {
                normalSprite.SetAnimation("castLeft");
                damagedSprite.SetAnimation("castLeft");
            }
            else if (Facing == Directions.UP)
            {
                normalSprite.SetAnimation("castUp");
                damagedSprite.SetAnimation("castUp");
            }
            else if (Facing == Directions.RIGHT)
            {
                normalSprite.SetAnimation("castRight");
                damagedSprite.SetAnimation("castRight");
            }
        }

        // Removes velocity and changes animation to match lack of movement
        public void StopMoving()
        {
            accelerationDirection = Vector2.Zero;
            returnToBaseAnim();
        }


        // Return to base animation cycle based on states and facing dir
        private void returnToBaseAnim()
        {
            if (baseAnim == AnimationCycle.Idle)
            {
                if (Facing == Directions.DOWN)
                {
                    normalSprite.SetAnimation("downStill");
                    damagedSprite.SetAnimation("downStill");
                }
                else if (Facing == Directions.LEFT)
                {
                    normalSprite.SetAnimation("leftStill");
                    damagedSprite.SetAnimation("leftStill");
                }
                else if (Facing == Directions.UP)
                {
                    normalSprite.SetAnimation("upStill");
                    damagedSprite.SetAnimation("upStill");
                }
                else if (Facing == Directions.RIGHT)
                {
                    normalSprite.SetAnimation("rightStill");
                    damagedSprite.SetAnimation("rightStill");
                }
            }
            else if (baseAnim == AnimationCycle.Walk)
            {
                if (Facing == Directions.DOWN)
                {
                    normalSprite.SetAnimation("down");
                    damagedSprite.SetAnimation("down");
                }
                else if (Facing == Directions.LEFT)
                {
                    normalSprite.SetAnimation("left");
                    damagedSprite.SetAnimation("left");
                }
                else if (Facing == Directions.UP)
                {
                    normalSprite.SetAnimation("up");
                    damagedSprite.SetAnimation("up");
                }
                else if (Facing == Directions.RIGHT)
                {
                    normalSprite.SetAnimation("right");
                    damagedSprite.SetAnimation("right");
                }
            }

        }


        public void MoveLeft()
        {
            accelerationDirection.X -= 1; // Add to X acceleration to move left

            normalSprite.SetAnimation("left");
            damagedSprite.SetAnimation("left");
            Facing = Directions.LEFT;
            baseAnim = AnimationCycle.Walk;
            
        }

        public void MoveRight()
        {
            accelerationDirection.X += 1; // Add to X acceleration to move right

            
                sprite.SetAnimation("right");
                Facing = Directions.RIGHT;
                baseAnim = AnimationCycle.Walk;
            
        }

        public void MoveUp()
        {
            // Don't move while shielding
            if (shielded)
                return;
            accelerationDirection.Y -= 1; // Add to Y acceleration to move up
            normalSprite.SetAnimation("up");
            damagedSprite.SetAnimation("up");
            Facing = Directions.UP;
            baseAnim = AnimationCycle.Walk;
            
        }

        public void MoveDown()
        {// Don't move while shielding
            if (shielded)
                return;
            accelerationDirection.Y += 1; // Add to Y acceleration to move down
            normalSprite.SetAnimation("down");
            damagedSprite.SetAnimation("down");
            Facing = Directions.DOWN;
            baseAnim = AnimationCycle.Walk;
            
        }

        public void ReleaseLeft()
        {
            accelerationDirection.X += 1; // Subtract from X acceleration when left key is released
        }

        public void ReleaseRight()
        {
            accelerationDirection.X -= 1; // Subtract from X acceleration when right key is released
        }

        public void ReleaseUp()
        {
            accelerationDirection.Y += 1; // Subtract from Y acceleration when up key is released
        }

        public void ReleaseDown()
        {
            accelerationDirection.Y -= 1; // Subtract from Y acceleration when down key is released
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
            physics.UpdateVelocity(CharacterConstants.STILL_FRICTION, speed, gameTime);

            // Determine the animation based on acceleration
            if (physics.Acceleration != Vector2.Zero)
            {
                if (Math.Abs(physics.Acceleration.X) > Math.Abs(physics.Acceleration.Y))
                {
                    // Horizontal movement dominates
                    if (physics.Acceleration.X > 0)
                    {
                        Facing = Directions.RIGHT;
                        
                    }
                    else
                    {
                        Facing = Directions.LEFT;
                      
                    }
                }
                else
                {
                    // Vertical movement dominates
                    if (physics.Acceleration.Y > 0)
                    {
                        Facing = Directions.DOWN;
                        
                    }
                    else
                    {
                        Facing = Directions.UP;
                       
                    }
                }
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
                sprite = normalSprite;
                returnToBaseAnim();
            

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
            // switching sprites
            sprite = damagedSprite;
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

