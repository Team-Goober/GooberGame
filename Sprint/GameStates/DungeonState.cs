using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint.Characters;
using Sprint.Collision;
using Sprint.Commands;
using Sprint.Functions;
using Sprint.Functions.RoomTransition;
using Sprint.Input;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Loader;
using Sprint.Sprite;
using System.Collections.Generic;

namespace Sprint
{
    internal class DungeonState : IGameState
    {

        private Goober game;
        private ContentManager contentManager;
        private SpriteLoader spriteLoader;
        
        private IInputMap inputTable;
        private CollisionDetector collisionDetector;

        private bool resetGame = false; // Set to true to queue a reset on next update

        private List<SceneObjectManager> rooms; // Object managers for each room. Accessed by index
        private int currentRoom; // Index of currently updated room
        private SceneObjectManager hud; // Object manager for HUD that should persist between rooms
        private Player player; // Player game object to be moved as rooms switch

        HUDLoader hudLoader;

        public DungeonState(Goober game, SpriteLoader spriteLoader, ContentManager contentManager)
        {
            this.game = game;
            this.contentManager = contentManager;
            this.spriteLoader = spriteLoader;

            inputTable = new InputTable();
            collisionDetector = new CollisionDetector();

            player = new Player(inputTable, spriteLoader, new Reset(this));

            rooms = new List<SceneObjectManager>();

            // Load all rooms in the level from XML file
            LevelLoader loader = new LevelLoader(contentManager, this, spriteLoader, inputTable);
            loader.LoadLevelXML("LevelOne/Level1");
            makeCommands();

            //Load the hud
            hudLoader = new HUDLoader(contentManager, spriteLoader);
            hudLoader.LoadHUD("HUD/HUDData", loader.GetLevel());
            hud = hudLoader.GetScenes();

            loadDelegates();
        }

        private void loadDelegates ()
        {
            Inventory.keyHandler += hudLoader.UpdateKeyAmount;
            Inventory.gemHandler += hudLoader.UpdateGemAmount;
            Inventory.bombHandler += hudLoader.UpdateBombAmount;
        }

        // Generates all commands available while the player is moving in a room
        private void makeCommands()
        {
            //Uses the ICommand interface (MoveItems.cs) to execute command for the movement of the main character sprite
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.A), new MoveLeft(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D), new MoveRight(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.W), new MoveUp(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.S), new MoveDown(player));

            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Left), new MoveLeft(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Right), new MoveRight(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Up), new MoveUp(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Down), new MoveDown(player));

            Keys[] moveKeys = { Keys.A, Keys.D, Keys.W, Keys.S, Keys.Left, Keys.Right, Keys.Up, Keys.Down };
            inputTable.RegisterMapping(new MultipleKeyReleaseTrigger(moveKeys), new StopMoving(player));

            //Melee Regular Sword Attack
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Z), new Melee(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.N), new Melee(player));

            //Player uses a cast move
            // TODO: shouldnt bind separately from shoot commands
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D1), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D2), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D3), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D4), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D5), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D6), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D7), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D8), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D9), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D0), new Cast(player));

            // Reset command
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.R), new Reset(this));

            // Switching rooms
            inputTable.RegisterMapping(new SingleClickTrigger(SingleClickTrigger.MouseButton.Right), new NextRoomCommand(this));
            inputTable.RegisterMapping(new SingleClickTrigger(SingleClickTrigger.MouseButton.Left), new PrevRoomCommand(this));

            // Switching to new pause state
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Escape), new PauseCommand(game, this));

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // Draw current room's objects
            SceneObjectManager currRoom = rooms[currentRoom];
            foreach (IGameObject obj in currRoom.GetObjects())
                obj.Draw(spriteBatch, gameTime);

            // Draw HUD
            foreach (IGameObject obj in hud.GetObjects())
                obj.Draw(spriteBatch, gameTime);

            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {

            // Resets the game when requested
            if (resetGame)
            {
                ResetGame();
                resetGame = false;
                // End cycle to complete additions and deletions
                rooms[currentRoom].EndCycle();
            }

            SceneObjectManager currRoom = rooms[currentRoom];

            // Detect inputs and execute commands
            inputTable.Update(gameTime);

            // Update room objects
            foreach (IGameObject obj in currRoom.GetObjects())
                obj.Update(gameTime);

            // Update HUD
            foreach (IGameObject obj in hud.GetObjects())
                obj.Update(gameTime);

            hudLoader.Update();
            hud = hudLoader.GetScenes();

            // Complete additions and deletions
            currRoom.EndCycle();
            hud.EndCycle();

            // Test for collisions in room
            collisionDetector.Update(gameTime, currRoom.GetMovers(), currRoom.GetStatics());

            // Complete additions and deletions resulting from collisions
            currRoom.EndCycle();
        }

        public void PassToState(IGameState newState)
        {
            game.GameState = newState;
            inputTable.Sleep();
        }

        //checks if the user requested a reset for game
        public void ResetReq()
        {
            resetGame = true;
        }

        //clears input dictionary and object manager
        public void ResetGame()
        {
            // delete all game objects
            hud.ClearObjects();

            hud.EndCycle();

            ClearRooms();

            inputTable.ClearDictionary();

            // new player
            player = new Player(inputTable, spriteLoader, new Reset(this));

            // reload the level
            LevelLoader loader = new LevelLoader(contentManager, this, spriteLoader, inputTable);
            loader.LoadLevelXML("LevelOne/Level1");

            //reload the hud
            hudLoader = new HUDLoader(contentManager, spriteLoader);
            hudLoader.LoadHUD("HUD/HUDData", loader.GetLevel());
            hud = hudLoader.GetScenes();

            loadDelegates();

            // remake commands
            makeCommands();
        }

        public void AddRoom(SceneObjectManager room)
        {
            rooms.Add(room);
        }

        // Switches current room to a different one by index
        public void SwitchRoom(Vector2 spawn, int idx)
        {
            // Find direction of scroll based on spawn position
            // TODO: Replace this once doors are refactored
            Character.Directions dir;
            if (spawn.Y > 2 * Goober.gameHeight / 3)
            {
                dir = Character.Directions.UP;
            }
            else if (spawn.Y < Goober.gameHeight / 3)
            {
                dir = Character.Directions.DOWN;
            }
            else if (spawn.X > 2 * Goober.gameWidth / 3)
            {
                dir = Character.Directions.LEFT;
            }
            else if (spawn.X < Goober.gameWidth / 3)           
            {
                dir = Character.Directions.RIGHT;
            }
            else
            {
                dir = Character.Directions.STILL;
            }

            // Create new GameState to scroll and then set back to this state
            TransitionState scroll = new TransitionState(game, new List<SceneObjectManager> { rooms[currentRoom] }, 
                new List<SceneObjectManager> { rooms[idx] }, new List<SceneObjectManager> { hud },
                dir, 0.75f, this);

            PassToState(scroll);


            // Clean up previous room changes
            rooms[idx].EndCycle();
            // Move player to new room
            player.SetScene(rooms[idx]);
            currentRoom = idx;
            player.MoveTo(spawn);

        }

        public int RoomIndex()
        {
            return currentRoom;
        }

        public int NumRooms()
        {
            return rooms.Count;
        }

        public void ClearRooms()
        {
            rooms.Clear();
        }
    }
}
