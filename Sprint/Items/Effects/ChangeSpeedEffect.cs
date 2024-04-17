using Sprint.Characters;
using Sprint.Interfaces.Powerups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Items.Effects
{
    internal class ChangeSpeedEffect : IEffect
    {

        public float speedChange;

        public void Execute(Player player)
        {
            // Increment player speed
            player.AddSpeed(speedChange);
        }

        public void Reverse(Player player)
        {
            // Decrement player speed
            player.AddSpeed(-speedChange);
        }

        public IEffect Clone()
        {
            return new ChangeSpeedEffect() { speedChange = speedChange };
        }
    }
}
