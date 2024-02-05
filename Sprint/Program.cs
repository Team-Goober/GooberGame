
using Microsoft.Xna.Framework;
using System;

string[] c_args = Environment.GetCommandLineArgs();

Game game;

if (c_args.Length == 2 && c_args[1].Equals("test"))
{
    game = new Sprint.Testing.GameTest();
}
else
{
    game = new Sprint.Game1();
}


game.Run();
