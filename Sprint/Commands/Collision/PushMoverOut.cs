
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using System.Numerics;
using static Sprint.Characters.Character;

namespace Sprint.Commands.Collision
{
    internal class PushMoverOut : ICommand
    {

        private IMovingCollidable receiver;
        private Vector2 distance;

        public PushMoverOut(IMovingCollidable receiver, Vector2 overlap) {
            this.receiver = receiver;
            distance = -overlap;
        }

        public void Execute()
        {
            receiver.Move(distance);
        }
    }
}
