using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Sprint.Interfaces;
using Sprint.Sprite;

namespace Sprint
{
    internal class CycleItem
    {
        private List<Item> items = new List<Item>();
        private int currentItem;
        private Vector2 position;
        private GameObjectManager objManager;

        public CycleItem(Goober game, Vector2 position, GameObjectManager objManager)
        {
            this.position = position;
            this.objManager = objManager;

            Texture2D rupyT = game.Content.Load<Texture2D>("Items/ZeldaSprite5Rupies");
            ISprite rupyS = new AnimatedSprite(rupyT);

            IAtlas rupyA = new SingleAtlas(new Rectangle(0, 0, 8, 16), new Vector2(4, 8));
            rupyS.RegisterAnimation("rupy", rupyA);

            rupyS.SetAnimation("rupy");
            rupyS.SetScale(4);

            Item rupy = new Item(game, rupyS, this.position);
            items.Add(rupy);

            Texture2D arrowT = game.Content.Load<Texture2D>("Items/ZeldaSpriteArrow");
            ISprite arrowS = new AnimatedSprite(arrowT);

            IAtlas arrowA = new SingleAtlas(new Rectangle(0, 0, 5, 16), new Vector2(2, 8));
            arrowS.RegisterAnimation("arrow", arrowA);

            arrowS.SetAnimation("arrow");
            arrowS.SetScale(4);

            Item arrow = new Item(game, arrowS, this.position);
            items.Add(arrow);

            Texture2D heartConT = game.Content.Load<Texture2D>("Items/ZeldaSpriteHeartContainer");
            ISprite heartConS = new AnimatedSprite(heartConT);

            IAtlas heartConA = new SingleAtlas(new Rectangle(0, 0, 13, 13), new Vector2(5, 5));
            heartConS.RegisterAnimation("heartCon", heartConA);

            heartConS.SetAnimation("heartCon");
            heartConS.SetScale(4);

            Item heart = new Item(game, heartConS, this.position);
            items.Add(heart);

            Texture2D magicalShieldT = game.Content.Load<Texture2D>("Items/ZeldaSpriteMagicalShield");
            ISprite magicalShieldS = new AnimatedSprite(magicalShieldT);

            IAtlas magicalShieldA = new SingleAtlas(new Rectangle(0, 0, 8, 12), new Vector2(4, 6));
            magicalShieldS.RegisterAnimation("magicalShield", magicalShieldA);

            magicalShieldS.SetAnimation("magicalShield");
            magicalShieldS.SetScale(4);

            Item shield = new Item(game, magicalShieldS, this.position);
            items.Add(shield);

            Texture2D triforceT = game.Content.Load<Texture2D>("Items/Triforce");
            ISprite triforceS = new AnimatedSprite(triforceT);

            IAtlas triforceA = new AutoAtlas(new Rectangle(0, 0, 24, 10), 1, 2, 4, new Vector2(5, 5), true, 10);
            triforceS.RegisterAnimation("triforce", triforceA);

            triforceS.SetAnimation("triforce");
            triforceS.SetScale(4);

            Item triforce = new Item(game, triforceS, this.position);
            items.Add(triforce);

            SwitchItem(null, items[0]);
        }

        public void Next()
        {
            int before = currentItem;
            currentItem++;

            if(currentItem == items.Count)
            {
                currentItem = 0;
            }
            SwitchItem(items[before], items[currentItem]);
        }

        public void Back()
        {
            int before = currentItem;
            currentItem--;
            if(currentItem == -1)
            {
                currentItem = items.Count - 1;
            }
            SwitchItem(items[before], items[currentItem]);
        }

        public void SwitchItem(Item oldI, Item newI)
        {
            if (oldI != null)
                objManager.Remove(oldI);
            if (newI != null)
                objManager.Add(newI);
        }

    }
}
