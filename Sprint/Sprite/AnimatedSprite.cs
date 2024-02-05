using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sprint.Sprite
{
    internal class AnimatedSprite : ISprite
    {

        private Texture2D texture;
        private float scale = 1.0f;
        private Dictionary<string, IAtlas> animations;
        private IAtlas currentAnimation;

        public AnimatedSprite(Texture2D texture)
        {
            this.texture = texture;
            animations = new Dictionary<string, IAtlas>();
        }

        public void SetScale(float scale)
        {
            this.scale = scale;
        }

        public void RegisterAnimation(string label, IAtlas atlas)
        {
            animations[label] = atlas;
        }

        public void SetAnimation(string label)
        {
            currentAnimation = animations[label];
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, GameTime gameTime)
        {
            // No animation set, so don't draw
            if (currentAnimation == null) {
                return;
            }

            currentAnimation.PassTime(gameTime);

            // Get spritesheet bounds of current frame from atlas
            Rectangle sourceRectangle = currentAnimation.CurrentFrame();
            // Calculate game position to draw sprite at
            Vector2 drawPos = location - currentAnimation.CurrentCenterPoint() * scale;
            Rectangle destinationRectangle = new Rectangle((int)(drawPos.X), (int)(drawPos.Y), (int)(scale * sourceRectangle.Width), (int)(scale * sourceRectangle.Height));

            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
        }

    }
}
