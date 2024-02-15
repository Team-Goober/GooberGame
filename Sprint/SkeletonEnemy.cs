using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint.Commands.SecondaryItem;
using Sprint.Input;
using System;
using Sprint.Projectile;
using Sprint.Sprite;

namespace Sprint
{

    //Code based on the BluebubbleEnemy.cs file
    public class SkeletonEnemy : Enemy
    {
        private float elapsedTime;
        private Vector2 initialPosition;
        private Vector2 moveDirection; // Movement direction for the random pattern
        private Game1 game;
        private SimpleProjectileFactory itemFactory;
        private ICommand projectileCommand;
        private GameObjectManager objectManager;


        private Timer timeAttack;

        
        public SkeletonEnemy(Game1 game, Texture2D spriteSheet, Vector2 initialPosition, IAtlas enemyAtlas, ContentManager contManager, GameObjectManager objectManager)
            : base(game, new AnimatedSprite(spriteSheet), initialPosition)
        {
            //register default animation using the provided enemyAtlas
            sprite.RegisterAnimation("default", enemyAtlas);

            // Store the initial position for reference
            this.initialPosition = initialPosition;

            this.game = game;

            timeAttack = new Timer(2);
            timeAttack.Start();

            this.itemFactory = new SimpleProjectileFactory();
            
            itemFactory.LoadAllTextures(contManager);


            this.projectileCommand = new ShootArrowCommand(itemFactory, objectManager);




            // Initialize the move direction randomly
            RandomizeMoveDirection();
        }

        // Factory method to create a enemy with default settings
        public static SkeletonEnemy CreateSkeletonEnemy(Game1 game, Vector2 initialPosition, GameObjectManager objectManager)
        {

            
            string textureName = "zelda_enemies"; // Using the same texture as JellyfishEnemy
            int scale = 2;

            // Load SkeletonEnemy texture
            Texture2D skeletonTexture = game.Content.Load<Texture2D>(textureName);

            // Define auto atlases for animations
            IAtlas moveAnimation = new AutoAtlas(new Rectangle(420,120,15,46), 2, 1, 16, new Vector2(7.5f, 8), true, 10);

            // Create SkeletonEnemy instance
            SkeletonEnemy skeletonEnemy = new SkeletonEnemy(game, skeletonTexture, initialPosition, moveAnimation, game.Content, objectManager);

            // Register directional animations
            skeletonEnemy.RegisterDirectionalAnimation("moving", moveAnimation);



            // Set the default animation and scale
            skeletonEnemy.SetAnimation("default");
            skeletonEnemy.SetScale(scale);

            return skeletonEnemy;
        }

        // Register a directional animation for sprite
        public void RegisterDirectionalAnimation(string animationLabel, IAtlas atlas)
        {
            sprite.RegisterAnimation(animationLabel, atlas);
        }

        // Set the current animation for sprite
        public void SetAnimation(string animationLabel)
        {
            sprite.SetAnimation(animationLabel);
        }

        // Set the scale of sprite
        public void SetScale(int scale)
        {
            sprite.SetScale(scale);
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

                projectileCommand.Execute();

                timeAttack.Start();

            }


            // Calculate movement based on elapsed time for the random pattern
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move randomly within a specified area
            MoveRandomly(gameTime);

            // Update the sprite and physics
            sprite.Update(gameTime);
            physics.Update(gameTime);
            
            
        }

        // Set animation based on the direction of movement
        private void SetAnimationBasedOnDirection()
        {

            SetAnimation("moving");

        }

        // Move Skeleton randomly within the game area
        private void MoveRandomly(GameTime gameTime)
        {
            float speed = 50; // Adjust the speed as needed
            float moveTime = 2; // Time before changing direction (in seconds)

            if (elapsedTime > moveTime)
            {
                // Change direction after the specified time
                RandomizeMoveDirection();
                elapsedTime = 0;
            }

            // Set animation based on the new direction
            SetAnimationBasedOnDirection();

            // Move in the current direction
            Vector2 newPosition = physics.Position + moveDirection * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            CheckBounds(newPosition, 3);

            physics.SetPosition(newPosition);
        }

        //ensures that the enemy always stays within windows of the game
        private void CheckBounds(Vector2 pos, float scale)
        {
            int gameX = 600;
            int gameY = 400;

            //makes the enemy go to the other direction when it reaches a certain distance so that it doesnt go over window
            if(pos.X + scale > gameX)
            {
                moveDirection.X = -moveDirection.X;
                
            }

            if(pos.Y + scale > gameY)
            {
                moveDirection.Y = -moveDirection.Y;
            }
        }

        // Generate a random movement direction
        private void RandomizeMoveDirection()
        {
            // Generate a random movement direction
            Random random = new Random();
            float angle = (float)random.NextDouble() * MathHelper.TwoPi;
            moveDirection = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            
        }
    }
}
