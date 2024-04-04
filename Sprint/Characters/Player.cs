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

namespace Sprint.Characters
{

    internal class Player : Character, IMovingCollidable
    {
        public Inventory inventory;

        private ISprite sprite;
        private ISprite defaultSprite;
        private SpriteLoader spriteLoader;
        private ISprite damagedSprite;
        public event EventHandler OnPlayerDamaged;

        private Physics physics;

        private int sideLength = 3 * 16;

        private ProjectileSystem secondaryItems;
        private SwordCollision swordCollision;
        private const int swordWidth = 40, swordLength = 90;

        public Directions Facing { get; private set; }

        public Rectangle BoundingBox => new((int)(physics.Position.X - sideLength / 2.0),
                (int) (physics.Position.Y - sideLength / 2.0),
                sideLength,
                sideLength);

        public CollisionTypes[] CollisionType => new CollisionTypes[] { CollisionTypes.PLAYER, CollisionTypes.CHARACTER };

        private const float speed = 200;

        private Timer attackTimer;
        private Timer castTimer;
        private Timer damageTimer;
        private SceneObjectManager objectManager;
        private Reset reset;

        // TODO: replace this with state machine
        // Animation to return to as base after a played animation ends
        private enum AnimationCycle
        {
            Idle,
            Walk
        }
        private AnimationCycle baseAnim;


        //declares the move systems for the main character sprite
        public Player(IInputMap inputTable, SpriteLoader spriteLoader, Reset reset)
        {

            //Initialize physics and objectManager
            physics = new Physics(Vector2.Zero);
            this.spriteLoader = spriteLoader;

            inventory = new Inventory();

            //Loads sprite for link
            sprite = spriteLoader.BuildSprite("playerAnims" , "player");
            damagedSprite = spriteLoader.BuildSprite("playerDamagedAnims" , "player");

            // Duration of one sword swing or item use
            attackTimer = new Timer(0.5);
            castTimer = new Timer(0.5);
            // Duration of the damage state
            damageTimer = new Timer(0.1);

            objectManager = null;

            // Start out idle
            Facing = Directions.STILL;
            baseAnim = AnimationCycle.Idle;

            // Set up projectiles
            secondaryItems = new ProjectileSystem(physics.Position, inputTable, spriteLoader);

            this.reset = reset;
        }

        // Moves the player from current scene into a new one
        public void SetScene(SceneObjectManager scene)
        {
            if (objectManager != null)
            {
                objectManager.Remove(this);
            }

            objectManager = scene;
            secondaryItems.SetScene(objectManager);
            objectManager.Add(this);
            StopMoving();
        }

        //Melee attack according to direction
        public void Attack()
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
            switch (Facing)
            {
                case Directions.RIGHT:
                    sprite.SetAnimation("swordRight");
                    damagedSprite.SetAnimation("swordRight");
                    swordRec = new Rectangle((int)physics.Position.X, (int)physics.Position.Y - swordWidth / 2, swordLength, swordWidth);
                    break;
                case Directions.LEFT:
                    sprite.SetAnimation("swordLeft");
                    damagedSprite.SetAnimation("swordLeft");
                    swordRec = new Rectangle((int)physics.Position.X - swordLength, (int)physics.Position.Y - swordWidth / 2, swordLength, swordWidth);
                    break;
                case Directions.UP:
                    sprite.SetAnimation("swordUp");
                    damagedSprite.SetAnimation("swordUp");
                    swordRec = new Rectangle((int)physics.Position.X - swordWidth / 2, (int)physics.Position.Y - swordLength, swordWidth, swordLength);
                    break;
                case Directions.DOWN:
                    sprite.SetAnimation("swordDown");
                    damagedSprite.SetAnimation("swordDown");
                    swordRec = new Rectangle((int)physics.Position.X - swordWidth / 2, (int)physics.Position.Y, swordWidth, swordLength);
                    break;
                default:
                    break;
            }

            swordCollision = new SwordCollision(swordRec, this);
            
            objectManager.Add(swordCollision);
        }

        //Cast according to direction
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

            switch (Facing)
            {
                case Directions.RIGHT:
                    sprite.SetAnimation("castRight");
                    damagedSprite.SetAnimation("castRight");
                    break;
                case Directions.LEFT:
                    sprite.SetAnimation("castLeft");
                    damagedSprite.SetAnimation("castLeft");
                    break;
                case Directions.UP:
                    sprite.SetAnimation("castUp");
                    damagedSprite.SetAnimation("castUp");
                    break;
                case Directions.DOWN:
                    sprite.SetAnimation("castDown");
                    damagedSprite.SetAnimation("castDown");
                    break;
                default:
                    break;
            }
        }

        // Removes velocity and changes animation to match lack of movement
        public void StopMoving()
        {
            physics.SetVelocity(new Vector2(0, 0));
            baseAnim = AnimationCycle.Idle;
            returnToBaseAnim();
        }

        // Return to base animation cycle based on states and facing dir
        // TODO: replace with a state machine
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
            // Sets velocity towards left
            physics.SetVelocity(new Vector2(-speed, 0));

            sprite.SetAnimation("left");
            Facing = Directions.LEFT;
            baseAnim = AnimationCycle.Walk;
        }

        public void MoveRight()
        {
            // Sets velocity towards right
            physics.SetVelocity(new Vector2(speed, 0));

            sprite.SetAnimation("right");
            Facing = Directions.RIGHT;
            baseAnim = AnimationCycle.Walk;
        }

        public void MoveUp()
        {
            // Sets velocity towards up
            physics.SetVelocity(new Vector2(0, -speed));

            sprite.SetAnimation("up");
            Facing = Directions.UP;
            baseAnim = AnimationCycle.Walk;
        }

        public void MoveDown()
        {
            // Sets velocity towards down
            physics.SetVelocity(new Vector2(0, speed));
            sprite.SetAnimation("down");
            Facing = Directions.DOWN;
            baseAnim = AnimationCycle.Walk;
        }

        public Physics GetPhysic()
        {
            return physics;
        }

        public override void TakeDamage()
        {
            // ATTENTION
            defaultSprite = sprite;
            sprite = damagedSprite;
            damageTimer.Start();
            OnPlayerDamaged?.Invoke(this, EventArgs.Empty);
        }


        public override void Update(GameTime gameTime)
        {

            // Check for end of sword swing
            attackTimer.Update(gameTime);
            if (attackTimer.JustEnded)
            {
                objectManager.Remove(swordCollision);
                returnToBaseAnim();
            }
            castTimer.Update(gameTime);
            if (castTimer.JustEnded)
            {
                returnToBaseAnim();
            }

            secondaryItems.UpdateDirection(Facing);
            secondaryItems.UpdatePostion(physics.Position);

            // Checks for damage state
            damageTimer.Update(gameTime);
            if (damageTimer.JustEnded)
            {
                Debug.WriteLine("AWWWOOOGAA!!!");
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
        // Should be in Characters?
        public void MoveTo(Vector2 pos)
        {
            physics.SetPosition(pos);
        }

        /// <summary>
        /// Pickup Item off the ground
        /// </summary>
        /// <param name="item"> ItemType to pickup</param>
        public void PickupItem(Item item)
        {
            ItemType itemType = item.GetItemType();
            if(item.GetColliable())
            {
                inventory.PickupItem(itemType);
                objectManager.Remove(item);
            }
        }

        /// <summary>
        /// Subtract item from inventory
        /// </summary>
        /// <param name="item">ItemType to decrement</param>
        public void UseItem(ItemType item)
        {
            inventory.ConsumeItem(item);
        }

        // Remove player from game
        public override void Die()
        {
            reset.Execute();
        }
    }
}

