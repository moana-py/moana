using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ExifDateTimeMaker;

namespace MOANA_RED
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandOption c = new CommandOption();
            if (c.SetCommandOption(args) == false)
                return;
            var dateTimeMaker = new DateTimeMarker(c.rootDirectory, c.dateTimeMode, c.recursive, c.outputDirectory, c.overwrite);
            dateTimeMaker.Execute();
        }
    }
}
