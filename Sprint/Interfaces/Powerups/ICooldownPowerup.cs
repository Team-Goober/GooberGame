using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Interfaces.Powerups
{
    internal interface ICooldownPowerup : IPowerup
    {
        // Sets duration of cooldown
        public void SetDuration(double duration);
        
        // Sets how much time is left to count down
        public void SetTimeLeft(double duration);

        // Returns the amount of time for the full cooldown
        public float GetDuration();

        // Returns the amount of time left in cooldown
        public float GetTimeLeft();

    }
}
