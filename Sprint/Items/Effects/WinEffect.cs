using Sprint.Characters;
using Sprint.Interfaces.Powerups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Items.Effects
{
    internal class WinEffect : IEffect
    {

        public void Execute(Player player)
        {
            // Tell player to win
            player.Win();
        }
    }
}
