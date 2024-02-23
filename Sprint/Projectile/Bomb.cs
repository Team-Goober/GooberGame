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

        public Bomb(Texture2D sheet, Vector2 startPos)
        {
            this.position = startPos;
            
            sprite = new AnimatedSprite(sheet);
            IAtlas bomb = new AutoAtlas(new Rectangle(0, 0, 85, 16), 1, 5, 1, new Vector2(8, 8), false, 3);
            sprite.RegisterAnimation("bomb", bomb);
            sprite.SetAnimation("bomb");
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
