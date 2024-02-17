using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Characters;

namespace Sprint
{
    internal class Tiles : Character
    {
        ISprite sprite;
        Vector2 position;

        public Tiles(Goober game, ISprite sprite, Vector2 position)
        {
            this.sprite = sprite;
            this.position = position;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }
    }
}
