using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Levels
{
    internal class InvisibleWall : IGameObject, ICollidable
    {

        Rectangle bounds;
        private const string ANIM_FILE = "tileAnims";

        public InvisibleWall(Rectangle bounds)
        {
            this.bounds = bounds;
        }
        public Rectangle GetBoundingBox()
        {
            return bounds;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // nothing to draw
        }


        public void Update(GameTime gameTime)
        {
            // no updates
        }



        public void CreateWall()
        {
            //ISprite mapSprite = spriteLoader.BuildSprite(ANIM_FILE, spriteName);
            InvisibleWall wall = new InvisibleWall(bounds);
        }
    }
}
