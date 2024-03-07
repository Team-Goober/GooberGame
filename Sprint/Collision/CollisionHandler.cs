using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Commands.Collision;
using Sprint.Interfaces;
using Sprint.Levels;


namespace Sprint.Collision
{
    internal class CollisionHandler
    {

        public readonly struct TypePairKey
        {
            // Represents a dictionary key for two colliding types, where the first type must react
            public TypePairKey(Type receptor, Type effector)
            {
                Receptor = receptor;
                Effector = effector;
            }

            public Type Receptor { get; init; }
            public Type Effector { get; init; }

        }

        // Dictionary mapping two collider types, where the first one is passed as a receiver to the command value
        // TODO: Read this from file
        Dictionary<TypePairKey, ConstructorInfo> commandDictionary = new Dictionary<TypePairKey, ConstructorInfo>()
            {
                {new TypePairKey(typeof(Player), typeof(WallTile)), typeof(PushMoverOut).GetConstructor(new Type[] {typeof(IMovingCollidable), typeof(Vector2)})}
            };

        //Made assuming that ICollidable can access the objects native type

        /// <summary>
        /// Takes Collision and maps to function call to handle
        /// </summary>
        /// <param name="object1"></param>
        /// <param name="object2"></param>
        /// <param name="overlap"></param>
        public void HandleCollision(ICollidable object1, ICollidable object2, Vector2 overlap)
        {

            TypePairKey key1 = new TypePairKey(object1.GetType(), object2.GetType());
            TypePairKey key2 = new TypePairKey(object2.GetType(), object1.GetType());

            // Handle object1 reaction
            if (commandDictionary.ContainsKey(key1))
            {
                CreateAndRun(commandDictionary[key1], object1, overlap);
            }

            // Handle object2 reaction
            if (commandDictionary.ContainsKey(key2))
            {
                CreateAndRun(commandDictionary[key2], object2, -overlap);
            }


        }


        public void CreateAndRun(ConstructorInfo commandConstructor, ICollidable receiver, Vector2 overlap)
        {
            // Construct command then execute
            ICommand c = commandConstructor.Invoke(new object[] { receiver, overlap }) as ICommand;
            c.Execute();
        }


    }
}
