using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Collision;
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

        public Rectangle BoundingBox => bounds;

        public CollisionTypes[] CollisionType => new CollisionTypes[] { CollisionTypes.WALL };

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




    }
}
