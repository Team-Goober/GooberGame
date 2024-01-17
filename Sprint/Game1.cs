using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Sprint.Controllers;
using Sprint.Interfaces;
using System.Collections;
using Sprint.Sprite;

namespace Sprint
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //////////////////////////////////////////
        private Lugi lugi;
        //////////////////////////////////////////
        private ArrayList controllerList;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            controllerList = new ArrayList(); 
            
            Texture2D texture = Content.Load<Texture2D>("lugi_left");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Lugi lugiInput = new Lugi(texture, _spriteBatch, 1, 4);

            controllerList.Add(new KeyboardC(this, lugiInput));
            controllerList.Add(new MouseC(this));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //////////////////////////////////////////
            Texture2D texture = Content.Load<Texture2D>("lugi_left");
            lugi = new Lugi(texture, _spriteBatch, 1, 4);
            //////////////////////////////////////////
            
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            foreach (IController controller in controllerList)
            {
                controller.UpdateInput();
            }


            //////////////////////////////////////////
            lugi.Update();
            //////////////////////////////////////////
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            lugi.DrawFrozen(new Vector2(300, 200));

            base.Draw(gameTime);
        }
    }
}
