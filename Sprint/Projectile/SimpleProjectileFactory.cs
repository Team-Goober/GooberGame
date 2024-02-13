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
    internal class SimpleProjectileFactory
    {
        private Texture2D itemSheet;
        Vector2 position;
        Vector2 direction;

        public SimpleProjectileFactory()
        {
         
        }

        public void LoadAllTextures(ContentManager content)
        {
            itemSheet = content.Load<Texture2D>("zelda_items");
        }

        public Arrow CreateArrow()
        {
            return new Arrow(itemSheet, position, direction);
        }

        // TODO: implement other types of projectile
        /*public BlueArrow CreateBlueArrow()
        {
            ISprite blueArrowSprite = new AnimatedSprite(itemSheet);
            IAtlas blueArrowAtlas = new SingleAtlas(new Rectangle(0, 125, 16, 5), new Vector2(0, 0));
            blueArrowSprite.RegisterAnimation("blueArrow", blueArrowAtlas);
            blueArrowSprite.SetAnimation("blueArrow");
            blueArrowSprite.SetScale(4);

            return blueArrowSprite;
        }

        public Bomb CreateBomb()
        {
            ISprite bombSprite = new AnimatedSprite(itemSheet);
            IAtlas bombAtlas = new SingleAtlas(new Rectangle(204, 1, 9, 14), new Vector2(0, 0));
            bombSprite.RegisterAnimation("bomb", bombAtlas);
            bombSprite.SetAnimation("bomb");
            bombSprite.SetScale(4);

            return bombSprite; 
        }

        public Boomerang CreateBoomarang()
        {
            ISprite boomarangSprite = new AnimatedSprite(itemSheet);
            IAtlas boomarangAtlas = new SingleAtlas(new Rectangle(285, 4, 5, 8), new Vector2(0, 0));
            boomarangSprite.RegisterAnimation("boomarang", boomarangAtlas);
            boomarangSprite.SetAnimation("boomarang");
            boomarangSprite.SetScale(4);

            return boomarangSprite;
        }*/

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
