using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Interfaces
{
    public interface ISprite
    { 
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch, Vector2 location, string animation);
        void DrawFrozen(SpriteBatch spriteBatch, Vector2 location);
        void DrawRunning(SpriteBatch spriteBatch, Vector2 location);
    }
}
