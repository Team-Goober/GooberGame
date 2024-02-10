using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Sprint.Interfaces;
using Sprint.Sprite;
using System;




namespace Sprint
{
    internal class CycleItem
    {
        private List<ISprite> sprites = new List<ISprite>();
        private int position;

        public CycleItem(Game1 game)
        {
            position = 0;

            Texture2D rupyT = game.Content.Load<Texture2D>("Items/ZeldaSprite5Rupies");
            ISprite rupyS = new AnimatedSprite(rupyT);

            IAtlas rupyA = new SingleAtlas(new Rectangle(0, 0, 8, 16), new Vector2(4, 8));
            rupyS.RegisterAnimation("rupy", rupyA);

            rupyS.SetAnimation("rupy");
            rupyS.SetScale(4);
            sprites.Add(rupyS);

            Texture2D arrowT = game.Content.Load<Texture2D>("Items/ZeldaSpriteArrow");
            ISprite arrowS = new AnimatedSprite(arrowT);

            IAtlas arrowA = new SingleAtlas(new Rectangle(0, 0, 5, 16), new Vector2(2, 8));
            arrowS.RegisterAnimation("arrow", arrowA);

            arrowS.SetAnimation("arrow");
            arrowS.SetScale(4);
            sprites.Add(arrowS);

            Texture2D heartConT = game.Content.Load<Texture2D>("Items/ZeldaSpriteHeartContainer");
            ISprite heartConS = new AnimatedSprite(heartConT);

            IAtlas heartConA = new SingleAtlas(new Rectangle(0, 0, 13, 13), new Vector2(5, 5));
            heartConS.RegisterAnimation("heartCon", heartConA);

            heartConS.SetAnimation("heartCon");
            heartConS.SetScale(4);
            sprites.Add(heartConS);

            Texture2D magicalShieldT = game.Content.Load<Texture2D>("Items/ZeldaSpriteMagicalShield");
            ISprite magicalShieldS = new AnimatedSprite(magicalShieldT);

            IAtlas magicalShieldA = new SingleAtlas(new Rectangle(0, 0, 8, 12), new Vector2(4, 6));
            magicalShieldS.RegisterAnimation("magicalShield", magicalShieldA);

            magicalShieldS.SetAnimation("magicalShield");
            magicalShieldS.SetScale(4);
            sprites.Add(magicalShieldS);

            Texture2D triforceT = game.Content.Load<Texture2D>("Items/Triforce");
            ISprite triforceS = new AnimatedSprite(triforceT);

            IAtlas triforceA = new AutoAtlas(new Rectangle(0, 0, 24, 10), 1, 2, 4, true, 10);
            triforceS.RegisterAnimation("triforce", triforceA);

            triforceS.SetAnimation("triforce");
            triforceS.SetScale(4);
            sprites.Add(triforceS);
        }

        public void Next()
        {
            position++;

            if(position == sprites.Count)
            {
                position = 0;
            }
        }

        public void Back()
        {
            position--;
            if(position == -1)
            {
                position = sprites.Count - 1;
            }
        }

        public void Update(GameTime gameTime)
        {
            sprites[position].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprites[position].Draw(spriteBatch, new Vector2(500, 100), gameTime);
        }
    }
}
