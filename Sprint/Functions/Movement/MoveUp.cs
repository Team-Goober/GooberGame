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
            // Execute the movement action
            this.player.MoveUp();
        }

        // Inner class to handle key release
        internal class ReleaseUp : ICommand
        {
            private Player player;

            public ReleaseUp(Player player)
            {
                this.player = player;
            }

            public void Execute()
            {
                // Execute the release action
                this.player.ReleaseUp();
            }
        }
    }
}
