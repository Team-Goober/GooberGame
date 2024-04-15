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

        float speed;

        public ChangeSpeedEffect(float speed)
        { 
            this.speed = speed;
        }

        public void Execute(Player player)
        {
            // Set player speed to new value
            player.SetSpeed(speed);
        }
    }
}
