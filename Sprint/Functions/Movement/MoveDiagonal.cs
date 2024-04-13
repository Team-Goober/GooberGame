using Sprint.Characters;
using Microsoft.Xna.Framework;
using System;
using Sprint.Interfaces;

namespace Sprint.Commands
{
    internal class MoveDiagonal : ICommand
    {
        private Player player;
        private Vector2 direction;

        public MoveDiagonal(Player player, Vector2 direction)
        {
            this.player = player;
            this.direction = direction;
        }

        public void Execute()
        {
            float diagonalSpeed = CharacterConstants.PLAYER_SPEED / (float)(Math.Sqrt(2) * 64); // Diagonal movement speed
            Vector2 movementVector = direction;
            movementVector.Normalize();
            player.Move(movementVector * diagonalSpeed);
        }
    }

    internal class MoveDiagonalUpRight : MoveDiagonal
    {
        public MoveDiagonalUpRight(Player player) : base(player, new Vector2(1, -1)) { }
    }

    internal class MoveDiagonalUpLeft : MoveDiagonal
    {
        public MoveDiagonalUpLeft(Player player) : base(player, new Vector2(-1, -1)) { }
    }

    internal class MoveDiagonalDownRight : MoveDiagonal
    {
        public MoveDiagonalDownRight(Player player) : base(player, new Vector2(1, 1)) { }
    }

    internal class MoveDiagonalDownLeft : MoveDiagonal
    {
        public MoveDiagonalDownLeft(Player player) : base(player, new Vector2(-1, 1)) { }
    }
}
