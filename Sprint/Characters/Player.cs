using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;
using Sprint.Projectile;
using Sprint.Levels;
using Sprint.Collision;
using System.Diagnostics;
using Sprint.Testing;
using Sprint.Commands;
using System;
using Sprint.Music.Sfx;
using Sprint.Items;
using Sprint.HUD;
using Sprint.Functions.DeathState;
using System.Collections.Generic;

namespace Sprint.Characters
{
    internal class Player : Character, IMovingCollidable
    {
        public Inventory inventory;
        private SfxFactory sfxFactory;
        private ISprite sprite;
        private SpriteLoader spriteLoader;
        private ISprite damagedSprite;

        public event EventHandler OnPlayerDied;
        public delegate void HealthUpdateDelegate(double prev, double next);
        public event HealthUpdateDelegate OnPlayerHealthChange;
        public delegate void MaxHealthUpdateDelegate(int prev, int next, double health);
        public event MaxHealthUpdateDelegate OnPlayerMaxHealthChange;

        private Physics physics;
        private int sideLength = CharacterConstants.DEFAULT_SIDE_LENGTH * CharacterConstants.COLLIDER_SCALE;
        private int maxHealth = CharacterConstants.STARTING_HEALTH;
        private double health = CharacterConstants.STARTING_HEALTH;
        private ProjectileSystem secondaryItems;
        private SwordCollision swordCollision;
        private const int swordWidth = CharacterConstants.SWORD_WIDTH, swordLength = CharacterConstants.SWORD_LENGTH;

        public Vector2 Facing { get; private set; }
        public Rectangle BoundingBox => new((int)(physics.Position.X - sideLength / 2.0),
                (int)(physics.Position.Y - sideLength / 2.0),
                sideLength,
                sideLength);
        public CollisionTypes[] CollisionType => new CollisionTypes[] { CollisionTypes.PLAYER, CollisionTypes.CHARACTER };
        private float speed = CharacterConstants.PLAYER_SPEED;
        private Timer attackTimer;
        private Timer castTimer;
        private Timer damageTimer;
        private Room room;
        private OpenDeath gameOver;
        private enum AnimationCycle
        {
            Idle,
            Walk
        }
        private AnimationCycle baseAnim;
        private Vector2 acceleration = Vector2.Zero;

        private float accelerationRate = 500f;

        public Player(SpriteLoader spriteLoader, DungeonState dungeon)
        {
            sfxFactory = SfxFactory.GetInstance();
            physics = new Physics(Vector2.Zero);
            this.spriteLoader = spriteLoader;
            inventory = new Inventory();
            sprite = spriteLoader.BuildSprite("playerAnims", "player");
            damagedSprite = spriteLoader.BuildSprite("playerDamagedAnims", "player");
            attackTimer = new Timer(0.5);
            castTimer = new Timer(0.5);
            damageTimer = new Timer(0.5);
            room = null;
            Facing = Directions.STILL;
            baseAnim = AnimationCycle.Idle;
            secondaryItems = new ProjectileSystem(physics.Position, spriteLoader);
            this.gameOver = new OpenDeath(dungeon);
        }

        public SimpleProjectileFactory GetProjectileFactory()
        {
            return secondaryItems.ProjectileFactory;
        }

        public Inventory GetInventory()
        {
            return inventory;
        }

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

        public void Attack()
        {
            Rectangle swordRec = new Rectangle();
            if (!attackTimer.Ended)
            {
                return;
            }

            StopMoving();

            attackTimer.Start();
            castTimer.Start();

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

            swordCollision = new SwordCollision(swordRec, this);

            room.GetScene().Add(swordCollision);
        }

        public void Cast()
        {
            if (!castTimer.Ended)
            {
                return;
            }

            StopMoving();

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

        public void WinPose()
        {
            sprite.SetAnimation("holdItem");
        }

        public void StopMoving()
        {
            acceleration = Vector2.Zero;
            physics.SetVelocity(Vector2.Zero);
            returnToBaseAnim();
            // Reset Facing direction to STILL
            Facing = Directions.STILL;
        }


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
            acceleration.X = -accelerationRate;
            sprite.SetAnimation("left");
            Facing = Directions.LEFT;
            baseAnim = AnimationCycle.Walk;
        }

        public void MoveRight()
        {
            acceleration.X = accelerationRate;
            sprite.SetAnimation("right");
            Facing = Directions.RIGHT;
            baseAnim = AnimationCycle.Walk;

        }

        public void MoveUp()
        {
            acceleration.Y = -accelerationRate;
            sprite.SetAnimation("up");
            Facing = Directions.UP;
            baseAnim = AnimationCycle.Walk;

        }

        public void MoveDown()
        {
            acceleration.Y = accelerationRate;
            sprite.SetAnimation("down");
            Facing = Directions.DOWN;
            baseAnim = AnimationCycle.Walk;

        }

        public void MoveDiagonal(Vector2 direction)
        {
            float diagonalSpeed = CharacterConstants.PLAYER_SPEED / (float)(Math.Sqrt(2) * 64); // Diagonal movement speed
            Vector2 movementVector = direction;
            movementVector.Normalize();
            acceleration = movementVector * accelerationRate;           
        }






        public Physics GetPhysic()
        {
            return physics;
        }

        public override void TakeDamage(double dmg)
        {
            if (!damageTimer.Ended)
            {
                return;
            }

            sfxFactory.PlaySoundEffect("Player Hurt");
            sprite = damagedSprite;
            damageTimer.Start();
            double prevHealth = health;
            health -= dmg;

            OnPlayerHealthChange?.Invoke(prevHealth, health);

            if (health <= 0.0)
            {
                this.Die();
            }
            else if (health > maxHealth)
            {
                health = maxHealth;
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Update player's velocity based on acceleration
            Vector2 newVelocity = physics.Velocity + acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Apply friction or deceleration to gradually slow down the player
            // Adjust friction dynamically based on player's movement
            float friction = 0.02f; // Default friction
            if (physics.Velocity.LengthSquared() > 0)
            {
                // Apply higher friction if the player is moving
                friction = 0.03f;
            }
            newVelocity *= (1f - friction);

            // Clamp the speed for both X and Y components
            newVelocity.X = MathHelper.Clamp(newVelocity.X, -speed, speed);
            newVelocity.Y = MathHelper.Clamp(newVelocity.Y, -speed, speed);

            physics.SetVelocity(newVelocity);

            // Check if the player is not moving to return to the base animation
            if (physics.Velocity == Vector2.Zero)
            {
                baseAnim = AnimationCycle.Idle;
                returnToBaseAnim();
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
           

            // Update timers and other components
            attackTimer.Update(gameTime);
            if (attackTimer.JustEnded)
            {
                room.GetScene().Remove(swordCollision);
                returnToBaseAnim();
            }

            castTimer.Update(gameTime);
            if (castTimer.JustEnded)
            {
                returnToBaseAnim();
            }

            secondaryItems.UpdateDirection(Facing);
            secondaryItems.UpdatePostion(physics.Position);

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
            sprite.Draw(spriteBatch, physics.Position, gameTime);
        }

        public void Move(Vector2 distance)
        {
            physics.SetPosition(physics.Position + distance);
        }

        public void MoveTo(Vector2 pos)
        {
            physics.SetPosition(pos);
        }

        public void PickupItem(Item item)
        {
            ItemType itemType = item.GetItemType();

            if (item.GetColliable())
            {
                inventory.PickupItem(itemType);
                room.GetScene().Remove(item);
            }
        }

        public void UseItem(ItemType item)
        {
            inventory.ConsumeItem(item);
        }

        public override void Die()
        {
            OnPlayerDied?.Invoke(this, EventArgs.Empty);
            gameOver.Execute();
        }



        public void OnInventoryEvent(ItemType it, int prev, int next, List<ItemType> ownedUpgrades)
        {
            switch (it)
            {
                case ItemType.HeartPiece:
                    int prevMax = maxHealth;
                    if (maxHealth < 16)
                        maxHealth += 1;
                    OnPlayerMaxHealthChange?.Invoke(prevMax, maxHealth, health);
                    break;
                case ItemType.Heart:
                    double prevHealth = health;
                    health += 1;
                    if (health > maxHealth)
                    {
                        health = maxHealth;
                    }
                    OnPlayerHealthChange?.Invoke(prevHealth, health);
                    break;
                default:
                    break;
            }
        }
    }
}
