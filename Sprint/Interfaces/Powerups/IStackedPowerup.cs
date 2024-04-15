using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Interfaces.Powerups
{
    internal interface IStackedPowerup : IPowerup
    {
        // Adds a quantity to the stack
        public void AddAmount(int amount);

        // Return size of stack
        public int Quantity();
    }
}
