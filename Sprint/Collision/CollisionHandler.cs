using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Xna.Framework;
using Sprint.Functions;
using Sprint.Functions.Collision;
using Sprint.Functions.RoomTransition;
using Sprint.Functions.SecondaryItem;
using Sprint.Interfaces;


namespace Sprint.Collision
{
    internal class CollisionHandler
    {

        public readonly struct TypePairKey
        {
            // Represents a dictionary key for two colliding types, where the first type must react
            public TypePairKey(CollisionTypes receptor, CollisionTypes effector)
            {
                Receptor = receptor;
                Effector = effector;
            }

            public CollisionTypes Receptor { get; init; }
            public CollisionTypes Effector { get; init; }

        }

        static Type[] constructorParams = new Type[] { typeof(ICollidable), typeof(ICollidable), typeof(Vector2) };
        static ConstructorInfo pushOut = typeof(PushMoverOut).GetConstructor( constructorParams );
        static ConstructorInfo pushBlockOut = typeof(PushMoverBlock).GetConstructor(constructorParams);

        // Dictionary mapping two collider types, where the first one is passed as a receiver to the command value
        // TODO: Read this from file
        Dictionary<TypePairKey, ConstructorInfo> commandDictionary = new Dictionary<TypePairKey, ConstructorInfo>()
            {
                {new TypePairKey(CollisionTypes.CHARACTER, CollisionTypes.WALL), pushOut},
                {new TypePairKey(CollisionTypes.CHARACTER, CollisionTypes.GAP), pushOut},
                {new TypePairKey(CollisionTypes.CHARACTER, CollisionTypes.DOOR), pushOut},
                {new TypePairKey(CollisionTypes.MOVEWALL, CollisionTypes.CHARACTER), pushOut},
                {new TypePairKey(CollisionTypes.MOVEWALL, CollisionTypes.WALL), pushOut},




                {new TypePairKey(CollisionTypes.PROJECTILE, CollisionTypes.WALL), typeof(DissipateProjectile).GetConstructor( constructorParams ) },
                {new TypePairKey(CollisionTypes.PROJECTILE, CollisionTypes.DOOR), typeof(DissipateProjectile).GetConstructor( constructorParams ) },
                
                {new TypePairKey(CollisionTypes.ENEMY_PROJECTILE, CollisionTypes.WALL), typeof(DissipateProjectile).GetConstructor( constructorParams ) },
                {new TypePairKey(CollisionTypes.ENEMY_PROJECTILE, CollisionTypes.DOOR), typeof(DissipateProjectile).GetConstructor( constructorParams ) },

                {new TypePairKey(CollisionTypes.OPEN_DOOR, CollisionTypes.PLAYER), typeof(SwitchRoomCommand).GetConstructor( constructorParams ) },
                {new TypePairKey(CollisionTypes.HIDDEN_DOOR, CollisionTypes.EXPLOSION), typeof(OpenDoorCommand).GetConstructor( constructorParams ) },
                {new TypePairKey(CollisionTypes.PLAYER,CollisionTypes.LOCKED_DOOR), typeof(OpenLockedDoorCommand).GetConstructor(constructorParams)},

                {new TypePairKey(CollisionTypes.PLAYER, CollisionTypes.ITEM), typeof(PickUpItem).GetConstructor( constructorParams ) },

                {new TypePairKey(CollisionTypes.ENEMY, CollisionTypes.SWORD), typeof(KillCommand).GetConstructor( constructorParams ) },
                {new TypePairKey(CollisionTypes.PLAYER, CollisionTypes.ENEMY_PROJECTILE), typeof(KillCommand).GetConstructor( constructorParams ) },
                {new TypePairKey(CollisionTypes.PLAYER, CollisionTypes.ENEMY), typeof(KillCommand).GetConstructor( constructorParams ) },
                {new TypePairKey(CollisionTypes.ENEMY, CollisionTypes.PROJECTILE), typeof(KillCommand).GetConstructor( constructorParams ) },
                {new TypePairKey(CollisionTypes.PROJECTILE, CollisionTypes.ENEMY), typeof(DissipateProjectile).GetConstructor( constructorParams ) },
                {new TypePairKey(CollisionTypes.ENEMY_PROJECTILE, CollisionTypes.PLAYER), typeof(DissipateProjectile).GetConstructor( constructorParams ) }
            };

        //Made assuming that ICollidable can access the objects native type

        /// <summary>
        /// Takes Collision and maps to function call to handle
        /// </summary>
        /// <param name="object1">First colliding obj</param>
        /// <param name="object2">Second colliding obj</param>
        /// <param name="overlap">Overlap from obj1 to obj2</param>
        public void HandleCollision(ICollidable object1, ICollidable object2, Vector2 overlap)
        {

            // Handle object1 reaction
            FindInteraction(object1, object2, overlap);

            // Handle object2 reaction
            FindInteraction(object2, object1, -overlap);
        }

        /// <summary>
        /// Finds the correct interaction in the command dictionary for two collidables, then runs it
        /// </summary>
        /// <param name="receiver">The collidable to be affected by the command</param>
        /// <param name="effector">The collidable that is not affected by the command</param>
        /// <param name="overlap">The amount of overlap, measured from the receiver towards the effector</param>
        public void FindInteraction(ICollidable receiver, ICollidable effector, Vector2 overlap)
        {
            // each possible "most precise" interaction
            // first int is index of receiver's collision types, second int is for effector
            List<int[]> possibleInteractions = new();

            for(int i=0; i<receiver.CollisionType.Length; i++)
            {
                for (int j = 0; j < effector.CollisionType.Length; j++)
                {
                    CollisionHandler.TypePairKey key = new CollisionHandler.TypePairKey(receiver.CollisionType[i], effector.CollisionType[j]);
                    // test if key exists
                    if (commandDictionary.ContainsKey(key))
                    {
                        // record working key and break, as any larger j values are less precise than this one
                        possibleInteractions.Add(new int[] { i, j });
                        break;
                    }
                }
            }

            if (possibleInteractions.Count == 0)
            {
                // no collision interaction
                return;
            }

            // Assert that none of the interactions have a higher i but smaller j than another, as this would 
            // create ambiguity
            for(int k=1; k<possibleInteractions.Count; k++)
            {
                Debug.Assert(possibleInteractions[k][1] >= possibleInteractions[k - 1][1]);
            }

            // The first item in the array should have the lowest i and therefore be most precise
            CollisionHandler.TypePairKey preciseKey = new CollisionHandler.TypePairKey(receiver.CollisionType[possibleInteractions[0][0]], effector.CollisionType[possibleInteractions[0][1]]);

            CreateAndRun(commandDictionary[preciseKey], receiver, effector, overlap);

        }

        /// <summary>
        /// Creates the given command and executes it
        /// </summary>
        /// <param name="commandConstructor">Constructor for command to execute</param>
        /// <param name="receiver">Collidable to execute the command on</param>
        /// <param name="effector">Collidable to receive the command.</param>
        /// <param name="overlap">Overlap distance, measured out from receiver</param>
        public void CreateAndRun(ConstructorInfo commandConstructor, ICollidable receiver, ICollidable effector, Vector2 overlap)
        {
            // Construct command then execute
            ICommand c = commandConstructor.Invoke(new object[] { receiver, effector, overlap }) as ICommand;
            c.Execute();
        }
    }
}
