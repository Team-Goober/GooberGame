using Sprint.Characters;
using Sprint.Interfaces;
using Microsoft.Xna.Framework;

namespace Sprint.Functions
{
    internal class Walk : ICommand
    {
        private Player player;
        private Vector2 direction;
        public Walk(Player player, Vector2 direction)
        {
            this.player = player;
            this.direction = direction;
        }

        public void Execute()
        {
            // Execute the movement action
            this.player.Walk(direction);
        }
    }
}
