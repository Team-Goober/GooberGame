using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint.Commands;
using Sprint.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Controllers
{
    public class MouseC : IController
    {
        private MouseState oldState;
        private Game1 game;
        private int mousePosX;
        private int mousePosY;

        //Height: 480
        //Width: 800
        // Quad 1: 0-400, 0-240
        // Quad 2: 400-800, 0-240
        // Quad 3: 0-400, 240-480
        // Quad 4: 400-800, 240-480

        public MouseC(Game1 newGame) 
        {
            this.oldState = Mouse.GetState();
            this.game = newGame;
        }

        public void UpdateInput(GameTime gameTime)
        {
            if(oldState.RightButton == ButtonState.Pressed)
            {
                ICommand quit = new QuitCommand(game);
                quit.Execute();
            }

            if(oldState.LeftButton == ButtonState.Pressed)
            {
                //Get Position
                mousePosX = oldState.X;
                mousePosY = oldState.Y;
                DrawAnimation();
            }

            this.oldState = Mouse.GetState();
        }

        public void DrawAnimation()
        {
            bool quad1 = mousePosX >= 0 && mousePosX <= 400 && mousePosY >= 0 && mousePosY <= 240;
            bool quad2 = mousePosX > 400 && mousePosX <= 800 && mousePosY >= 0 && mousePosY <= 240;
            bool quad3 = mousePosX >= 0 && mousePosX <= 400 && mousePosY > 240 && mousePosY <= 480;
            bool quad4 = mousePosX > 400 && mousePosX <= 800 && mousePosY > 240 && mousePosY <= 480;

            if (quad1)
            {
                ICommand frozen = new Frozen(game);
                frozen.Execute();
            }

            if(quad2)
            {
                ICommand runningInPlace = new RunningInPlace(game);
                runningInPlace.Execute();
            }

            if(quad3)
            {
                ICommand frozenMoveUpandDown = new FrozenMoveUpAndDown(game);
                frozenMoveUpandDown.Execute();
            }

            if(quad4)
            {
                ICommand runningLeftAndRight = new RunningLeftAndRight(game);
                runningLeftAndRight.Execute();
            }
        }
    }
}
