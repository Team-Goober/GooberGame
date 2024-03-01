using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Sprite;
using static Sprint.Characters.Character;

namespace Sprint.Collision
{
    internal class CollisionHandler
    {
        public CollisionHandler() {}

        public void Update(GameTime gt, List<ICollidable> movingObjects, List<ICollidable> staticObjects )
        {

            foreach ( var movingElement in movingObjects)
            {
                Rectangle movingElementBoundingBox = movingElement.GetBoundingBox();

                List<ICollidable> iteratorList = staticObjects;
                iteratorList.AddRange(iteratorList);

                foreach (var element in iteratorList)
                {
                    Rectangle elementBoundingBox = element.GetBoundingBox();

                    if (movingElementBoundingBox.Intersects(elementBoundingBox))
                    {
                        FindCollisionType(movingElement, element);
                    }
                }
            }
        }

        public void FindCollisionType(ICollidable movingElement, ICollidable element)
        {
            Rectangle movingElementBoundingBox = movingElement.GetBoundingBox();
            Rectangle elementBoundingBox = element.GetBoundingBox();

            CollisionDetector collisionDetector = new CollisionDetector();
            Rectangle intersection = Rectangle.Union(movingElementBoundingBox, elementBoundingBox);
            Directions collisionDirection = new();

            //Left Right collision check
            if ( intersection.Height > intersection.Width )
            {
                if (movingElementBoundingBox.X > elementBoundingBox.X)
                    collisionDirection = Directions.RIGHT;
                if (movingElementBoundingBox.X < elementBoundingBox.X)
                    collisionDirection = Directions.LEFT;
            }
            else //Top Bottom Collision Check
            {
                if (movingElementBoundingBox.Y > elementBoundingBox.Y)
                    collisionDirection = Directions.UP;
                if (movingElementBoundingBox.Y < elementBoundingBox.Y)
                    collisionDirection = Directions.DOWN;
            }
            collisionDetector.HandleCollision(movingElement, element, collisionDirection);
            
        }
    }
}
