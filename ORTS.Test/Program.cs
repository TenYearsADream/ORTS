using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core;
using ORTS.Core.Messages;
namespace ORTS.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Engine engine = new Engine())
            {
                engine.Bus.OfType<SystemMessage>().Subscribe(m => Console.WriteLine("{0} SYSTEM - {1}", m.TimeSent.ToString(), m.Message));
                engine.Start();
            }
        }
    }
}
