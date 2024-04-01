using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Levels;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Sprint.Functions
{
    internal class ScrollStatesCommand : ICommand
    {
        Goober game;
        IGameState from;
        IGameState to;
        Vector2 direction;

        public ScrollStatesCommand(Goober game, IGameState from, IGameState to, Vector2 direction)
        {
            this.game = game;
            this.from = from;
            this.to = to;
            this.direction = direction;
        }


        public void Execute()
        {
            TransitionState scroll = new TransitionState(game, from.AllObjectManagers(),
                to.AllObjectManagers(), new List<SceneObjectManager> { },
                direction, 0.75f, Vector2.Zero, to);

            from.PassToState(scroll);
        }
    }
}
