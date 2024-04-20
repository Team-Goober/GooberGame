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

        // Tries to use up one and returns whether it succeeded
        public bool ReadyConsume(int amount);

        // Adds a quantity to the stack
        public void AddAmount(int amount);

        // Return size of stack
        public int Quantity();

        // Sets whether the stack should allow subtraction
        public void SetUnlimited(bool unlimited);

        // Gets whether the stack should allow subtraction
        public bool GetUnlimited();
    }
}
