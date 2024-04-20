namespace XMLData
{
    public class PowerupData
    {

        public string Label;
        public string Description;
        public string Sprite;

        public string Effect;
        public ParameterPair[] EffectParams;
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

    public struct ParameterPair
    {
        public string Name;
        public object Value;
    }

}
