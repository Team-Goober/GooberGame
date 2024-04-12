using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;

namespace Sprint.Loader
{
    internal class HUDPowerup : IHUD
    {
        private IPowerup powerup;
        private Vector2 position;

        public HUDPowerup(IPowerup powerup, Vector2 position)
        {
            this.powerup = powerup;
            this.position = position;
        }

        public void SetPowerup(IPowerup powerup)
        {
            this.powerup = powerup;
        }

        public IPowerup GetPowerup()
        {
            return powerup;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (powerup!= null)
                powerup.Draw(spriteBatch, position, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            // Nothing Here
        }
    }
}
