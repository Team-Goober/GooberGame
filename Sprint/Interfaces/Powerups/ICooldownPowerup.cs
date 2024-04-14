using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Interfaces.Powerups
{
    internal interface ICooldownPowerup : IPowerup
    {

        public void SetDuration(float duration);
        public void SetTimeLeft(float duration);
        public float GetTimeLeft();

    }
}
