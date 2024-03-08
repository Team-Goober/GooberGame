﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sprint.Levels
{
    public class RoomObjectManager
    {

        private List<ICollidable> staticColliders;
        private List<IMovingCollidable> movingColliders;
        private List<IGameObject> objects;

        private Queue<IGameObject> removeQueue;
        private Queue<IGameObject> addQueue;
        private bool clear;

        public RoomObjectManager()
        {
            objects = new List<IGameObject>();
            staticColliders = new List<ICollidable>();
            movingColliders = new List<IMovingCollidable>();
            removeQueue = new Queue<IGameObject>();
            addQueue = new Queue<IGameObject>();
            clear = false;
        }

        public List<IGameObject> GetObjects()
        {
            return objects;
        }

        public List<ICollidable> GetStatics()
        {
            return staticColliders;
        }

        public List<IMovingCollidable> GetMovers()
        {
            return movingColliders;
        }

        public void Add(IGameObject gameObject)
        {
            addQueue.Enqueue(gameObject);
        }

        public void Remove(IGameObject gameObject)
        {
            removeQueue.Enqueue(gameObject);
        }

        // clears all the objects in the arrays
        public void ClearObjects()
        {
            // clear on next cycle end
            clear = true;
        }

        // completes all queued changes
        public void EndCycle()
        {
            while (addQueue.Count > 0)
            {
                // Adds a new game object to the list
                IGameObject gameObject = addQueue.Dequeue();
                if (!objects.Contains(gameObject))
                {
                    objects.Add(gameObject);
                    // add to collision if needed
                    if (gameObject is IMovingCollidable)
                    {
                        IMovingCollidable mc = gameObject as IMovingCollidable;
                        movingColliders.Add(mc);
                    }
                    else if (gameObject is ICollidable)
                    {
                        ICollidable c = gameObject as ICollidable;
                        staticColliders.Add(c);
                    }
                }
                else
                {   // Error message if object is not found
                    System.Diagnostics.Debug.WriteLine("\nThe added object \"" + gameObject + "\" is already in Object Manager!\n");
                }
            }
            while (removeQueue.Count > 0)
            {
                // Removes an existing game object from the list
                IGameObject gameObject = removeQueue.Dequeue();
                if (objects.Contains(gameObject))
                {
                    objects.Remove(gameObject);
                    // remove from collision if needed
                    if (gameObject is IMovingCollidable)
                    {
                        IMovingCollidable mc = gameObject as IMovingCollidable;
                        movingColliders.Remove(mc);
                    }
                    else if (gameObject is ICollidable)
                    {
                        ICollidable c = gameObject as ICollidable;
                        staticColliders.Remove(c);
                    }
                }
                else
                {   // Error message if object is not found
                    System.Diagnostics.Debug.WriteLine("\nThe removed object \"" + gameObject + "\" is not in Object Manager!\n");
                }

            }

            // Clear all list objects
            if (clear)
            {
                objects.Clear();
                staticColliders.Clear();
                movingColliders.Clear();
                clear = false;
            }

        }

    }
}