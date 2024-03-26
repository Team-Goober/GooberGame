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

            //if (receiver is MoveWallTile tile && effector is Player link)
            //{



            //    //get center points of the rectangle for tile
            //    int tileCenterX = tile.GetBoundingBox().X + (tile.GetBoundingBox().Right - tile.GetBoundingBox().X) / 2;
            //    int tileCenterY = tile.GetBoundingBox().Y + (tile.GetBoundingBox().Bottom - tile.GetBoundingBox().Y) / 2;


            //    Vector2 wallTile = new Vector2(tileCenterX, tileCenterY);

            //    //gets center points for rectangle of link
            //    int linkCenterX = link.BoundingBox.X + (link.BoundingBox.Right - tile.BoundingBox.X) / 2;
            //    int linkCenterY = link.BoundingBox.Y + (link.BoundingBox.Right - tile.BoundingBox.Y) / 2;

            //    Vector2 linkTile = new Vector2(linkCenterX, linkCenterY);

            //    Vector2 calcDiff = wallTile -linkTile;

            //    distance = calcDiff * overlap;


            //}
            


            distance = -overlap * 5;
            
        }

        public void Execute()
        {
            // Moves receiver by displacement
            receiver.Move(distance);
        }
    }
}
