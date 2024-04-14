using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Interfaces.Powerups
{
    internal interface ICooldownPowerup : IPowerup
    {

        public void SetDuration(double duration);
        public void SetTimeLeft(double duration);
        public float GetTimeLeft();

    }
}
