using Microsoft.Xna.Framework;

namespace Sprint.Interfaces
{
    public interface IAtlas
    {
        // Enable or disable animation looping
        void SetLooping(bool loop);

        // Set number of frames displayed per second
        void SetFramerate(float fps);

        // Position within rectangle to treat as center for drawing
        public Vector2 CurrentCenterPoint();

        // Multiplier for amount of time to linger on this frame
        public float CurrentDuration();

        // Get bounds of current frame on spritesheet
        Rectangle CurrentFrame();

        // Check whether frame should be changed each update cycle
        void Update(GameTime gameTime);

        // Return animation to first frame
        void Reset();

    }
}
