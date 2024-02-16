using Sprint.Interfaces;

namespace Sprint.Commands
{
    internal class Quit : ICommand
    {
        private Goober game;

        public Quit(Goober game)
        {
            this.game = game;
        }

        public void Execute()
        {
            this.game.Exit();

        }
    }
}