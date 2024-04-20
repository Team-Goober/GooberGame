using Sprint.Interfaces;
using Microsoft.Xna.Framework;
using Sprint.Functions.SecondaryItem;
using Sprint.Projectile;
using Sprint.Sprite;
using Sprint.Levels;

namespace Sprint.Characters
{

    
    //Code based on the BluebubbleEnemy.cs file
    internal class SkeletonEnemy : Enemy
    {
        private Vector2 moveDirection; // Movement direction for the random pattern
        private SimpleProjectileFactory itemFactory;
        private ICommand projectileCommand;
        private MoveVert moveVert;
        private Player player;

        private Timer timeAttack;


        public SkeletonEnemy(ISprite sprite, ISprite damagedSprite, Vector2 initialPosition, Room room, SpriteLoader spriteLoader, Player player)
            : base(sprite, damagedSprite, initialPosition, room)
        {

            timeAttack = new Timer(2);
            timeAttack.Start();
            this.player = player;



            health = CharacterConstants.LOW_HP;

            itemFactory = new SimpleProjectileFactory(spriteLoader, 30, true, room);

            projectileCommand = new ShootArrowCommand(itemFactory);

           

            moveVert = new MoveVert(physics, player);
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

            // Move randomly within a specified area
            moveVert.MoveAI(gameTime);

            base.Update(gameTime);

        }

    }
}
