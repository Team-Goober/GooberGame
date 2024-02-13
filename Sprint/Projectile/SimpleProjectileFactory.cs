using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint.Projectile
{
    internal class SimpleProjectileFactory
    {
        private Texture2D itemSheet;
        Vector2 position;
        Vector2 direction;

        public SimpleProjectileFactory()
        {
         
        }

        public void LoadAllTextures(ContentManager content)
        {
            itemSheet = content.Load<Texture2D>("zelda_items");
        }

        public Arrow CreateArrow()
        {
            return new Arrow(itemSheet, position, direction);
        }

        // TODO: implement other types of projectile
        public BlueArrow CreateBlueArrow()
        {
            return new BlueArrow(itemSheet, position, direction);
        }

        
        public Bomb CreateBomb()
        {
            return new Bomb(itemSheet, position, direction); 
        }
        
        public Boomarang CreateBoomarang()
        { 
            return new Boomarang(itemSheet, position, direction);
        }

        public void SetDirection(Vector2 direction)
        {
            this.direction = direction;
        }

        public void SetStartPosition(Vector2 pos)
        {
            position = pos;
        }
    }
}
