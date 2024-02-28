using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Sprint.Interfaces;
using Sprint.Sprite;

namespace Sprint
{
    internal class CycleItem
    {
        private List<IGameObject> items = new List<IGameObject>();
        private int currentItem;
        private Vector2 position;

        private const string ANIM_FILE = "itemAnims";

        public CycleItem(Goober game, Vector2 position, SpriteLoader spriteLoader)
        {
            this.position = position;

            ISprite rupyS = spriteLoader.BuildSprite(ANIM_FILE, "rupy");
            Item rupy = new Item(game, rupyS, this.position);
            items.Add(rupy);

            ISprite arrowS = spriteLoader.BuildSprite(ANIM_FILE, "arrow");
            Item arrow = new Item(game, arrowS, this.position);
            items.Add(arrow);

            ISprite heartS = spriteLoader.BuildSprite(ANIM_FILE, "heart");
            Item heart = new Item(game, heartS, this.position);
            items.Add(heart);

            ISprite shieldS = spriteLoader.BuildSprite(ANIM_FILE, "shield");
            Item shield = new Item(game, shieldS, this.position);
            items.Add(shield);

            ISprite triforceS = spriteLoader.BuildSprite(ANIM_FILE, "triforce");
            Item triforce = new Item(game, triforceS, this.position);
            items.Add(triforce);
        }

        public void Next()
        {
            currentItem++;

            if(currentItem == items.Count)
            {
                currentItem = 0;
            }
        }

        public void Back()
        {
            currentItem--;
            if(currentItem == -1)
            {
                currentItem = items.Count - 1;
            }
        }

        public void Update(GameTime gameTime)
        {
            items[currentItem].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            items[currentItem].Draw(spriteBatch, gameTime);
        }
    }
}
