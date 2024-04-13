using Sprint.Characters;
using Sprint.Interfaces;
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
            player.SetSpeed(speed);
        }
    }
}
