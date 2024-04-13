using Sprint.GameStates;
using Sprint.Interfaces;

namespace Sprint.Functions.States
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
