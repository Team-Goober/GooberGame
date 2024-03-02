using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            // Check all movers as collision instigators
            for (int i = 0; i < movingObjects.Count; i++)
            {
                // Check only the movers after this one in the list (to avoid duplicate checks)
                for (int k = i + 1; k < movingObjects.Count; k++)
                {
                    CheckCollision(movingObjects[i], movingObjects[k]);
                }

                // Check against all static objects
                for (int j = 0; j < staticObjects.Count; j++)
                {
                    CheckCollision(movingObjects[i], staticObjects[j]);
                }
            }
        }

        public void CheckCollision(IMovingCollidable movingElement, ICollidable element)
        {
            Rectangle movingElementBoundingBox = movingElement.GetBoundingBox();
            Rectangle elementBoundingBox = element.GetBoundingBox();

            // If bounding boxes intersect, handle it
            if (movingElementBoundingBox.Intersects(elementBoundingBox))
            {
                FindCollisionType(movingElement, element);
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

            // Get Intersection of boxes
            Rectangle intersection = Rectangle.Intersect(movingElementBoundingBox, elementBoundingBox);
            Directions collisionDirection = new();
            Vector2 overlap;


            //Left Right collision check
            if ( intersection.Height > intersection.Width )
            {
                if (movingElementBoundingBox.X > elementBoundingBox.X)
                {
                    collisionDirection = Directions.LEFT;
                    overlap = new Vector2(-intersection.Width, 0);
                }else{
                    collisionDirection = Directions.RIGHT;
                    overlap = new Vector2(intersection.Width, 0);
                }

            }
            else //Top Bottom Collision Check
            {
                if (movingElementBoundingBox.Y > elementBoundingBox.Y)
                {
                    collisionDirection = Directions.UP;
                    overlap = new Vector2(0, -intersection.Height);
                }
                else
                {
                    collisionDirection = Directions.DOWN;
                    overlap = new Vector2(0, intersection.Height);
                }
            }
            collisionHandler.HandleCollision(movingElement, element, overlap);
            
        }
    }
}
