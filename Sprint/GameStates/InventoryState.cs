using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint.Functions;
using Sprint.HUD;
using Sprint.Input;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Sprite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Sprint.GameStates
{
    internal class InventoryState : IGameState
    {

        private Goober game;
        private IInputMap input;
        private SceneObjectManager hud;
        private SceneObjectManager inventoryUI;

        private Vector2 hudPosition;

        public InventoryState(Goober game)
        {
            this.game = game;
        }

        public List<SceneObjectManager> AllObjectManagers()
        {
            return new List<SceneObjectManager>();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Matrix translateMat = Matrix.CreateTranslation(new Vector3(hudPosition.X, hudPosition.Y, 0));
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: translateMat);

            // Draw HUD
            foreach (IGameObject obj in hud.GetObjects())
                obj.Draw(spriteBatch, gameTime);

            spriteBatch.End();

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // Draw inventory
            foreach (IGameObject obj in inventoryUI.GetObjects())
                obj.Draw(spriteBatch, gameTime);

            spriteBatch.End();
        }
        public void Update(GameTime gameTime)
        {
            // Update commands
            input.Update(gameTime);

            // Update HUD
            foreach (IGameObject obj in hud.GetObjects())
                obj.Update(gameTime);
            hud.EndCycle();

            // Update inventory
            foreach (IGameObject obj in inventoryUI.GetObjects())
                obj.Update(gameTime);
            inventoryUI.EndCycle();
        }

        public void MakeCommands()
        {
            input = new InputTable();
            // Register command to return to game
            input.RegisterMapping(new SingleKeyPressTrigger(Keys.I), new CloseInventoryCommand(this));
        }

        public void PassToState(IGameState newState)
        {
            game.GameState = newState;
            input.Sleep();
        }



        public void SetHUD(HUDLoader hudLoader, Vector2 pos)
        {
            hudPosition = pos;
            hud = hudLoader.GetTopDisplay();
            inventoryUI = hudLoader.GetInventoryScreen();
        }

        public SceneObjectManager GetScene()
        {
            return inventoryUI;
        }

        public void CloseInventory()
        {
            DungeonState dungeon = (DungeonState)game.GetDungeonState();

            // Set all the start and end positions for the scenes
            Dictionary<SceneObjectManager, Vector4> scrollScenes = new()
            {
                { dungeon.GetRoomAt(dungeon.RoomIndex()), new Vector4(hudPosition.X, Goober.gameHeight, hudPosition.X, Goober.gameHeight - hudPosition.Y) },
                { inventoryUI, new Vector4(hudPosition.X, 0, -hudPosition.X, -hudPosition.Y) },
                { hud, new Vector4(hudPosition.X, hudPosition.Y, hudPosition.X, 0) }
            };

            // Create new GameState to scroll and then set back to this state
            TransitionState scroll = new TransitionState(game, scrollScenes, 0.75f, dungeon);

            PassToState(scroll);
        }

    }
}
