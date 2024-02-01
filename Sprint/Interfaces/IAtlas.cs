using Microsoft.Xna.Framework;

namespace Sprint.Interfaces
{
    public interface IAtlas
    {
        // Enable or disable animation looping
        void SetLooping(bool loop);

        // Set number of frames displayed per second
        void SetFramerate(int fps);

        // Get bounds of current frame on spritesheet
        Rectangle CurrentFrame();

        // Check whether frame should be changed each update cycle
        void Update(GameTime gameTime);

        // Return animation to first frame
        void Reset();

    }
}
