using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sprint.Characters.Character;

namespace Sprint
{
    internal class TransitionState : IGameState
    {

        private Goober game;
        private List<SceneObjectManager> awayScenes; // Scenes that should be scrolled away from
        private List<SceneObjectManager> towardScenes; // Scenes that should be scrolled into
        private List<SceneObjectManager> fixedScenes; // Scenes that shouldn't move in the scroll
        private Directions direction; // Direction to move camera towards. Scenes move in opposite direction
        private Vector2 velocity;
        private Vector2 offset;
        private Vector2 max;
        private IGameState nextState;

        public TransitionState(Goober game, List<SceneObjectManager> away, List<SceneObjectManager> toward, List<SceneObjectManager> fix, 
            Directions direction, float duration, IGameState next)
        {
            this.game = game;
            this.nextState = next;
            this.awayScenes = away;
            this.towardScenes = toward;
            this.fixedScenes = fix;
            this.direction = direction;

            offset = Vector2.Zero;

            // Determine velocities based on scroll direction, scroll duration, and window dimensions
            switch (this.direction)
            {
                case Directions.UP:
                    max = new Vector2(0, Goober.gameHeight);
                    break;
                case Directions.DOWN:
                    max = new Vector2(0, - Goober.gameHeight);
                    break;
                case Directions.LEFT:
                    max = new Vector2(Goober.gameWidth, 0);
                    break;
                case Directions.RIGHT:
                    max = new Vector2(- Goober.gameWidth, 0);
                    break;
            }

            velocity = max / duration;

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            DrawScenes(awayScenes, spriteBatch, gameTime, offset);
            DrawScenes(towardScenes, spriteBatch, gameTime, -max + offset);
            DrawScenes(fixedScenes, spriteBatch, gameTime, Vector2.Zero);

        }

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
            offset = offset + velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (offset.Length() >= max.Length())
            {
                offset = max;
                game.GameState = nextState;
            }
        }
    }
}
