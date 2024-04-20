using Sprint.Characters;
using Sprint.Interfaces;

namespace Sprint.Functions
{
    internal class StopMoving : ICommand
    {
        private Player player;

        public StopMoving(Player player)
        {
            this.player = player;
        }

        public void Execute()
        {
            this.player.StopMoving();
        }
    }

    internal class StopMovingLeftRight : ICommand
    {
        private Player player;

        public StopMovingLeftRight(Player player)
        {
            this.player = player;
        }

        public void Execute()
        {
            this.player.StopMovingLeftRight();
        }
    }

    internal class StopMovingUpDown : ICommand
    {
        private Player player;

        public StopMovingUpDown(Player player)
        {
            this.player = player;
        }

        public void Execute()
        {
            this.player.StopMovingUpDown();
        }
    }
}
