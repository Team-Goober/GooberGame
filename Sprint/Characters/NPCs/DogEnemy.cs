
using Sprint.Interfaces;
using Microsoft.Xna.Framework;
using Sprint.Commands.SecondaryItem;
using Sprint.Projectile;
using Sprint.Sprite;
using Sprint.Levels;
using System.Runtime.CompilerServices;
using System.Threading;

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
        private ProjBoomarang projBoomarang;
        private Timer delayTimer;
        private bool shooting;
        private Player player;

        public DogEnemy(ISprite sprite, ISprite damagedSprite, Vector2 initialPosition, Room room, SpriteLoader spriteLoader, Player player)
            : base(sprite, damagedSprite, initialPosition, room)
        {

            //timeAttack = new Timer(2);
            //timeAttack.Start();

            delayTimer = new Timer(1);
            shooting = false;


            this.player = player;


            health = CharacterConstants.MID_HP;


            projBoomarang = new ProjBoomarang(spriteLoader, room, moveDirection);

            moveVert = new MoveVert(physics, player);

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


            projBoomarang.Update(gameTime, physics, moveDirection);

            if (projBoomarang.shootingTF && !shooting)
            {
                shooting = true;
                delayTimer.Start();
                
            }



            delayTimer.Update(gameTime);


            // Set animation based on the new direction
            SetAnimationBasedOnDirection();

            if (!shooting)
            {
                // Move randomly within a specified area
                moveVert.MoveAI(gameTime);
                physics.Update(gameTime);
            }
            else if (delayTimer.Ended)
            {

                shooting = false;
            }

            // Update the sprite and physics
            sprite.Update(gameTime);



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
