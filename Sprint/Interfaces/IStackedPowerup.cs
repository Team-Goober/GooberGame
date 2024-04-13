using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Interfaces
{
    internal interface IStackedPowerup : IPowerup
    {

        public void AddAmount(int amount);

        public int Quantity();
    }
}
