using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint.Interfaces
{
    public interface ISprite
    {

        // Set scale multiplier for drawing
        void SetScale(float scale);

        // Register atlas for animation under label
        void RegisterAnimation(string label, IAtlas atlas);

        // Set currently playing animation using label
        void SetAnimation(string label);

        // Draw Sprite to given sprite batch at location
        void Draw(SpriteBatch spriteBatch, Vector2 location, GameTime gameTime);
    }
}
