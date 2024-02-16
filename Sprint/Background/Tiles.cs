using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint
{
    internal class Tiles : Character
    {
        ISprite sprite;
        Physics physics;

        public Tiles(Goober game, ISprite sprite, Vector2 position)
        {
            this.sprite = sprite;
            physics = new Physics(game, position);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, physics.Position, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            physics.Update(gameTime);
            sprite.Update(gameTime);
        }
    }
}
