using Sprint.HUD;
using Sprint.Interfaces;
using Sprint.Interfaces.Powerups;
using Sprint.Loader;
using System.Diagnostics;
using System.Numerics;

namespace Sprint.Functions.SecondaryItem
{
    internal class SetDescriptiveTextCommand : ICommand
    {

        HUDText receiver;
        HUDPowerupArray array;
        int r, c;

        public SetDescriptiveTextCommand(HUDText receiver, HUDPowerupArray array, int r, int c)
        {
            this.receiver = receiver;
            this.array = array;
            this.r = r;
            this.c = c;
        }

        public void Execute()
        {
            IPowerup p = array.GetPowerups()[r, c];
            if (p == null)
            {
                receiver.SetText("--HOVER ITEM--");
            }
            else
            {
                receiver.SetText(p.GetDescription());
            }
        }
    }
}
