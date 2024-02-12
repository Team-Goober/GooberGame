using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint.Commands;
using Sprint.Input;
using Sprint.Interfaces;
using Sprint.Sprite;
using System.Diagnostics;

namespace Sprint.Projectile
{
    internal class SimpleProjectileFactory : IProjectileFactory
    {
        private Texture2D itemSheet;
        Vector2 position;
        GameObjectManager objectManager;
        Vector2 direction;

        private Vector2 velocity;

        public SimpleProjectileFactory(GameObjectManager objectManager)
        {
            //this.projSprite = projSprite;
            //this.speed = speed;
            //this.position = position;
            this.objectManager = objectManager;
        }

        public void LoadAllTextures(ContentManager content)
        {
            itemSheet = content.Load<Texture2D>("zelda_items");
        }

        public ISprite CreateArrow()
        {
            ISprite arrowSprite = new AnimatedSprite(itemSheet);
            IAtlas arrowAtlas = new SingleAtlas(new Rectangle(0, 45, 16, 5), new Vector2(0, 0));
            arrowSprite.RegisterAnimation("arrow", arrowAtlas);
            arrowSprite.SetAnimation("arrow");
            arrowSprite.SetScale(4);

            return arrowSprite;
        }

        public ISprite CreateBlueArrow()
        {
            ISprite blueArrowSprite = new AnimatedSprite(itemSheet);
            IAtlas blueArrowAtlas = new SingleAtlas(new Rectangle(0, 125, 16, 5), new Vector2(0, 0));
            blueArrowSprite.RegisterAnimation("blueArrow", blueArrowAtlas);
            blueArrowSprite.SetAnimation("blueArrow");
            blueArrowSprite.SetScale(4);

            return blueArrowSprite;
        }

        public ISprite CreateBomb()
        {
            ISprite bombSprite = new AnimatedSprite(itemSheet);
            IAtlas bombAtlas = new SingleAtlas(new Rectangle(204, 1, 9, 14), new Vector2(0, 0));
            bombSprite.RegisterAnimation("bomb", bombAtlas);
            bombSprite.SetAnimation("bomb");
            bombSprite.SetScale(4);

            return bombSprite; 
        }

        public ISprite CreateBoomarang()
        {
            ISprite boomarangSprite = new AnimatedSprite(itemSheet);
            IAtlas boomarangAtlas = new SingleAtlas(new Rectangle(285, 4, 5, 8), new Vector2(0, 0));
            boomarangSprite.RegisterAnimation("boomarang", boomarangAtlas);
            boomarangSprite.SetAnimation("boomarang");
            boomarangSprite.SetScale(4);

            return boomarangSprite;
        }

        public void Create(ISprite projSprite)
        {
            // Start of projectile with correct initial position and velocity
            /*
            Vector2 velocity;
            if (direction.Length() == 0)
            {
                velocity = Vector2.Zero;
            }
            else
            {
                velocity = Vector2.Normalize(direction) * speed;
            }
            */
            Debug.WriteLine(velocity);
            Debug.WriteLine("Created");
            IProjectile proj = new SimpleProjectile(projSprite, position, velocity);

            // Add projectile to game's entity manager
            objectManager.Add(proj);
        }

        public void Stage(float speed)
        {
            // Start of projectile with correct initial position and velocity
            
            if (direction.Length() == 0)
            {
                velocity = Vector2.Zero;
            }
            else
            {
                velocity = Vector2.Normalize(direction) * speed;
            }

        }

        public void SetDirection(Vector2 direction)
        {
            this.direction = direction;
        }

        public void SetStartPosition(Vector2 pos)
        {
            position = pos;
        }
    }
}
