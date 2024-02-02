using Sprint.Interfaces;
using System.Diagnostics;

namespace Sprint.Commands
{
    internal class DebugPrintCommand : ICommand
    {

        private string message;

        public DebugPrintCommand(string msg)
        {
            message = msg;
        }

        public void Execute()
        {
            // Print message to console
            Debug.WriteLine(message);
        }

    }
}
