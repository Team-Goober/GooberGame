using Sprint.Interfaces;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint.Commands.SecondaryItem;
using Sprint.Input;
using Sprint.Characters;
using Sprint.Sprite;
using Sprint.Levels;
using System.Collections.Generic;

namespace Sprint.Projectile
{
    internal class ProjectileSystem
    {
        private SimpleProjectileFactory itemFactory;

        private const float spawnDistance = 40;

        //Dictinaries to replace switch statements
        private Dictionary<Character.Directions, Vector2> SwordAnimDict = new Dictionary<Character.Directions, Vector2>
        {
            { Character.Directions.RIGHT, new Vector2(1, 0) },
            { Character.Directions.LEFT, new Vector2(-1, 0) },
            { Character.Directions.UP, new Vector2(0, -1) },
            { Character.Directions.DOWN, new Vector2(0, 1) }
        };

        public ProjectileSystem(Vector2 startPos, IInputMap inputTable, GameObjectManager objManager, SpriteLoader spriteLoader)
        {

            this.itemFactory = new SimpleProjectileFactory(spriteLoader, spawnDistance, false,objManager);
            itemFactory.SetDirection(new Vector2(1, 90));
            itemFactory.SetStartPosition(startPos);

            //Arrow
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D1), new ShootArrowCommand(itemFactory));
            

            //Blue Arrow
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D2), new ShootBlueArrowC(itemFactory));

            
            //Bomb
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D3), new ShootBombC(itemFactory));


            //Boomarang
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D4), new ShootBoomarangC(itemFactory));

            //FireBall
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D5), new ShootFireBallC(itemFactory));

            //Blue Boomerang
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D6), new ShootBlueBoomerangC(itemFactory));



        }

        public void UpdateDirection(Character.Directions dir)
        {



            if (SwordAnimDict.TryGetValue(dir, out Vector2 direction))
            {

                itemFactory.SetDirection(direction);

            }

        }

        public void UpdatePostion(Vector2 pos)
        {
            itemFactory.SetStartPosition(pos);
        }

    }
}
