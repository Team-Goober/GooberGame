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

        private ISprite sprite;
        



        //declares the move systems for the main character sprite
        public MainCharacter(Game1 game)
        {


            //Loads sprite sheet for link
            Texture2D zeldaSheet = game.Content.Load<Texture2D>("zelda_links");
            sprite = new AnimatedSprite(zeldaSheet);

            //declares autoatlas on the location of animation down
            //0, 0, 16, 47 is the coordinates of the x, y for down animation in sprite sheet
            //Important: the second part 16, 47 is the width and height of sprite. For instance is the sprite size is 
            //22x47, then you have to state that in the second part of the rectangle
            //the next 2, 1 is the rows and cols of the sprites
            //the next 2 is the padding between sprites
            //true (boolean) is whether animation should loop
            //5 is the speed 
            IAtlas downAtlas = new AutoAtlas(new Rectangle(0,0,22,47), 2, 1, 2, true, 5);
            sprite.RegisterAnimation("down", downAtlas);


            IAtlas leftAtlas = new AutoAtlas(new Rectangle(23,0,22,48), 2, 1, 2, true, 5);
            sprite.RegisterAnimation("left", leftAtlas);


            IAtlas rightAtlas = new AutoAtlas(new Rectangle(88, 0, 22, 47), 2, 1, 2, true,5);
            sprite.RegisterAnimation("right", rightAtlas);

            IAtlas upAtlas = new AutoAtlas(new Rectangle(55, 0,22,47), 2, 1, 2, true, 5);
            sprite.RegisterAnimation("up", upAtlas);

            IAtlas stillAtlas = new SingleAtlas(new Rectangle(0, 0, 22, 22), new Vector2(20, 20));  
            sprite.RegisterAnimation("still", stillAtlas);


            sprite.SetAnimation("still");
            sprite.SetScale(3);

        }



        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Vector2 spriteLocation, string direction)
        {
            sprite.SetAnimation(direction);
            //Draws sprite animation using AnimationSprite class
            sprite.Draw(spriteBatch, spriteLocation, gameTime);
            
        }

    }
}

