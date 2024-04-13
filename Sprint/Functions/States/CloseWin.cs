using Sprint.GameStates;
using Sprint.Interfaces;

namespace Sprint.Functions.WinStateCommands
{
    internal class CloseWin : ICommand
    {
        public WinState receiver;

        public CloseWin(WinState receiver) 
        {
            this.receiver = receiver;
        }

        public void Execute()
        {
            receiver.CloseWin();
        }
    }
}
