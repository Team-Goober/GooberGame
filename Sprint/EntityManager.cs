using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using System.Collections.Generic;

namespace Sprint
{
    internal class EntityManager
    {

        private List<IEntity> entities;

        public EntityManager()
        {
            entities = new List<IEntity>();
        }

        public void AddEntity(IEntity entity)
        {
            // Add new entity to manage
            entities.Add(entity);
        }

        public void Update(GameTime gameTime)
        {
            // Update every entity
            for(int i = entities.Count - 1; i >= 0; i--)
            {
                entities[i].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Draw every entity
            for (int i = entities.Count - 1; i >= 0; i--)
            {
                entities[i].Draw(spriteBatch, gameTime);
            }
        }

    }
}
