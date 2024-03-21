using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Sprite;
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Sprint.Characters
{
    public class JellyfishEnemy : Enemy
    {
        private float elapsedTime;
        private Vector2 initialPosition;

        public JellyfishEnemy(ISprite sprite, Vector2 initialPosition, GameObjectManager objManager, SpriteLoader spriteLoader)
            : base(sprite, initialPosition, objManager)
        {

            // Store the initial position for reference
            this.initialPosition = initialPosition;
        }

        private Dictionary<Directions, string> JellyAnimDict = new Dictionary<Directions, string>
        {
            { Directions.RIGHT, "rightFacing" },
            { Directions.LEFT, "leftFacing" },
            { Directions.UP, "upFacing" },
            { Directions.DOWN, "downFacing" }
        };


        // Set the direction and update the animation accordingly
        public void SetDirection(Directions direction)
        {


            if (JellyAnimDict.TryGetValue(direction, out string facing))
            {
                sprite.SetAnimation(facing);

            }
        }

        public override void Update(GameTime gameTime)
        {
            // Calculate movement based on elapsed time
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Adjust the speed and side length of the square loop
            float speed = 50;
            float sideLength = 100;

            float totalTimeForLoop = 4 * sideLength / speed;

            // Reset elapsedTime to keep the loop going indefinitely
            if (elapsedTime > totalTimeForLoop)
            {
                elapsedTime -= totalTimeForLoop;
            }

            // Move in a solid straight-line square loop
            float offsetX = 0;
            float offsetY = 0;

            if (elapsedTime < sideLength / speed)
            {
                // Move right
                offsetX = speed * elapsedTime;
            }
            else if (elapsedTime < 2 * sideLength / speed)
            {
                // Move down
                offsetX = sideLength;
                offsetY = speed * (elapsedTime - sideLength / speed);
            }
            else if (elapsedTime < 3 * sideLength / speed)
            {
                // Move left
                offsetX = sideLength - speed * (elapsedTime - 2 * sideLength / speed);
                offsetY = sideLength;
            }
            else
            {
                // Move up
                offsetY = Math.Max(0, sideLength - speed * (elapsedTime - 3 * sideLength / speed));
            }

            // Set the new position based on the calculated offsets
            Vector2 newPosition = initialPosition + new Vector2(offsetX, offsetY);
            physics.SetPosition(newPosition);

            // Determine and set the animation based on the direction
            if (elapsedTime < sideLength / speed)
            {
                SetDirection(Directions.RIGHT);
            }
            else if (elapsedTime < 2 * sideLength / speed)
            {
                SetDirection(Directions.DOWN);
            }
            else if (elapsedTime < 3 * sideLength / speed)
            {
                SetDirection(Directions.LEFT);
            }
            else
            {
                SetDirection(Directions.UP);
            }

            // Update the sprite and physics
            sprite.Update(gameTime);
            physics.Update(gameTime);
        }

        // Set the current animation
        public void SetAnimation(string animationLabel)
        {
            sprite.SetAnimation(animationLabel);
        }

        // Set the scale of the sprite
        public void SetScale(int scale)
        {
            sprite.SetScale(scale);
        }
    }
}
