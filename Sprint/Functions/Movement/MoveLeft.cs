using Sprint.Characters;
using Sprint.Interfaces;

namespace Sprint.Functions
{
    internal class MoveLeft : ICommand
    {
        private Player player;

        public MoveLeft(Player player)
        {
            this.player = player;
        }

        public void Execute()
        {
            // Execute the movement action
            this.player.MoveLeft();
        }

        // Inner class to handle key release
        internal class ReleaseLeft : ICommand
        {
            private Player player;

            public ReleaseLeft(Player player)
            {
                this.player = player;
            }

            public void Execute()
            {
                // Execute the release action
                this.player.ReleaseLeft();
            }
        }
    }
}
