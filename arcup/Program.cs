using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arcup
{
    class Program
    {
        static void Main(string[] args)
        {
            int defaultOption = (int)UI.Options.Download;

            if (args.Length == 1)
                int.TryParse(args[0], out defaultOption);

            UI ui = new UI();
            ui.Startup((UI.Options)defaultOption);
        }

    }
}
