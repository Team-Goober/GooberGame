using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint.Characters;
using Sprint.Functions;
using Sprint.HUD;
using Sprint.Input;
using Sprint.Interfaces;
using Sprint.Items;
using Sprint.Levels;
using System.Collections.Generic;

namespace Sprint.GameStates
{
    internal class InventoryState : IGameState
    {

        private Goober game;
        private IInputMap input;
        private SceneObjectManager hud;
        private SceneObjectManager inventoryUI;
        private Inventory playerInventory;
        private Point slot;

        private Vector2 hudPosition;

        public delegate void SelectorMoveDelegate(int row, int column);
        public event SelectorMoveDelegate SelectorMoveEvent;

        public InventoryState(Goober game)
        {
            this.game = game;
            slot = new Point(0, 0);
        }

        // Assign an inventory to modify
        public void AttachPlayer(Player player)
        {
            playerInventory = player.GetInventory();
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
            // Register commands to move selector
            input.RegisterMapping(new SingleKeyPressTrigger(Keys.W), new MoveSelectorCommand(this, new Point(0, -1)));
            input.RegisterMapping(new SingleKeyPressTrigger(Keys.A), new MoveSelectorCommand(this, new Point(-1, 0)));
            input.RegisterMapping(new SingleKeyPressTrigger(Keys.S), new MoveSelectorCommand(this, new Point(0, 1)));
            input.RegisterMapping(new SingleKeyPressTrigger(Keys.D), new MoveSelectorCommand(this, new Point(1, 0)));
            input.RegisterMapping(new SingleKeyPressTrigger(Keys.Up), new MoveSelectorCommand(this, new Point(0, -1)));
            input.RegisterMapping(new SingleKeyPressTrigger(Keys.Left), new MoveSelectorCommand(this, new Point(-1, 0)));
            input.RegisterMapping(new SingleKeyPressTrigger(Keys.Down), new MoveSelectorCommand(this, new Point(0, 1)));
            input.RegisterMapping(new SingleKeyPressTrigger(Keys.Right), new MoveSelectorCommand(this, new Point(1, 0)));
            input.RegisterMapping(new SingleKeyPressTrigger(Keys.Z), new SelectSlotCommand(this));
        }

        public void Reset()
        {
            input.ClearDictionary();

            slot = new Point(0, 0);

            playerInventory = null;

            hudPosition = Vector2.Zero;
            hud = null;
            inventoryUI = null;

            MakeCommands();

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
            // Move selector back to start location
            TryMoveSelector(Point.Zero);

            DungeonState dungeon = (DungeonState)game.GetDungeonState();

            // Set all the start and end positions for the scenes
            Dictionary<SceneObjectManager, Vector4> scrollScenes = new()
            {
                { dungeon.GetRoomAt(dungeon.RoomIndex()).GetScene(), new Vector4(hudPosition.X, Goober.gameHeight, hudPosition.X, Goober.gameHeight - hudPosition.Y) },
                { inventoryUI, new Vector4(hudPosition.X, 0, -hudPosition.X, -hudPosition.Y) },
                { hud, new Vector4(hudPosition.X, hudPosition.Y, hudPosition.X, 0) }
            };

            // Create new GameState to scroll and then set back to this state
            TransitionState scroll = new TransitionState(game, scrollScenes, 0.75f, dungeon);

            PassToState(scroll);
        }
        

        // Try to move the selector in the direction given by p
        public void TryMoveSelector(Point p)
        {
            ItemType[,] slotsArr = Inventory.Slots;
            Point newSlot = slot + p;
            // Check if new slot is in bounds
            if (newSlot.X >= 0 && newSlot.X < slotsArr.GetLength(1) && newSlot.Y >= 0 && newSlot.Y < slotsArr.GetLength(0))
            {
                slot = newSlot;
                SelectorMoveEvent?.Invoke(slot.Y, slot.X);
            }
        }

        // Choose on the current slot
        public void SelectSlot()
        {
            playerInventory.Select(slot.Y, slot.X);
        }

    }
}
