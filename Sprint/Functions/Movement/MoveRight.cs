using Sprint.Characters;
using Sprint.Interfaces;

namespace Sprint.Functions
{
    internal class MoveRight : ICommand
    {
        private Player player;

        public MoveRight(Player player)
        {
            this.player = player;
        }

        public void Execute()
        {
            // Execute the movement action
            this.player.MoveRight();
        }

        // Inner class to handle key release
        internal class ReleaseRight : ICommand
        {
            private Player player;

            public ReleaseRight(Player player)
            {
                this.player = player;
            }

            public void Execute()
            {
                // Execute the release action
                this.player.ReleaseRight();
            }
        }
    }
}
