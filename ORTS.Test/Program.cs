using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core;
using ORTS.Core.Messaging.Messages;
namespace ORTS.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (InstancingExample test = new InstancingExample())
            {
                test.Run();
            }
        }
    }
}
