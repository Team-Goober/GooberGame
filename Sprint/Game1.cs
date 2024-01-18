using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Sprint.Controllers;
using Sprint.Interfaces;
using System.Collections;
using Sprint.Sprite;
using System.Diagnostics;

namespace Sprint
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //////////////////////////////////////////
        private Lugi lugi;
        private string animation;
        //////////////////////////////////////////

        private SpriteFont font;

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
            controllerList.Add(new KeyboardC(this));
            controllerList.Add(new MouseC(this));

            animation = "frozen";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //////////////////////////////////////////
            Texture2D texture = Content.Load<Texture2D>("lugi_left2");
            Texture2D textureRight = Content.Load<Texture2D>("lugi_right");
            lugi = new Lugi(texture, textureRight, 1, 4);
            //////////////////////////////////////////

            font = Content.Load<SpriteFont>("Font");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            foreach (IController controller in controllerList)
            {
                controller.UpdateInput(gameTime);
            }


            //////////////////////////////////////////
            lugi.Update(gameTime);
            //////////////////////////////////////////
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            lugi.Draw(_spriteBatch, new Vector2(300, 200), animation);
            _spriteBatch.DrawString(font, "Credit", new Vector2(10, 300), Color.Black);
            _spriteBatch.DrawString(font, "Program Made By: Bill Yang", new Vector2(10, 330), Color.Black);
            _spriteBatch.DrawString(font, "Sprites from: www.mariomayhem.com/downloads/sprites/super_mario_bros_sprites.php", new Vector2(10, 360), Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void SetAnimation(string newAnimation)
        {
            this.animation = newAnimation;
        }
    }
}
