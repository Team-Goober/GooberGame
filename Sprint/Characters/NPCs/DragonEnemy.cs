using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Microsoft.Xna.Framework;
using Sprint.Commands.SecondaryItem;
using System;
using Sprint.Projectile;
using Sprint.Sprite;
using Sprint.Levels;
using Sprint.Music.Sfx;
using System.Runtime.Serialization;
using Sprint.Items;
using System.Collections.Generic;


namespace Sprint.Characters
{
    internal class DragonEnemy : Enemy
    {
        private float elapsedTime;
        private Timer timeAttack;
        private Vector2 moveDirection; // Movement direction for the random pattern
        private ICommand projectileCommand;
        private SimpleProjectileFactory itemFactory;
        private Vector2 initialPosition;
        private string lastAnimationName;
        private SfxFactory sfxFactory;
        private SceneObjectManager objectManager;
        private MoveVert moveVert;

        public DragonEnemy(ISprite sprite, ISprite damagedSprite, Vector2 initialPosition, Room room, SpriteLoader spriteLoader)
            : base(sprite, damagedSprite, initialPosition, room)
        {

            sfxFactory = SfxFactory.GetInstance();
            this.objectManager = room.GetScene();

            // Store the initial position for reference
            this.initialPosition = initialPosition;

            timeAttack = new Timer(2);
            timeAttack.Start();

            health = 10;

            itemFactory = new SimpleProjectileFactory(spriteLoader, 30, true, room);


            moveVert = new MoveVert(physics);


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
            if (Math.Abs(moveVert.moveDirection.X) > Math.Abs(moveVert.moveDirection.Y))
            {

                if (moveDirection.X > 0)
                    newAnim = "rightFacing";
                else
                    newAnim = "leftFacing";

            }
            else
            {

                if (moveVert.moveDirection.Y > 0)
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
