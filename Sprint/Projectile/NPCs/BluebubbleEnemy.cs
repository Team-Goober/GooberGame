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
    public class BluebubbleEnemy : Enemy
    {
        private float elapsedTime;
        private Timer timeAttack;
        private Vector2 moveDirection; // Movement direction for the random pattern
        private Goober game;
        private ICommand projectileCommand;
        private SimpleProjectileFactory itemFactory;
        private GameObjectManager objectManager;
        private Vector2 initialPosition;

        public BluebubbleEnemy(Goober game, Texture2D spriteSheet, Vector2 initialPosition, IAtlas enemyAtlas, GameObjectManager objectManager)
            : base(game, new AnimatedSprite(spriteSheet), initialPosition)
        {
            // Register default animation using the provided enemyAtlas
            sprite.RegisterAnimation("default", enemyAtlas);

            // Store the initial position for reference
            this.initialPosition = initialPosition;

            this.game = game;

            timeAttack = new Timer(2);
            timeAttack.Start();

            this.itemFactory = new SimpleProjectileFactory(30);

            itemFactory.LoadAllTextures(game.Content);

            this.objectManager = objectManager;

            this.projectileCommand = new ShootBombC(itemFactory, objectManager);

            // Initialize the move direction randomly
            RandomizeMoveDirection();
        }

        // Factory method to create a BluebubbleEnemy with default settings
        public static BluebubbleEnemy CreateBluebubbleEnemy(Goober game, Vector2 initialPosition, GameObjectManager objectManager)
        {
            string textureName = "zelda_enemies"; // Using the same texture as JellyfishEnemy
            int scale = 2;

            // Load BluebubbleEnemy texture
            Texture2D bluebubbleTexture = game.Content.Load<Texture2D>(textureName);

            // Define directional atlases for animations
            IAtlas upFacing = new SingleAtlas(new Rectangle(180, 270, 16, 16), new Vector2(8, 8));
            IAtlas leftFacing = new SingleAtlas(new Rectangle(152, 270, 16, 16), new Vector2(8, 8));
            IAtlas downFacing = new SingleAtlas(new Rectangle(120, 270, 16, 16), new Vector2(8, 8));
            IAtlas rightFacing = new SingleAtlas(new Rectangle(210, 270, 16, 16), new Vector2(8, 8));

            // Create BluebubbleEnemy instance
            BluebubbleEnemy bluebubbleEnemy = new BluebubbleEnemy(game, bluebubbleTexture, initialPosition, upFacing, objectManager);

            // Register directional animations
            bluebubbleEnemy.RegisterDirectionalAnimation("upFacing", upFacing);
            bluebubbleEnemy.RegisterDirectionalAnimation("leftFacing", leftFacing);
            bluebubbleEnemy.RegisterDirectionalAnimation("downFacing", downFacing);
            bluebubbleEnemy.RegisterDirectionalAnimation("rightFacing", rightFacing);

            // Set the default animation and scale
            bluebubbleEnemy.SetAnimation("default");
            bluebubbleEnemy.SetScale(scale);

            return bluebubbleEnemy;
        }

        // Register a directional animation for BluebubbleEnemy sprite
        public void RegisterDirectionalAnimation(string animationLabel, IAtlas atlas)
        {
            sprite.RegisterAnimation(animationLabel, atlas);
        }

        // Set the current animation for BluebubbleEnemy sprite
        public void SetAnimation(string animationLabel)
        {
            sprite.SetAnimation(animationLabel);
        }

        // Set the scale of BluebubbleEnemy sprite
        public void SetScale(int scale)
        {
            sprite.SetScale(scale);
        }

        // Update BluebubbleEnemy logic
        public override void Update(GameTime gameTime)
        {
            timeAttack.Update(gameTime);

            // Uses timer to shoot projectiles every 2 seconds
            if (timeAttack.JustEnded)
            {
                itemFactory.SetStartPosition(physics.Position);
                itemFactory.SetDirection(moveDirection);
                projectileCommand.Execute();
                timeAttack.Start();
            }

            // Calculate movement based on elapsed time for the random pattern
            MoveRandomly(gameTime);

            // Update the sprite and physics
            sprite.Update(gameTime);
            physics.Update(gameTime);
        }

        // Set animation based on the direction of movement
        private void SetAnimationBasedOnDirection()
        {
            // Determine the direction and set the appropriate animation label
            if (Math.Abs(moveDirection.X) > Math.Abs(moveDirection.Y))
            {
                // Horizontal movement
                if (moveDirection.X > 0)
                    SetAnimation("rightFacing");
                else
                    SetAnimation("leftFacing");
            }
            else
            {
                // Vertical movement
                if (moveDirection.Y > 0)
                    SetAnimation("downFacing");
                else
                    SetAnimation("upFacing");
            }
        }

        // Move BluebubbleEnemy randomly within the game area
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
            CheckBounds(newPosition, 3); // Ensure enemy stays within game bounds
            physics.SetPosition(newPosition);
        }

        // Ensure that the enemy always stays within the game bounds
        private void CheckBounds(Vector2 pos, float scale)
        {
            int gameX = 600;
            int gameY = 400;

            // Make the enemy go to the other direction when it reaches a certain distance so that it doesn't go over the window
            if (pos.X + scale > gameX)
            {
                moveDirection.X = -moveDirection.X;
            }

            if (pos.Y + scale > gameY)
            {
                moveDirection.Y = -moveDirection.Y;
            }
        }

        // Generate a random movement direction for BluebubbleEnemy
        private void RandomizeMoveDirection()
        {
            // Generate a random movement direction
            Random random = new Random();
            float angle = (float)random.NextDouble() * MathHelper.TwoPi;
            moveDirection = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

            // Normalize the moveDirection vector
            moveDirection.Normalize();
        }

    }
}
