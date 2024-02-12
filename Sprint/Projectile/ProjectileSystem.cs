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

        private Physics link;

        public ProjectileSystem(SimpleProjectileFactory factory, IInputMap inputTable, Physics character)
        {
            this.link = character;
            // oldLocation = link.spriteLocation;

            this.itemFactory = factory;
            itemFactory.SetDirection(new Vector2(90, 0));
            itemFactory.SetStartPosition(link.Position);

            //Arrow
            ISprite arrowSprite = itemFactory.CreateArrow();
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D1), new ShootCommand(itemFactory, arrowSprite, 200));
            

            //Blue Arrow
            ISprite blueArrowSprite = itemFactory.CreateBlueArrow();
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D2), new ShootCommand(itemFactory, blueArrowSprite, 200));

            //Bomb
            ISprite bombSprite = itemFactory.CreateBomb();
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D3), new ShootCommand(itemFactory, bombSprite, 0));


            //FireBomb
            ISprite boomarangSprite = itemFactory.CreateBoomarang();
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D4), new ShootCommand(itemFactory, boomarangSprite, 200));

        }

        public void UpdateDirection()
        {
            string direction = link.Direction;

            switch(direction)
            {
                case "left":
                    itemFactory.SetDirection(new Vector2(-1, 0));
                    break;
                case "right":
                    itemFactory.SetDirection(new Vector2(1, 0));
                    break;
                case "up":
                    itemFactory.SetDirection(new Vector2(1, -90));
                    break;
                case "down":
                    itemFactory.SetDirection(new Vector2(1, 90));
                    break;
                default: break;
            }
        }

        public void UpdatePostion()
        {
            Vector2 location = link.Position;

            float x = link.Position.X + 52;
            float y = link.Position.Y + 20;

            itemFactory.SetStartPosition(new Vector2(x, link.Position.Y));
        }
    }
}
