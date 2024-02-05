using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Sprint.Input;
using Sprint.Interfaces;
using Sprint.Sprite;

namespace Sprint.Testing
{
    public class GameTest : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ISprite sprite;
        private IInputMap inputTable;

        private SpriteFont font;

        public GameTest()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // create input mapping object
            inputTable = new InputTable();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // create sprite with spritesheet
            Texture2D texture = Content.Load<Texture2D>("smb_luigi_sheet");
            sprite = new AnimatedSprite(texture);

            // still frame animation
            IAtlas stillAnim = new SingleAtlas(new Rectangle(0, 16, 15, 14), new Vector2(7, 4));
            sprite.RegisterAnimation("fall", stillAnim);

            // animation based on calculated grid atlas on spritesheet
            IAtlas gridAnim = new AutoAtlas(new Rectangle(239, 0, 77, 16), 1, 3, 13, true, 10);
            sprite.RegisterAnimation("walk", gridAnim);

            // animation set by individually defining frames
            Rectangle[] rects = { new Rectangle(330, 0, 16, 16), new Rectangle(270, 30, 16, 16), new Rectangle(359, 0, 17, 17), new Rectangle(270, 30, 16, 16), };
            Vector2[] centers = { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0)};
            float[] durs = { 2.0f, 1.0f, 2.0f, 1.0f };
            IAtlas definedAnim = new ManualAtlas(rects, centers, durs, true, 10);
            sprite.RegisterAnimation("punch", definedAnim);

            sprite.SetAnimation("fall");


            font = Content.Load<SpriteFont>("Font");
        }

        protected override void Update(GameTime gameTime)
        {
            // must update input map to test for inputs
            inputTable.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            // draw sprite
            sprite.Draw(_spriteBatch, new Vector2(300, 200), gameTime);
            _spriteBatch.DrawString(font, "Credit", new Vector2(10, 300), Color.Black);
            _spriteBatch.DrawString(font, "Program Made By: Team Goobers", new Vector2(10, 330), Color.Black);
            _spriteBatch.DrawString(font, "Sprites from: www.mariomayhem.com/downloads/sprites/super_mario_bros_sprites.php", new Vector2(10, 360), Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
