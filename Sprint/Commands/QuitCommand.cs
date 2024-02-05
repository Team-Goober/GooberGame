using Microsoft.Xna.Framework;
using Sprint.Interfaces;

namespace Sprint.Commands
{

    public class QuitCommand : ICommand
    {
        private Game receiver;

        public QuitCommand(Game game)
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
