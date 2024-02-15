using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;
using System.Diagnostics;

namespace Sprint.Projectile
{
    internal class Bomb : IProjectile
    {
        ISprite sprite;
        Vector2 position;
        Vector2 direction;

        public Bomb(Texture2D sheet, Vector2 startPos, Vector2 newDirection)
        {
            this.position = startPos;
            this.direction = newDirection;

            //left
            if(direction == new Vector2(-1, 0))
            {
                position.X -= 20;
            }

            //right
            if(direction == new Vector2(1, 0))
            {
                position.X += 60;
            }

            //up
            if(direction == new Vector2(1, -90))
            {
                position.Y -= 60;
            }

            //Down
            if(direction == new Vector2(1, 90))
            {
                position.Y += 50;
            }
            
            sprite = new AnimatedSprite(sheet);
            //IAtlas atlas = new SingleAtlas(new Rectangle(204, 1, 9, 14), new Vector2(0, 0));
            //Please look at this
            IAtlas bomb = new AutoAtlas(new Rectangle(0, 0, 85, 16), 1, 5, 1, new Vector2(8, 8), false, 5);
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
