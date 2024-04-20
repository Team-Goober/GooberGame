using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Interfaces.Powerups;
using System;
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

        // Attach array of powerups to draw
        public void SetPowerups(IPowerup[,] powerups)
        {
            this.powerups = powerups;
        }

        // Set array to contain only one powerup
        public void SetSinglePowerup(IPowerup powerup)
        {
            this.powerups = new IPowerup[1, 1];
            this.powerups[0, 0] = powerup;
        }

        public IPowerup[,] GetPowerups()
        {
            return powerups;
        }

        // Return scene position at a specific cell
        public Vector2 PositionAt(int r, int c)
        {
            return firstCell + offsets * new Vector2(c, r);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (powerups != null)
            {
                for(int i = 0; i < powerups.GetLength(0); i++)
                {
                    for (int j = 0; j < powerups.GetLength(1); j++)
                    {
                        // Draw all filled cells at their scene location
                        if (powerups[i, j] != null)
                        {
                            powerups[i, j].Draw(spriteBatch, PositionAt(i, j), gameTime);
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
