using Sprint.Interfaces;

namespace Sprint.Commands
{

    public class QuitCommand : ICommand
    {
        private Game1 receiver;

        public QuitCommand(Game1 game)
        {
            receiver = game;
        }

        public void Execute()
        {
            // Exit game
            receiver.Exit();
        }
    }
}
