﻿using Sprint.Levels;
using Sprint.Characters;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Sprint.Interfaces
{
    internal interface IPowerup
    {

        public void Apply(Player player, Room room);

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime);

        public void Update(GameTime gameTime);

    }
}
