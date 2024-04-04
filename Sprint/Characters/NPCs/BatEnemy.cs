using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint.Commands.SecondaryItem;
using Sprint.Input;
using System;
using Sprint.Projectile;
using Sprint.Sprite;
using Sprint.Levels;

namespace Sprint.Characters
{

    //Code based on the BluebubbleEnemy.cs file
    public class BatEnemy : Enemy
    {
        private float elapsedTime;
        private Vector2 initialPosition;
        private Vector2 moveDirection; // Movement direction for the random pattern
        private SimpleProjectileFactory itemFactory;
        private ICommand projectileCommand;
        private MoveRandom moveRandom;

        private Timer timeAttack;


        public BatEnemy(ISprite sprite, Vector2 initialPosition, SceneObjectManager objectManager, SpriteLoader spriteLoader)
            : base(sprite, initialPosition, objectManager)
        {

            // Store the initial position for reference
            this.initialPosition = initialPosition;

            timeAttack = new Timer(2);
            timeAttack.Start();

            itemFactory = new SimpleProjectileFactory(spriteLoader, 30, true, objectManager);


            moveRandom = new MoveRandom(physics);


 
        }

        // Update logic
        public override void Update(GameTime gameTime)
        {

            timeAttack.Update(gameTime);

            //uses timer to shoot arrows ever 3 seconds
            if (timeAttack.JustEnded)
            {
                itemFactory.SetStartPosition(physics.Position);

                itemFactory.SetDirection(moveDirection);

               

                timeAttack.Start();

            }


            // Calculate movement based on elapsed time for the random pattern
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move randomly within a specified area
            moveRandom.MoveAI(gameTime);

            // Update the sprite and physics
            sprite.Update(gameTime);
            physics.Update(gameTime);


        }

    }
}
