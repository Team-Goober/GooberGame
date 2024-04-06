
using Sprint.Interfaces;
using Microsoft.Xna.Framework;
using Sprint.Commands.SecondaryItem;
using Sprint.Projectile;
using Sprint.Sprite;
using Sprint.Levels;

namespace Sprint.Characters
{
    internal class DogEnemy : Enemy
    {
        private Timer timeAttack;
        private Vector2 moveDirection; // Movement direction for the random pattern
        private ICommand projectileCommand;
        private SimpleProjectileFactory itemFactory;
        private string lastAnimationName;
        private MoveVert moveVert;

        public DogEnemy(ISprite sprite, ISprite damagedSprite, Vector2 initialPosition, Room room, SpriteLoader spriteLoader)
            : base(sprite, damagedSprite, initialPosition, room)
        {

            timeAttack = new Timer(2);
            timeAttack.Start();

            health = CharacterConstants.MID_HP;

            itemFactory = new SimpleProjectileFactory(spriteLoader, 30, true, room);

            projectileCommand = new ShootBoomarangC(itemFactory);

            moveVert = new MoveVert(physics);

        }

        // Register a directional animation for DogEnemy sprite
        public void RegisterDirectionalAnimation(string animationLabel, IAtlas atlas)
        {
            sprite.RegisterAnimation(animationLabel, atlas);
        }

        // Set the current animation for DogEnemy sprite
        public void SetAnimation(string animationLabel)
        {
            sprite.SetAnimation(animationLabel);
        }

        // Set the scale of DogEnemy sprite
        public void SetScale(int scale)
        {
            sprite.SetScale(scale);
        }

        // Update DogEnemy logic
        public override void Update(GameTime gameTime)
        {
            timeAttack.Update(gameTime);
            base.Update(gameTime);

            // Uses timer to shoot projectiles every 2 seconds
            if (timeAttack.JustEnded)
            {
                itemFactory.SetStartPosition(physics.Position);
                itemFactory.SetDirection(moveDirection);
                projectileCommand.Execute();
                timeAttack.Start();
            }

            // Move randomly within a specified area
            moveVert.MoveAI(gameTime);

            // Set animation based on the new direction
            SetAnimationBasedOnDirection();

            // Update the sprite and physics
            sprite.Update(gameTime);
            physics.Update(gameTime);
        }

        // Set animation based on the direction of movement
        private void SetAnimationBasedOnDirection()
        {
            string newAnim = "";
            moveDirection = moveVert.directionFace;
            if (moveDirection == Directions.DOWN)
            {
                newAnim = "downFacing";
            }
            else if (moveDirection == Directions.LEFT)
            {
                newAnim = "leftFacing";
            }
            else if(moveDirection == Directions.UP)
            {
                newAnim = "upFacing";
            }
            else if(moveDirection == Directions.RIGHT)
            {
                newAnim = "rightFacing";
            }
            
            if (newAnim != lastAnimationName)
            {
                lastAnimationName = newAnim;
                SetAnimation(newAnim);
            }



        }




    }
}
