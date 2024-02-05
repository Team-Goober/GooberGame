using Microsoft.Xna.Framework;
using Sprint.Interfaces;

namespace Sprint.Sprite
{
    internal class SingleAtlas : IAtlas
    {

        private Vector2 center;
        private Rectangle rect;

        public SingleAtlas(Rectangle rectangle, Vector2 centerPoint)
        {
            this.rect = rectangle;
            this.center = centerPoint;
        }

        public Vector2 CurrentCenterPoint()
        {
            return center;
        }

        public float CurrentDuration()
        {
            // Return zero, as static sprites have no timing
            return 0;
        }

        public Rectangle CurrentFrame()
        {
            return rect;
        }

        public void Reset()
        {
            // Nothing to reset
        }

        public void SetFramerate(float fps)
        {
            // Framerate is not relevant
        }

        public void SetLooping(bool loop)
        {
            // Looping is not relevant
        }

        public void Update(GameTime gameTime)
        {
            // No need to update
        }
    }
}
