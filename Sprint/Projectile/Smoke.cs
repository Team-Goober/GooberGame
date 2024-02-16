using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Sprite;
using System;

namespace Sprint.Projectile
{
    internal class Smoke : IProjectile
    {
        ISprite sprite;
        Vector2 position;

        public Smoke(Texture2D smoke, Vector2 startPos) 
        {
            this.position = startPos;

            this.sprite = new AnimatedSprite(smoke);
            IAtlas atlas = new SingleAtlas(new Rectangle(0, 0, 7, 8), new Vector2(3.5f, 4));
            sprite.RegisterAnimation("smoke", atlas);
            sprite.SetAnimation("smoke");

            sprite.SetScale(4);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }
    }
}
