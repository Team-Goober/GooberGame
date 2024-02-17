using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;
using System;

namespace Sprint.Projectile
{
    internal class Boomarang : IProjectile
    {
        ISprite sprite;
        Vector2 position;
        Vector2 originalPosition;
        Vector2 velocity;
        Vector2 direction;

        const float speed = 400;
        const float backSpeed = -400;

        public Boomarang(Texture2D sheet, Vector2 startPos, Vector2 newDirection)
        {
            this.position = startPos;
            this.originalPosition = this.position;
            this.direction = newDirection;

            if (direction.Length() == 0)
            {
                velocity = Vector2.Zero;
            }
            else
            {
                velocity = Vector2.Normalize(direction) * speed;
            }

            sprite = new AnimatedSprite(sheet);
            IAtlas atlas = new AutoAtlas(new Rectangle(1, 2, 56, 9), 1, 4, 10, new Vector2(3, 4), true, 18);
            sprite.RegisterAnimation("boomarang", atlas);
            sprite.SetAnimation("boomarang");
            sprite.SetScale(4);
        }

        private void returnBack(Vector2 currentPosition)
        {
            float difX = Math.Abs(currentPosition.X - this.originalPosition.X);
            float difY = Math.Abs(currentPosition.Y - this.originalPosition.Y);
            if(difX > 200 || difY > 200)
            {
                velocity = Vector2.Normalize(direction) * backSpeed;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            float rotation = (float)Math.Atan2(velocity.Y, velocity.X);
            sprite.Draw(spriteBatch, position, gameTime, rotation);
        }

        public void Update(GameTime gameTime)
        {
            // Move linearly one way, then flip to the other way
            returnBack(position);
            position += velocity * (float)(gameTime.ElapsedGameTime.TotalSeconds);

            sprite.Update(gameTime);
        }
    }
}
