using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using RecursiveRenamer.Business;
using RecursiveRenamer.Models;

namespace RecursiveRenamer
{
    class Program
    {
        private static RenameBusiness renameBusiness;

        static void Main(string[] args)
        {
            string[] input;

            try
            {

                Initialize();
                List<string> argsList;
                Dictionary<String, String> inputArgs;
                bool exit = true;
                input = args;

                do
                {

                    argsList = input.ToList();
                    inputArgs = new Dictionary<string, string>();

                    foreach (var arg in argsList)
                    {
                        if (arg.StartsWith("-"))
                        {
                            if (arg.Equals("--run", StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (argsList.Count > argsList.IndexOf(arg) + 1 && !argsList[argsList.IndexOf(arg) + 1].StartsWith("-"))
                                {
                                    inputArgs.Add("RUN", argsList[argsList.IndexOf(arg) + 1]);
                                }
                                else
                                {
                                    throw new InvalidOperationException("Must saved patern, on --run");
                                }
                            }
                            else if (arg.Equals("--exec", StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (argsList.Count > argsList.IndexOf(arg) + 1 && !argsList[argsList.IndexOf(arg) + 1].StartsWith("-"))
                                {
                                    inputArgs.Add("FIND_PATERN", argsList[argsList.IndexOf(arg) + 1]);
                                }
                                else
                                {
                                    throw new InvalidOperationException("Must inform the find patern, on --exec");
                                }

                                if (argsList.Count > argsList.IndexOf(arg) + 2 && !argsList[argsList.IndexOf(arg) + 2].StartsWith("-"))
                                {
                                    inputArgs.Add("REPLACE_PATERN", argsList[argsList.IndexOf(arg) + 2]);
                                }
                                else
                                {
                                    throw new InvalidOperationException("Must inform the replace patern, on --exec");
                                }

                            }
                            else if (arg.Equals("--path", StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (argsList.Count > argsList.IndexOf(arg) + 1 && !argsList[argsList.IndexOf(arg) + 1].StartsWith("-"))
                                {
                                    inputArgs.Add("PATH", argsList[argsList.IndexOf(arg) + 1]);
                                }
                                else
                                {
                                    throw new InvalidOperationException("Must inform a path, on --path");
                                }
                            }
                            else if (arg.Equals("--list", StringComparison.InvariantCultureIgnoreCase))
                            {
                                inputArgs.Add("LIST", "");
                            }
                            else if (arg.Equals("--save", StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (argsList.Count > argsList.IndexOf(arg) + 1 && !argsList[argsList.IndexOf(arg) + 1].StartsWith("-"))
                                {
                                    inputArgs.Add("SAVE", argsList[argsList.IndexOf(arg) + 1]);
                                }
                                else
                                {
                                    throw new InvalidOperationException("Must inform the name, on --save");
                                }
                            }
                            else if (arg.Equals("--dir-filter", StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (argsList.Count > argsList.IndexOf(arg) + 1 && !argsList[argsList.IndexOf(arg) + 1].StartsWith("-"))
                                {
                                    inputArgs.Add("DIR_FILTER", argsList[argsList.IndexOf(arg) + 1]);
                                }
                                else
                                {
                                    throw new InvalidOperationException("Must inform the filter, on --dir-filter");
                                }
                            }
                            else if (arg.Equals("--file-filter", StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (argsList.Count > argsList.IndexOf(arg) + 1 && !argsList[argsList.IndexOf(arg) + 1].StartsWith("-"))
                                {
                                    inputArgs.Add("FILE_FILTER", argsList[argsList.IndexOf(arg) + 1]);
                                }
                                else
                                {
                                    throw new InvalidOperationException("Must inform the filter, on --file-filter");
                                }
                            }
                            else if (arg.Equals("--help", StringComparison.InvariantCultureIgnoreCase) ||
                                     arg.Equals("-h", StringComparison.InvariantCultureIgnoreCase))
                            {
                                inputArgs.Add("HELP", "");
                            }
                            else if (arg.Equals("--version", StringComparison.InvariantCultureIgnoreCase) ||
                                     arg.Equals("-v", StringComparison.InvariantCultureIgnoreCase))
                            {
                                inputArgs.Add("VERSION", "");
                            }
                            else if (arg.Equals("--env", StringComparison.InvariantCultureIgnoreCase) ||
                                     arg.Equals("-env", StringComparison.InvariantCultureIgnoreCase))
                            {
                                inputArgs.Add("ENV", "");
                            }
                            else if (arg.Equals("--no-exit", StringComparison.InvariantCultureIgnoreCase))
                            {
                                inputArgs.Add("NOEXIT", "");
                            }
                            else if (arg.Equals("--exit", StringComparison.InvariantCultureIgnoreCase))
                            {
                                inputArgs.Add("EXIT", "");
                            }
                            else
                            {
                                throw new InvalidOperationException("Invalid command use -h or --help to a list of valid commands.");
                            }
                        }
                    }


                    if (inputArgs.Count > 0 && !inputArgs.ContainsKey("EXIT"))
                    {

                        if (inputArgs.ContainsKey("NOEXIT"))
                        {
                            exit = false;
                        }

                        if (inputArgs.ContainsKey("EXEC"))
                        {
                            execute(inputArgs);
                        }
                        else if (inputArgs.ContainsKey("RUN"))
                        {
                            Run(inputArgs);
                        }
                        else if (inputArgs.ContainsKey("LIST"))
                        {
                            List();
                        }
                        else if (inputArgs.ContainsKey("VERSION"))
                        {
                            Console.WriteLine("RecursiveRenamer version: " + Assembly.GetEntryAssembly().GetName().Version);
                        }
                        else if (inputArgs.ContainsKey("ENV"))
                        {
                            Console.WriteLine("Current working directory: " + renameBusiness.CurrentDir);
                        }
                        else if (inputArgs.ContainsKey("HELP"))
                        {
                            help();
                        }
                        else
                        {
                            Console.WriteLine("Invalid command, use -h or --help to see a list of valid commands.");
                        }

                    }
                    else if (inputArgs.ContainsKey("EXIT"))
                    {
                        exit = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid command, use -h or --help to see a list of valid commands.");
                    }

                    if (!exit)
                    {

                        input = Console.In.ReadLine().Split(" ");
                    }

                } while (!exit);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void execute(Dictionary<string, string> inputArgs)
        {
            List<string> result;
            RenamePatern renamePatern;

            try
            {

                if (!inputArgs.ContainsKey("RUNPATH"))
                {
                    inputArgs.Add("RUNPATH", renameBusiness.CurrentDir);
                }

                renamePatern = BuiltPatern(inputArgs);

                if (string.IsNullOrEmpty(renamePatern.Name))
                {
                    result = renameBusiness.Execute(renamePatern, inputArgs.GetValueOrDefault("RUNPATH"));
                }
                else
                {
                    result = renameBusiness.Save(renamePatern);
                }

                foreach (string item in result)
                {
                    Console.WriteLine(item);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static RenamePatern BuiltPatern(Dictionary<string, string> inputArgs)
        {

            if (!inputArgs.ContainsKey("DIR_FILTER"))
            {
                inputArgs.Add("DIR_FILTER", "");
            }

            if (!inputArgs.ContainsKey("FILE_FILTER"))
            {
                inputArgs.Add("FILE_FILTER", "");
            }

            if (!inputArgs.ContainsKey("SAVE"))
            {
                inputArgs.Add("SAVE", "");
            }

            RenamePatern result = new RenamePatern(inputArgs.GetValueOrDefault("SAVE"),
                                                    inputArgs.GetValueOrDefault("DIR_FILTER"),
                                                    inputArgs.GetValueOrDefault("FILE_FILTER"),
                                                    inputArgs.GetValueOrDefault("FIND_PATERN"),
                                                    inputArgs.GetValueOrDefault("REPLACE_PATERN"));

            return result;

        }

        private static void Run(Dictionary<string, string> inputArgs)
        {
            List<string> result;
            RenamePatern renamePatern;

            try
            {

                if (!inputArgs.ContainsKey("RUNPATH"))
                {
                    inputArgs.Add("RUNPATH", renameBusiness.CurrentDir);
                }

                renamePatern = renameBusiness.Open(inputArgs.GetValueOrDefault("RUN"));

                if (renamePatern != null)
                {
                    result = renameBusiness.Execute(renamePatern, inputArgs.GetValueOrDefault("RUNPATH"));
                    foreach (string item in result)
                    {
                        Console.WriteLine(item);
                    }
                }
                else
                {
                    Console.WriteLine($"Pattern no found: {inputArgs.GetValueOrDefault("RUN")}");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void List()
        {
            List<string> result;

            try
            {
                result = renameBusiness.List();
                foreach (string item in result)
                {
                    Console.WriteLine(item);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        private static void help()
        {
            Console.WriteLine("Run:");
            Console.WriteLine(" --run <mapper file name> [optional directory path]");
            Console.WriteLine("     --replace (optional replace existing files)");
            Console.WriteLine("");

            Console.WriteLine("Run Sequence:");
            Console.WriteLine(" --run-seq <sequence file name> [optional directory path]");
            Console.WriteLine("     --replace (optional replace existing files)");
            Console.WriteLine("");

            Console.WriteLine("Initialize mapper:");
            Console.WriteLine(" --init <mapper file name>");
            Console.WriteLine("     --interactive (optional inform values)");
            Console.WriteLine("");

            Console.WriteLine("Initialize sequence:");
            Console.WriteLine(" --init-seq <sequence file name> ");
            Console.WriteLine("     --seq [Mappers List (Space separated)]");
            Console.WriteLine("");

            Console.WriteLine("Open Mapper Files:");
            Console.WriteLine(" --open [Mapper name optional]");
            Console.WriteLine("");

            Console.WriteLine("Open Sequence Files:");
            Console.WriteLine(" --open-seq [Sequence name optional]");
            Console.WriteLine("");

            Console.WriteLine(" Get current dir:");
            Console.WriteLine("     -env or --env");
            Console.WriteLine("");

            Console.WriteLine(" Program Version:");
            Console.WriteLine("     -v or --version");

            Console.WriteLine(" Help:");
            Console.WriteLine("     -h or --help");
        }

        private static void Initialize()
        {
            string auxPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            auxPath = Path.Combine(auxPath, "RecursiveReplacer");
            if (!Directory.Exists(auxPath))
            {
                Directory.CreateDirectory(auxPath);
            }
            renameBusiness = new RenameBusiness(auxPath, Environment.CurrentDirectory);
        }
    }
}
