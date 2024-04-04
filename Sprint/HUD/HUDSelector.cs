using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using System;

namespace Sprint.Loader
{
    internal class HUDSelector : IHUD
    {
        private ISprite sprite;
        private Vector2 position;
        private string activeAnim;
        private string inactiveAnim;
        private Rectangle firstLocation;

        public HUDSelector(ISprite sprite, Rectangle firstLocation, string active, string inactive)
        {
            this.sprite = sprite;
            this.position = new Vector2(firstLocation.X, firstLocation.Y);
            activeAnim = active;
            inactiveAnim = inactive;
            this.firstLocation = firstLocation;
        }

        public void SetActive(bool a)
        {
            sprite.SetAnimation(a ? activeAnim : inactiveAnim);
        }

        // Set location in selector panel based on point
        public void SetLocation(int row, int column)
        {
            position = new Vector2(firstLocation.Width * column + firstLocation.X, firstLocation.Height * row + firstLocation.Y);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            // Nothing Here
        }
    }
}
