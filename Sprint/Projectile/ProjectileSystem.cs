using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint.Commands;
using Sprint.Input;

namespace Sprint.Projectile
{
    internal class ProjectileSystem
    {
        private SimpleProjectileFactory itemFactory;

        public ProjectileSystem(Vector2 startPos, IInputMap inputTable, GameObjectManager objManager, ContentManager contManager)
        {
            // oldLocation = link.spriteLocation;

            this.itemFactory = new SimpleProjectileFactory();
            itemFactory.LoadAllTextures(contManager);
            itemFactory.SetDirection(new Vector2(90, 0));
            itemFactory.SetStartPosition(startPos);

            //Arrow
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D1), new ShootArrowCommand(itemFactory, objManager));
            

            /*//Blue Arrow
            ISprite blueArrowSprite = itemFactory.CreateBlueArrow();
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D2), new ShootArrowCommand(itemFactory, blueArrowSprite, 200));

            //Bomb
            ISprite bombSprite = itemFactory.CreateBomb();
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D3), new ShootArrowCommand(itemFactory, bombSprite, 0));


            //FireBomb
            ISprite boomarangSprite = itemFactory.CreateBoomarang();
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D4), new ShootArrowCommand(itemFactory, boomarangSprite, 200));
            */

        }

        public void UpdateDirection(Character.Directions dir)
        {

            switch(dir)
            {
                case Character.Directions.LEFT:
                    itemFactory.SetDirection(new Vector2(-1, 0));
                    break;
                case Character.Directions.RIGHT:
                    itemFactory.SetDirection(new Vector2(1, 0));
                    break;
                case Character.Directions.UP:
                    itemFactory.SetDirection(new Vector2(1, -90));
                    break;
                case Character.Directions.DOWN:
                    itemFactory.SetDirection(new Vector2(1, 90));
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
