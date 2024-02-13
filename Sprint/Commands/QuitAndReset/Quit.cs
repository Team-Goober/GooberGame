using Sprint.Interfaces;

namespace Sprint.Commands
{
    internal class Quit : ICommand
    {
        private Game1 game;

        public Quit(Game1 game)
        {
            this.game = game;
        }

        public void Execute()
        {
            this.game.Exit();

        }
    }
}