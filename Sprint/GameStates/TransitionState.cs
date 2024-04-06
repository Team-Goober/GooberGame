using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Levels;
using System.Collections.Generic;
using System.Linq;
using static Sprint.Characters.Character;

namespace Sprint
{
    internal class TransitionState : IGameState
    {

        private Goober game;
        private Dictionary<SceneObjectManager, Vector4> scenes; // Keys are all scenes to be drawn. Values are vector of start and end position during transition
        private float timePassed;
        private float totalDuration;
        private IGameState nextState; // State to go to after this one ends

        public TransitionState(Goober game, Dictionary<SceneObjectManager, Vector4> scenes, float duration, IGameState next)
        {
            this.game = game;
            nextState = next;
            timePassed = 0;
            totalDuration = duration;
            this.scenes = scenes;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            float proportion = timePassed / totalDuration;

            // Draw scenes at proper translations
            foreach (KeyValuePair<SceneObjectManager, Vector4> p in scenes)
            {
                // Calculate translation as interpolation between start and end positions
                Vector3 translation = new Vector3(p.Value.Z * proportion + p.Value.X * (1 - proportion),
                    p.Value.W * proportion + p.Value.Y * (1 - proportion), 0);
                DrawScene(p.Key, spriteBatch, gameTime, translation);
            }

        }

        // Draws the list of scenes at the given translation by beginning the sprite batch again
        public void DrawScene(SceneObjectManager scene, SpriteBatch spriteBatch, GameTime gameTime, Vector3 translate)
        {
            Matrix translateMat = Matrix.CreateTranslation(translate);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: translateMat);
            foreach (IGameObject obj in scene.GetObjects())
                obj.Draw(spriteBatch, gameTime);
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            // Increase time
            timePassed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Once end is reached, return game to a different state
            if (timePassed >= totalDuration)
            {
                timePassed = totalDuration;
                PassToState(nextState, false);
            }
        }

        public void PassToState(IGameState newState, bool reset)
        {
            game.GameState = newState;
        }

        public List<SceneObjectManager> AllObjectManagers()
        {
            List<SceneObjectManager> list = scenes.Keys.ToList();
            return list;
        }

        public void MakeCommands()
        {
            // None
        }
    }
}
