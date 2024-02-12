using Sprint.Interfaces;

namespace Sprint.Commands
{
    internal class Reset : ICommand
    {
        private Game1 game;

        public Reset(Game1 game)
        {
            this.game = game;
        }

        public void Execute()
        {
            this.game.ResetReq();

        }
    }
}