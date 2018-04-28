using System;

namespace Archon.engine.utils
{
    class DesktopConsoleLogger : ALogger
    {
        public void debug(string tag, string message)
        {
            Console.WriteLine("[DEBUG] "+ tag + " : " + message);
        }

        public void err(string tag, string message)
        {
            Console.WriteLine("[ERROR] "+tag + " : " + message);
        }

        public void log(string tag, string message)
        {
            Console.WriteLine("[LOG] "+tag + " : " + message);
        }
    }
}
