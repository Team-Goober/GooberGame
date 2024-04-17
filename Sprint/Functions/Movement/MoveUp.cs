using Sprint.Characters;
using Sprint.Interfaces;


namespace Sprint.Functions
{
    internal class MoveUp : ICommand
    {
        private Player player;

        public MoveUp(Player player)
        {
            this.player = player;
        }

        public void Execute()
        {
            this.player.MoveUp();
        }
    }
}