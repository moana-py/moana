using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExifDateTimeMaker
{
    public class CommandOption
    {
        public string rootDirectory = "";
        public string outputDirectory = "";
        public string overwrite = "";
        public string dateTimeMode = "";
        public bool recursive = false;

        public CommandOption() {
        
        }
        public bool SetCommandOption(string[] args)
        {
            string sampleCommand = "ex) /from <FolderPath> /DateTimeMode <CT|MT|AT> /r(option) /to(option) <FolderPath>";
            if (args.Length == 0)
            {
                Console.WriteLine("command is emtpy. Please enter the correct command.");
                Console.WriteLine(sampleCommand);
                return false;
            }
            else
            {
                if (args.Length < 4)
                {
                    Console.WriteLine(args.ToString() + " - command is wrong. Please enter the correct command.");
                    Console.WriteLine(sampleCommand);
                    return false;
                }
                else
                {
                    for (var i = 0; i < args.Length; i++)
                    {
                        string command = args[i];

                        if (command == "/from")
                        {
                            rootDirectory = args[++i];
                        }
                        else if (command == "/to")
                        {
                            outputDirectory = args[++i];
                        }
                        else if (command == "/DateTimeMode")
                        {
                            dateTimeMode = args[++i];
                        }
                        else if (command == "/r")
                        {
                            recursive = true;
                        }
                        else if (command == "/overwrite")
                        {
                            overwrite = args[++i];
                        }
                        else
                        {
                            continue;
                        }

                    }
                }
            }
            string commandLine = string.Format("Scan Directory - Path = \"{0}\", DateTimeMode = \"{1}\", Recursive = {2}, Output Path = {3}",
                rootDirectory, dateTimeMode, recursive.ToString(), outputDirectory);

            Console.WriteLine(commandLine);

            return true;
        }
    }
}
