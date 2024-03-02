using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Sprite;
using static Sprint.Characters.Character;

namespace Sprint.Collision
{
    internal class CollisionDetector
    {
        CollisionHandler collisionHandler;

        public CollisionDetector() {
        
            collisionHandler = new CollisionHandler();
        }

        /// <summary>
        /// Iterates game objects to check for collision
        /// </summary>
        /// <param name="gt"></param>
        /// <param name="movingObjects"></param>
        /// <param name="staticObjects"></param>
        public void Update(GameTime gt, List<IMovingCollidable> movingObjects, List<ICollidable> staticObjects )
        {

            foreach ( var movingElement in movingObjects)
            {
                Rectangle movingElementBoundingBox = movingElement.GetBoundingBox();

                List<ICollidable> iteratorList = staticObjects;
                iteratorList.AddRange(movingObjects);

                List<ICollidable> checkedItems = new List<ICollidable>();
                checkedItems.Add(movingElement);

                foreach (var element in iteratorList)
                {
                    if( checkedItems.Contains(element))
                          continue; 
                    
                    Rectangle elementBoundingBox = element.GetBoundingBox();

                    if (movingElementBoundingBox.Intersects(elementBoundingBox))
                    {
                        FindCollisionType(movingElement, element);
                    }

                }
            }
        }

        /// <summary>
        /// Used to find the collision direction, calls handler
        /// </summary>
        /// <param name="movingElement"></param>
        /// <param name="element"></param>
        public void FindCollisionType(IMovingCollidable movingElement, ICollidable element)
        {
            Rectangle movingElementBoundingBox = movingElement.GetBoundingBox();
            Rectangle elementBoundingBox = element.GetBoundingBox();

            Rectangle intersection = Rectangle.Union(movingElementBoundingBox, elementBoundingBox);
            Directions collisionDirection = new();
            Vector2 overlap = Vector2.Zero;

            //Left Right collision check
            if ( intersection.Height > intersection.Width )
            {
                if (movingElementBoundingBox.X > elementBoundingBox.X)
                {
                    collisionDirection = Directions.LEFT;
                    overlap = new Vector2(intersection.Width, 0);
                }else{
                    collisionDirection = Directions.RIGHT;
                    overlap = new Vector2(-intersection.Width, 0);
                }

            }
            else //Top Bottom Collision Check
            {
                if (movingElementBoundingBox.Y > elementBoundingBox.Y)
                {
                    collisionDirection = Directions.UP;
                    overlap = new Vector2(0, intersection.Height);
                }
                else
                {
                    collisionDirection = Directions.DOWN;
                    overlap = new Vector2(0, -intersection.Height);
                }
            }
            collisionHandler.HandleCollision(movingElement, element, overlap);
            
        }
    }
}
