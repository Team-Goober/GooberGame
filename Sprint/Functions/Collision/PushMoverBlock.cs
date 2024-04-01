using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Characters;

namespace Sprint.Functions.Collision
{
    internal class PushMoverBlock : ICommand
    {

        private IMovingCollidable receiver; // moving collidable to be pushed
        private Vector2 distance; // displacement to push over


        public PushMoverBlock(ICollidable receiver, ICollidable effector, Vector2 overlap) {

            // overlap is directed into the static collider; we want to move outwards
            this.receiver = (IMovingCollidable)receiver;
           
            distance = -overlap * 5;
            
        }

        public void Execute()
        {
            // Moves receiver by displacement
            receiver.Move(distance);
        }
    }
}
