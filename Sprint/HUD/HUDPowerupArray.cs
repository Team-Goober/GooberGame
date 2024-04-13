using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sprint.Loader
{
    internal class HUDPowerupArray : IHUD
    {
        private IPowerup[,] powerups;
        private Vector2 firstCell;
        private Vector2 offsets;

        public HUDPowerupArray(Vector2 firstCell, Vector2 offsets)
        {
            this.firstCell = firstCell;
            this.offsets = offsets;
        }

        public void SetPowerups(IPowerup[,] powerups)
        {
            this.powerups = powerups;
        }

        public void SetSinglePowerup(IPowerup powerup)
        {
            this.powerups = new IPowerup[1, 1];
            this.powerups[0, 0] = powerup;
        }

        public IPowerup[,] GetPowerups()
        {
            return powerups;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (powerups != null)
            {
                for(int i = 0; i < powerups.GetLength(0); i++)
                {
                    for (int j = 0; j < powerups.GetLength(1); j++)
                    {
                        if (powerups[i, j] != null)
                        {
                            powerups[i, j].Draw(spriteBatch, firstCell + offsets * new Vector2(j, i), gameTime);
                        }
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            if (powerups != null)
            {
                for (int i = 0; i < powerups.GetLength(0); i++)
                {
                    for (int j = 0; j < powerups.GetLength(1); j++)
                    {
                        if (powerups[i, j] != null)
                        {
                            powerups[i, j].Update(gameTime);
                        }
                    }
                }
            }
        }
    }
}
