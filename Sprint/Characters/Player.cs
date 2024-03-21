using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;
using Sprint.Projectile;
using Sprint.Levels;
using Sprint.Collision;
using System.Diagnostics;
using System.Collections.Generic;
using Sprint.Input;

namespace Sprint.Characters
{

    internal class Player : Character, IMovingCollidable
    {
        public Inventory inventory;

        private ISprite sprite;

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
        private GameObjectManager objectManager;



        //Dictinaries to replace switch statements
        private Dictionary<Directions, string> AttackAnimDict = new Dictionary<Directions, string>
        {
            { Directions.RIGHT, "swordRight" },
            { Directions.LEFT, "swordLeft" },
            { Directions.UP, "swordUp" },
            { Directions.DOWN, "swordDown" }
        };

        private Dictionary<Directions, string> CastAnimDict = new Dictionary<Directions, string>
        {
            { Directions.RIGHT, "castRight" },
            { Directions.LEFT, "castLeft" },
            { Directions.UP, "castUp" },
            { Directions.DOWN, "castDown" }
        };

        private Dictionary<Directions, string> BaseIdleAnimDict = new Dictionary<Directions, string>
        {
            { Directions.RIGHT, "rightStill" },
            { Directions.LEFT, "leftStill" },
            { Directions.UP, "upStill" },
            { Directions.DOWN, "downStill" }
        };

        private Dictionary<Directions, string> BaseWalkAnimDict = new Dictionary<Directions, string>
        {
            { Directions.RIGHT, "right" },
            { Directions.LEFT, "left" },
            { Directions.UP, "up" },
            { Directions.DOWN, "down" }
        };


        private Dictionary<Directions, Rectangle> FindSwordRec;


        // TODO: replace this with state machine
        // Animation to return to as base after a played animation ends
        private enum AnimationCycle
        {
            Idle,
            Walk
        }
        private AnimationCycle baseAnim;


        //declares the move systems for the main character sprite
        public Player(Vector2 pos, IInputMap inputTable, GameObjectManager objManager, SpriteLoader spriteLoader)
        {

            //Initialize physics and objectManager
            physics = new Physics(pos);

            inventory = new Inventory();

            objectManager = objManager;

            //Loads sprite for link
            sprite = spriteLoader.BuildSprite("playerAnims", "player");

            // Duration of one sword swing or item use
            attackTimer = new Timer(0.5);
            castTimer = new Timer(0.5);
            // Duration of the damage state
            damageTimer = new Timer(0.3);

            

            // Start out idle
            Facing = Directions.STILL;
            baseAnim = AnimationCycle.Idle;

            physics = new Physics(pos);

            // Set up projectiles
            secondaryItems = new ProjectileSystem(physics.Position, inputTable, objManager, spriteLoader);

            //Saves rectangles for sword collision
            FindSwordRec = new Dictionary<Directions, Rectangle>
            {
                { Directions.RIGHT, new Rectangle((int)physics.Position.X, (int)physics.Position.Y - swordWidth / 2, swordLength, swordWidth)},
                { Directions.LEFT, new Rectangle((int)physics.Position.X - swordLength, (int)physics.Position.Y - swordWidth / 2, swordLength, swordWidth) },
                { Directions.UP, new Rectangle((int)physics.Position.X - swordWidth / 2, (int)physics.Position.Y - swordLength, swordWidth, swordLength) },
                { Directions.DOWN, new Rectangle((int)physics.Position.X - swordWidth / 2, (int)physics.Position.Y, swordWidth, swordLength) }

            };
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


            if (AttackAnimDict.TryGetValue(Facing, out string direction) && FindSwordRec.TryGetValue(Facing, out swordRec))
            {
                sprite.SetAnimation(direction);

            }


            
            swordCollision = new SwordCollision(swordRec, this);
            
            objectManager.Add(swordCollision, false);
            
            
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


            if (CastAnimDict.TryGetValue(Facing, out string direction))
            {
                sprite.SetAnimation(direction);

            }
        }

        public void StopMoving()
        {
            physics.SetVelocity(new Vector2(0, 0));
            /*            // TODO: replace these checks once we have a state machine
                        string currAnim = sprite.GetCurrentAnimation();

                        // If the current animation is movement, stop it
                        if (currAnim.Equals("left") || currAnim.Equals("right") || currAnim.Equals("up") || currAnim.Equals("down"))
                        {
                            animateStill();
                        }*/
            baseAnim = AnimationCycle.Idle;
            returnToBaseAnim();
        }

        // Return to base animation cycle based on states and facing dir
        // TODO: replace with a state machine
        private void returnToBaseAnim()
        {
            if (baseAnim == AnimationCycle.Idle)
            {
                if (BaseIdleAnimDict.TryGetValue(Facing, out string direction))
                {
                    sprite.SetAnimation(direction);

                }
            }
            else if (baseAnim == AnimationCycle.Walk)
            {
                if (BaseWalkAnimDict.TryGetValue(Facing, out string direction))
                {
                    sprite.SetAnimation(direction);

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

        public void TakeDamage()
        {

            sprite.SetAnimation("damage");
            damageTimer.Start();

        }


        public override void Update(GameTime gameTime)
        {

            // Check for end of sword swing
            attackTimer.Update(gameTime);
            if (attackTimer.JustEnded)
            {
                objectManager.Remove(swordCollision, false);
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
            objectManager.Remove(this, true);
        }
    }
}

