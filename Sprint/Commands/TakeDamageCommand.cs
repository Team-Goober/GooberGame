using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;


namespace Sprint.Commands 
{
    internal class TakeDamageCommand : ICommand
    {
        private Player player;
        private SpriteBatch spriteBatch;
        private GameTime gameTime;

        public TakeDamageCommand(Player player)
        {
            this.player = player;
        }

        public void Execute()
        {
            this.player.TakeDamage();


        }
    }
}
