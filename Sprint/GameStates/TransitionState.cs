using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Levels;
using System.Collections.Generic;
using static Sprint.Characters.Character;

namespace Sprint
{
    internal class TransitionState : IGameState
    {

        private Goober game;
        private List<SceneObjectManager> awayScenes; // Scenes that should be scrolled away from
        private List<SceneObjectManager> towardScenes; // Scenes that should be scrolled into
        private List<SceneObjectManager> fixedScenes; // Scenes that shouldn't move in the scroll
        private Vector2 velocity; // Amount to move the scenes per second
        private Vector2 offset; // Current distance of scenes from starting positions
        private Vector2 max; // Total distance that scenes should be moved
        private IGameState nextState; // State to go to after this one ends

        public TransitionState(Goober game, List<SceneObjectManager> away, List<SceneObjectManager> toward, List<SceneObjectManager> fix, 
            Vector2 direction, float duration, IGameState next)
        {
            this.game = game;
            this.nextState = next;
            this.awayScenes = away;
            this.towardScenes = toward;
            this.fixedScenes = fix;

            offset = Vector2.Zero;

            // Determine velocities based on scroll direction, scroll duration, and window dimensions
            max = - direction * new Vector2(Goober.gameWidth, Goober.gameHeight);

            velocity = max / duration;

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Draw scenes at proper translations
            DrawScenes(awayScenes, spriteBatch, gameTime, offset); // Scenes that scroll away
            DrawScenes(towardScenes, spriteBatch, gameTime, -max + offset); // Scenes that scroll in
            DrawScenes(fixedScenes, spriteBatch, gameTime, Vector2.Zero); // Scenes that stay put

        }

        // Draws the list of scenes at the given translation by beginning the sprite batch again
        public void DrawScenes(List<SceneObjectManager> scenes, SpriteBatch spriteBatch, GameTime gameTime, Vector2 translate)
        {
            Matrix translateMat = Matrix.CreateTranslation(new Vector3(translate.X, translate.Y, 0));
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: translateMat);
            foreach (SceneObjectManager s in scenes)
            {
                foreach (IGameObject obj in s.GetObjects())
                    obj.Draw(spriteBatch, gameTime);
            }
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            // Increase offset by needed amount
            offset = offset + velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Once end is reached, return game to a different state
            if (offset.Length() >= max.Length())
            {
                offset = max;
                PassToState(nextState);
            }
        }

        public void PassToState(IGameState newState)
        {
            game.GameState = newState;
        }
    }
}
