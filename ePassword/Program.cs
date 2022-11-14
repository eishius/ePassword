using System;
using System;
using Con = ePassword.Api.Console.log;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using ePassword.Core;

namespace ePassword
{
    internal class Program
    {

        static void Main(string[] args)
        {

            Console.Title = "Eishius Password Generator";
            Core.Main.Startup();
        }
    }
}
