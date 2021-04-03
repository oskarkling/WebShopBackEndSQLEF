using System;
using Controllers;

namespace ConsoleMVC
{
    class Program
    {
        static void Main(string[] args)
        {
            CoreController coreLogic = new CoreController();
            coreLogic.Start();
        }
    }
}
