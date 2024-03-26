using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Characters;

namespace Sprint.Functions.Collision
{
    internal class PushMoverOut : ICommand
    {

        private IMovingCollidable receiver; // moving collidable to be pushed
        private Vector2 distance; // displacement to push over

        public PushMoverOut(ICollidable receiver, ICollidable effector, Vector2 overlap) {
            this.receiver = (IMovingCollidable)receiver;
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
