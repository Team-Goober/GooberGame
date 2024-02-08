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
        private Dictionary<string, ISprite> charAnimations = new Dictionary<string, ISprite>();

        private IAtlas currentAnimation;
        private int position;
        private ISprite currAnimation;




        //declares the move systems for the main character sprite
        public MainCharacter(Game1 game)
        {


            //Loads sprite sheet for link
            Texture2D zeldaSheet = game.Content.Load<Texture2D>("zelda_links");
            ISprite zeldaSprite = new AnimatedSprite(zeldaSheet);

            //declares autoatlas on the location of animation down
            //0, 0, 16, 47 is the coordinates of the x, y for down animation in sprite sheet
            //Important: the second part 16, 47 is the width and height of sprite. For instance is the sprite size is 
            //22x47, then you have to state that in the second part of the rectangle
            //the next 2, 1 is the rows and cols of the sprites
            //the next 2 is the padding between sprites
            //true (boolean) is whether animation should loop
            //5 is the speed 
            IAtlas downAtlas = new AutoAtlas(new Rectangle(0,0,22,47), 2, 1, 2, true, 5);
            zeldaSprite.RegisterAnimation("down", downAtlas);

            zeldaSprite.SetAnimation("down");
            zeldaSprite.SetScale(3);

            charAnimations.Add("down", zeldaSprite);



            //creates animation of link moving left
            ISprite leftSprite = new AnimatedSprite(zeldaSheet);

            IAtlas leftAtlas = new AutoAtlas(new Rectangle(23,0,22,48), 2, 1, 2, true, 5);
            leftSprite.RegisterAnimation("left", leftAtlas);
            leftSprite.SetAnimation("left");
            leftSprite.SetScale(3);

            charAnimations.Add("left", leftSprite);

            //creates animation of link moving right
            ISprite rightSprite = new AnimatedSprite(zeldaSheet);

            IAtlas rightAtlas = new AutoAtlas(new Rectangle(88, 0, 22, 47), 2, 1, 2, true,5);
            rightSprite.RegisterAnimation("right", rightAtlas);
            rightSprite.SetAnimation("right");
            rightSprite.SetScale(3);

            charAnimations.Add("right", rightSprite);

            
            //creates animations of link moving up
            ISprite upSprite = new AnimatedSprite(zeldaSheet);

            IAtlas upAtlas = new AutoAtlas(new Rectangle(55, 0,22,47), 2, 1, 2, true, 5);
            upSprite.RegisterAnimation("up", upAtlas);
            upSprite.SetAnimation("up");
            upSprite.SetScale(3);
            charAnimations.Add("up", upSprite);

        }





        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Vector2 spriteLocation)
        {

            //Draws sprite animation using AnimationSprite class
            charAnimations["left"].Draw(spriteBatch, spriteLocation, gameTime);
            
        }

    }
}

