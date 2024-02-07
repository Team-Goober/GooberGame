using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Sprint.Interfaces;
using Sprint.Sprite;
using System;


namespace Sprint
{

    public class MainCharacter
    {

        private Texture2D texture;
        private float scale = 1.0f;

        //Dictionary to store the sprite animations
        //!!FIXME Im not sure if it will be better to use a list or a dictionary
        //private Dictionary<string, ISprite> keyAnimations = new Dictionary<string, ISprite>();

        //FIXME!! Might have to change this to dictionary in the future
        private List<ISprite> sprites = new List<ISprite>();
        private IAtlas currentAnimation;
        private int position;
        private ISprite currAnimation;
        



        //declares the move systems for the main character sprite
        public MainCharacter(Game1 game)
        {


            //loads the sprite animation from content
            Texture2D leftZelda = game.Content.Load<Texture2D>("Sprite/ZeldaSpriteLinkLeft");
            ISprite spriteZelda = new AnimatedSprite(leftZelda);

            //Implements the animation of sprite walking left
            //FIXME!! I will have to change this to a walking animation using AutoAtlas
            IAtlas leftZeldaAni = new SingleAtlas(new Rectangle(0, 0, 8, 16), new Vector2(8, 8));
            spriteZelda.RegisterAnimation("leftZelda", leftZeldaAni);

            spriteZelda.SetAnimation("leftZelda");
            spriteZelda.SetScale(3);

            //Adds the sprite animation to the dictionary
            //keyAnimations.Add("leftWalk", spriteZelda);

            sprites.Add(spriteZelda);



        }





        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Vector2 spriteLocation)
        {

            //Draws sprite animation using AnimationSprite class
            sprites[position].Draw(spriteBatch, spriteLocation, gameTime);
        }

    }
}

