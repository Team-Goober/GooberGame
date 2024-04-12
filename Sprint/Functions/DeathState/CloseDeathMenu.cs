using Sprint.GameStates;
using Sprint.Interfaces;

namespace Sprint.Functions.DeathState
{
    internal class CloseDeathMenu : ICommand
    {
        private GameOverState receiver;

        public CloseDeathMenu(GameOverState receiver) 
        {
            this.receiver = receiver;
        }

        public void Execute()
        {
            receiver.CloseDeathMenu();
        }
    }
}
