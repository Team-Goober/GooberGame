using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using static Sprint.Characters.Character;

namespace Sprint.Collision
{
    internal class CollisionHandler
    {
        
        public CollisionHandler() { }

        //Made assuming that ICollidable can access the objects native type

        /// <summary>
        /// Takes Collision and maps to function call to handle
        /// </summary>
        /// <param name="object1"></param>
        /// <param name="object2"></param>
        /// <param name="direction"></param>
        public void HandleCollision(ICollidable object1, ICollidable object2, Directions direction)
        {
            Dictionary<String, Action> methodDictionary = new Dictionary<String, Action>() 
            {
                //Add collision methods here
                {"Key", methodName }
            };

            var object1NativeType = object1.GetNativeType().ToString();
            var object2NativeType = object2.GetNativeType().ToString();

            String dictionaryKey = object1NativeType + object2NativeType + direction.ToString();
            Action methodToRun;

            methodDictionary.TryGetValue(dictionaryKey,out methodToRun);
            methodToRun.Invoke();
        }


        public void methodName()
        {
            Console.WriteLine("Hi");
        }
    }
}
