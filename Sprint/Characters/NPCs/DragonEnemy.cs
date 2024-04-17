using Sprint.Interfaces;
using Microsoft.Xna.Framework;
using System;
using Sprint.Projectile;
using Sprint.Sprite;
using Sprint.Levels;
using Sprint.Music.Sfx;
using Sprint.Commands.SecondaryItem;



namespace Sprint.Characters
{
    internal class DragonEnemy : Enemy
    {
        private Timer timeAttack;
        private Vector2 moveDirection; // Movement direction for the random pattern
        private ICommand projectileCommand;
        private SimpleProjectileFactory itemFactory;
        private string lastAnimationName;
        private SfxFactory sfxFactory;
        private SceneObjectManager objectManager;
        private MoveVert moveVert;
        private Player player;
        private MoveRandom moveRandom;

        public DragonEnemy(ISprite sprite, ISprite damagedSprite, Vector2 initialPosition, Room room, SpriteLoader spriteLoader, Player player)
            : base(sprite, damagedSprite, initialPosition, room)
        {

            sfxFactory = SfxFactory.GetInstance();
            this.objectManager = room.GetScene();
            this.player = player;

            timeAttack = new Timer(2);
            timeAttack.Start();

            health = CharacterConstants.HIGH_HP;

            itemFactory = new SimpleProjectileFactory(spriteLoader, 30, true, room);

            projectileCommand = new ShootFireBallC(itemFactory);

            moveRandom = new MoveRandom(physics, player);


        }

        // Register a directional animation for DragonEnemy sprite
        public void RegisterDirectionalAnimation(string animationLabel, IAtlas atlas)
        {
            sprite.RegisterAnimation(animationLabel, atlas);
        }

        // Set the current animation for DragonEnemy sprite
        public void SetAnimation(string animationLabel)
        {
            sprite.SetAnimation(animationLabel);
        }

        // Set the scale of DragonEnemy sprite
        public void SetScale(int scale)
        {
            sprite.SetScale(scale);
        }

        // Update DragonEnemy logic
        public override void Update(GameTime gameTime)
        {
            timeAttack.Update(gameTime);
            base.Update(gameTime);


            // Move randomly within a specified area
            moveRandom.MoveAI(gameTime);

            if (timeAttack.JustEnded)
            {
                itemFactory.SetStartPosition(physics.Position);
                itemFactory.SetDirection(Directions.LEFT);
                projectileCommand.Execute();
                timeAttack.Start();
            }

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
            if (Math.Abs(moveRandom.moveDirection.X) > Math.Abs(moveRandom.moveDirection.Y))
            {

                if (moveRandom.moveDirection.X > 0)
                    newAnim = "rightFacing";
                else
                    newAnim = "leftFacing";

            }
            else
            {

                if (moveRandom.moveDirection.Y > 0)
                    newAnim = "upFacing";
                else
                    newAnim = "downFacing";
            }

            if (newAnim != lastAnimationName)
            {
                lastAnimationName = newAnim;
                SetAnimation("dragmoving");
            }


        }


        // Remove enemy from game
        public override void Die()
        {
            objectManager.Remove(this);
            sfxFactory.PlaySoundEffect("Boss Defeated");
        }

    }
}
