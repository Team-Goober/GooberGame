
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using System.Diagnostics;
using static Sprint.Characters.Character;

namespace Sprint.Commands.Collision
{
    internal class PushMoverOut : ICommand
    {

        private IMovingCollidable receiver; // moving collidable to be pushed
        private Vector2 distance; // displacement to push over

        public PushMoverOut(IMovingCollidable receiver, Vector2 overlap) {
            this.receiver = receiver;
            // overlap is directed into the static collider; we want to move outwards
            distance = -overlap;
        }

        public void Execute()
        {
            // Moves receiver by displacement
            receiver.Move(distance);
        }
    }
}
