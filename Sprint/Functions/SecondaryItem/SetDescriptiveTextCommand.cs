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
            // If slot is empty, set to default text
            if (p == null)
            {
                receiver.SetText("--HOVER ITEM--");
            }
            // If slot is filled, use that powerup's description as text
            else
            {
                receiver.SetText(p.GetDescription());
            }
        }
    }
}
