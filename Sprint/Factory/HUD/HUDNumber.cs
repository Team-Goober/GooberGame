﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Characters;
using Sprint.Interfaces;
using System;

namespace Sprint.Loader
{
    internal class HUDNumber : IHUD
    {
        private ISprite sprite;
        private Vector2 position;

        public HUDNumber(ISprite sprite, Vector2 position)
        {
            this.sprite = sprite;
            this.position = position;
        }

        public void SetNumber(string number)
        {
            sprite.SetAnimation(number);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            // Nothing Here
        }
    }
}