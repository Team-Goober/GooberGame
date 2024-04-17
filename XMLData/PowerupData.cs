using Sprint;
using Sprint.Interfaces.Powerups;
using System.Reflection;

namespace XMLData
{
    public class PowerupData
    {

        public string Label;
        public string Description;
        public string Sprite;
        public IEffect Effect;
        public string Type;
    }

    public class StackedPowerupData : PowerupData
    {
        public int Quantity;
    }

    public class CooldownPowerupData : PowerupData
    {
        public double Duration;
    }

    public class UpgradePowerupData : PowerupData
    {
        public string[] Bases;
    }

}
