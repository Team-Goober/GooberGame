using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Sprite;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint.Commands;
using Sprint.Input;

namespace Sprint.Projectile
{
    internal class ProjectileSystem
    {
        private IProjectileFactory arrowFactory;
        private IProjectileFactory blueArrowFactory;
        private IProjectileFactory bombFactory;
        private IProjectileFactory boomarangFactory;

        private MoveSystems link;
        private Vector2 oldLocation;
        private Vector2 newLocation;

        public ProjectileSystem(ContentManager content, EntityManager entityManager, IInputMap inputTable, MoveSystems character)
        {
            this.link = character;
            oldLocation = link.spriteLocation;

            Texture2D itemSheet = content.Load<Texture2D>("zelda_items");

            //Arrow
            ISprite arrowSprite = new AnimatedSprite(itemSheet);
            IAtlas arrowAtlas = new SingleAtlas(new Rectangle(0, 45, 16, 5), new Vector2(0, 0));
            arrowSprite.RegisterAnimation("arrow", arrowAtlas);
            arrowSprite.SetAnimation("arrow");
            arrowSprite.SetScale(4);
            arrowFactory = new SimpleProjectileFactory(entityManager, arrowSprite, 200, new Vector2(300, 300));
            arrowFactory.SetDirection(new Vector2(1, 0));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D1), new ShootCommand(arrowFactory));

            //Blue Arrow
            ISprite blueArrowSprite = new AnimatedSprite(itemSheet);
            IAtlas blueArrowAtlas = new SingleAtlas(new Rectangle(0, 125, 16, 5), new Vector2(0, 0));
            blueArrowSprite.RegisterAnimation("blueArrow", blueArrowAtlas);
            blueArrowSprite.SetAnimation("blueArrow");
            blueArrowSprite.SetScale(4);
            blueArrowFactory = new SimpleProjectileFactory(entityManager, blueArrowSprite, 200, new Vector2(0, 0));
            blueArrowFactory.SetDirection(new Vector2(1, 0));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D2), new ShootCommand(blueArrowFactory));

            //Bomb
            ISprite bombSprite = new AnimatedSprite(itemSheet);
            IAtlas bombAtlas = new SingleAtlas(new Rectangle(204, 1, 9, 14), new Vector2(0, 0));
            bombSprite.RegisterAnimation("bomb", bombAtlas);
            bombSprite.SetAnimation("bomb");
            bombSprite.SetScale(4);
            bombFactory = new SimpleProjectileFactory(entityManager, bombSprite, 0, new Vector2(0, 0));
            bombFactory.SetDirection(new Vector2(1, 0));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D3), new ShootCommand(bombFactory));

            //FireBomb
            ISprite boomarangSprite = new AnimatedSprite(itemSheet);
            IAtlas boomarangAtlas = new SingleAtlas(new Rectangle(285, 4, 5, 8), new Vector2(0, 0));
            boomarangSprite.RegisterAnimation("boomarang", boomarangAtlas);
            boomarangSprite.SetAnimation("boomarang");
            boomarangSprite.SetScale(4);
            boomarangFactory = new SimpleProjectileFactory(entityManager, boomarangSprite, 200, new Vector2(0, 0));
            boomarangFactory.SetDirection(new Vector2(1, 0));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D4), new ShootCommand(boomarangFactory));
        }

        public void UpdateDirection()
        {
            newLocation = link.spriteLocation;

            //Left
            arrowFactory.SetDirection(new Vector2(1, 0));

            oldLocation = newLocation;
        }

        public void UpdatePostion()
        {
            Vector2 location = link.spriteLocation;

            //Corrections.
            /*This could be better. But alot of other code file would need to be added to.*/
            float y = location.Y - 35;

            arrowFactory.SetStartPosition(new Vector2(location.X, y));
            blueArrowFactory.SetStartPosition(new Vector2(location.X, y));
            bombFactory.SetStartPosition(new Vector2(location.X, y));
            boomarangFactory.SetStartPosition(new Vector2(location.X, y));
        }
    }
}
