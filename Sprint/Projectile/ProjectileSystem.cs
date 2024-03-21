using Sprint.Interfaces;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint.Commands.SecondaryItem;
using Sprint.Input;
using Sprint.Characters;
using Sprint.Sprite;
using Sprint.Levels;

namespace Sprint.Projectile
{
    internal class ProjectileSystem
    {

        private SimpleProjectileFactory itemFactory;

        private const float spawnDistance = 40;

        public ProjectileSystem(Vector2 startPos, IInputMap inputTable, SpriteLoader spriteLoader)
        {

            this.itemFactory = new SimpleProjectileFactory(spriteLoader, spawnDistance, false, null);
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


        public void SetScene(SceneObjectManager scene)
        {
            itemFactory.SetScene(scene);
        }

        public void UpdateDirection(Character.Directions dir)
        {
            

            switch (dir)
            {
                case Character.Directions.LEFT:
                    itemFactory.SetDirection(new Vector2(-1, 0));
                    break;
                case Character.Directions.RIGHT:
                    itemFactory.SetDirection(new Vector2(1, 0));
                    break;
                case Character.Directions.UP:
                    itemFactory.SetDirection(new Vector2(0, -1));
                    break;
                case Character.Directions.DOWN:
                    itemFactory.SetDirection(new Vector2(0, 1));
                    break;
                default: break;
            }
        }

        public void UpdatePostion(Vector2 pos)
        {
            itemFactory.SetStartPosition(pos);
        }

    }
}
