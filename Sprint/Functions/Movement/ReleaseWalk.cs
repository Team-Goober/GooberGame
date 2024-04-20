using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Interfaces;

namespace Sprint.Functions
{
    internal class ReleaseWalk : ICommand
    {
        private Player player;
        private Vector2 direction;
        public ReleaseWalk(Player player, Vector2 direction)
        {
            this.player = player;
            this.direction = direction;
        }

        public void Execute()
        {
            this.player.EndWalk(direction);
        }
    }
}
