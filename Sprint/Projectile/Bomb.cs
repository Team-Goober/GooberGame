using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;

namespace Sprint.Projectile
{
    internal class Bomb : IProjectile
    {
        ISprite sprite;
        Vector2 position;

        public Bomb(ISprite sprite, Vector2 startPos)
        {
            this.position = startPos;
            
            this.sprite = sprite;

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
