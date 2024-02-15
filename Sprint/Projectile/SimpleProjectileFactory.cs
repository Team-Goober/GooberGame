using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint.Projectile
{
    internal class SimpleProjectileFactory
    {
        private Texture2D itemSheet;
        private Texture2D bomb;
        private Texture2D fireBall;

        Vector2 position;
        Vector2 direction;

        string spriteDirection;

        public SimpleProjectileFactory()
        {
         
        }

        public void LoadAllTextures(ContentManager content)
        {
            itemSheet = content.Load<Texture2D>("zelda_items");
            bomb = content.Load<Texture2D>("Items/Bomb");
            fireBall = content.Load<Texture2D>("Items/FireBall");

        }

        public Arrow CreateArrow()
        {
            return new Arrow(itemSheet, position, direction, spriteDirection);
        }

        public BlueArrow CreateBlueArrow()
        {
            return new BlueArrow(itemSheet, position, direction, spriteDirection);
        }

        public Bomb CreateBomb()
        {
            return new Bomb(bomb, position, direction); 
        }
        
        public Boomarang CreateBoomarang()
        { 
            return new Boomarang(itemSheet, position, direction);
        }

        public FireBall CreateFireBall()
        {
            return new FireBall(fireBall, position, direction);
        }

        public void SetDirection(Vector2 direction)
        {
            this.direction = direction;
        }

        public void SetSpriteDirection(string newSpriteDirection)
        {
            this.spriteDirection = newSpriteDirection;
        }

        public void SetStartPosition(Vector2 pos)
        {
            position = pos;
        }
    }
}
