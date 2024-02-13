// JellyfishEnemy.cs

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Sprite;
using System;

namespace Sprint
{
    public class JellyfishEnemy : Enemy
    {
        private float elapsedTime;
        private Vector2 initialPosition;

        public JellyfishEnemy(Game1 game, Texture2D spriteSheet, Vector2 initialPosition, IAtlas enemyAtlas)
            : base(game, new AnimatedSprite(spriteSheet), initialPosition)
        {
            // Register the default animation
            sprite.RegisterAnimation("default", enemyAtlas);

            // Store the initial position for reference
            this.initialPosition = initialPosition;
        }

        public static JellyfishEnemy CreateJellyfishEnemy(Game1 game, Vector2 initialPosition)
        {
            string textureName = "zelda_enemies";
            Vector2 center = new Vector2(8, 8);
            int scale = 2;

            Texture2D jellyfishTexture = game.Content.Load<Texture2D>(textureName);

            // Define directional atlases for animations
            IAtlas upFacing = new SingleAtlas(new Rectangle(0, 0, 16, 16), new Vector2(8, 8));
            IAtlas leftFacing = new SingleAtlas(new Rectangle(88, 0, 16, 16), new Vector2(8, 8));
            IAtlas downFacing = new SingleAtlas(new Rectangle(60, 0, 16, 16), new Vector2(8, 8));
            IAtlas rightFacing = new SingleAtlas(new Rectangle(32, 0, 16, 16), new Vector2(8, 8));

            JellyfishEnemy jellyfishEnemy = new JellyfishEnemy(game, jellyfishTexture, initialPosition, upFacing);

            // Register directional animations
            jellyfishEnemy.RegisterDirectionalAnimation("upFacing", upFacing);
            jellyfishEnemy.RegisterDirectionalAnimation("leftFacing", leftFacing);
            jellyfishEnemy.RegisterDirectionalAnimation("downFacing", downFacing);
            jellyfishEnemy.RegisterDirectionalAnimation("rightFacing", rightFacing);

            // Set the default animation and scale
            jellyfishEnemy.SetAnimation("default");
            jellyfishEnemy.SetScale(scale);

            return jellyfishEnemy;
        }

        // Register a directional animation with a specific atlas
        public void RegisterDirectionalAnimation(string animationLabel, IAtlas atlas)
        {
            sprite.RegisterAnimation(animationLabel, atlas);
        }

        // Set the direction and update the animation accordingly
        public void SetDirection(Directions direction)
        {
            switch (direction)
            {
                case Directions.UP:
                    SetAnimation("upFacing");
                    break;
                case Directions.LEFT:
                    SetAnimation("leftFacing");
                    break;
                case Directions.DOWN:
                    SetAnimation("downFacing");
                    break;
                case Directions.RIGHT:
                    SetAnimation("rightFacing");
                    break;
                default:
                    SetAnimation("default");
                    break;
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
