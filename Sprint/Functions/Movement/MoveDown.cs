using Sprint.Characters;
using Sprint.Interfaces;

namespace Sprint.Functions
{
    internal class MoveDown : ICommand
    {
        private Player player;

        public MoveDown(Player player)
        {
            this.player = player;
        }

        public void Execute()
        {
            // Execute the movement action
            this.player.MoveDown();
        }

        // Inner class to handle key release
        internal class ReleaseDown : ICommand
        {
            private Player player;

            public ReleaseDown(Player player)
            {
                this.player = player;
            }

            public void Execute()
            {
                // Execute the release action
                this.player.ReleaseDown();
            }
        }
    }
}
