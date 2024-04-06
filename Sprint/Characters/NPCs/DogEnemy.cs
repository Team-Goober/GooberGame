using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Microsoft.Xna.Framework;
using Sprint.Commands.SecondaryItem;
using System;
using Sprint.Projectile;
using Sprint.Sprite;
using Sprint.Levels;
using Sprint.Collision;
using Sprint.Door;

namespace Sprint.Characters
{
    internal class DogEnemy : Enemy
    {
        private float elapsedTime;
        private Timer timeAttack;
        private Vector2 moveDirection; // Movement direction for the random pattern
        private ICommand projectileCommand;
        private SimpleProjectileFactory itemFactory;
        private Vector2 initialPosition;
        private string lastAnimationName;
        private MoveVert moveVert;

        public DogEnemy(ISprite sprite, ISprite damagedSprite, Vector2 initialPosition, Room room, SpriteLoader spriteLoader)
            : base(sprite, damagedSprite, initialPosition, room)
        {

            // Store the initial position for reference
            this.initialPosition = initialPosition;

            timeAttack = new Timer(2);
            timeAttack.Start();

            itemFactory = new SimpleProjectileFactory(spriteLoader, 30, true, room);

            projectileCommand = new ShootBoomarangC(itemFactory);

            moveVert = new MoveVert(physics);

        }

        // Register a directional animation for DogEnemy sprite
        public void RegisterDirectionalAnimation(string animationLabel, IAtlas atlas)
        {
            sprite.RegisterAnimation(animationLabel, atlas);
        }

        // Set the current animation for DogEnemy sprite
        public void SetAnimation(string animationLabel)
        {
            sprite.SetAnimation(animationLabel);
        }

        // Set the scale of DogEnemy sprite
        public void SetScale(int scale)
        {
            sprite.SetScale(scale);
        }

        // Update DogEnemy logic
        public override void Update(GameTime gameTime)
        {
            timeAttack.Update(gameTime);
            base.Update(gameTime);

            // Uses timer to shoot projectiles every 2 seconds
            if (timeAttack.JustEnded)
            {
                itemFactory.SetStartPosition(physics.Position);
                itemFactory.SetDirection(moveDirection);
                projectileCommand.Execute();
                timeAttack.Start();
            }

            // Move randomly within a specified area
            moveVert.MoveAI(gameTime);

            // Set animation based on the new direction
            SetAnimationBasedOnDirection();

            // Update the sprite and physics
            sprite.Update(gameTime);
            physics.Update(gameTime);
        }

        // Set animation based on the direction of movement
        private void SetAnimationBasedOnDirection()
        {
            string newAnim = "";
            DirectionFace currDir = moveVert.directionFace;
            switch (currDir)
            {
                case DirectionFace.UP:
                    newAnim = "downFacing";
                    moveDirection = Directions.DOWN;
                    break;
                case DirectionFace.LEFT:
                    newAnim = "leftFacing";
                    moveDirection = Directions.LEFT;
                    break;
                case DirectionFace.DOWN:
                    newAnim = "upFacing";
                    moveDirection = Directions.UP;
                    break;
                case DirectionFace.RIGHT:
                    newAnim = "rightFacing";
                    moveDirection = Directions.RIGHT;
                    break;
                default:
                    newAnim = "downFacing";
                    break;
            }
            
            if (newAnim != lastAnimationName)
            {
                lastAnimationName = newAnim;
                SetAnimation(newAnim);
            }



        }




    }
}
