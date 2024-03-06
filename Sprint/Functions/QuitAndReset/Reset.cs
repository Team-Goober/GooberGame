using Sprint.Interfaces;

namespace Sprint.Commands
{
    internal class Reset : ICommand
    {
        private Goober game;

        public Reset(Goober game)
        {
            this.game = game;
        }

        public void Execute()
        {
            this.game.ResetReq();

        }
    }
}