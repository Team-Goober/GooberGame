using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using System.Collections.Generic;

namespace Sprint
{
     public class GameObjectManager
    {

        private List<IGameObject> objects;

        public GameObjectManager()
        {
            objects = new List<IGameObject>();
        }

        public void Add(IGameObject gameObject)
        {
            // Add new game object to manage
            objects.Add(gameObject);
        }

        public void Update(GameTime gameTime)
        {
            // Update every entity
            for(int i = objects.Count - 1; i >= 0; i--)
            {
                objects[i].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Draw every entity
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                objects[i].Draw(spriteBatch, gameTime);
            }
        }

    }
}
