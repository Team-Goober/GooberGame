using Microsoft.Xna.Framework;
using Sprint.Interfaces;

namespace Sprint.Functions.Collision
{
    internal class PushMoverBlock : ICommand
    {

        private IMovingCollidable receiver; // moving collidable to be pushed
        private Vector2 distance; // displacement to push over

        public PushMoverBlock(ICollidable receiver, ICollidable effector, Vector2 overlap) {
            this.receiver = (IMovingCollidable)receiver;
            // overlap is directed into the static collider; we want to move outwards
            distance = -overlap * overlap;
            
        }

        public void Execute()
        {
            // Moves receiver by displacement
            receiver.Move(distance);
        }
    }
}
