using Discord;
using Discord.WebSocket;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DiscordQueueSystem
{
    class Program
    {
        public static States currentState;
        public static DiscordSocketClient _client;
        public static UserStorage group;
        public static bool adminLoop;
        public static bool userStorageInit = false;
        static void Main(string[] args) //Calls the state machine to boot then main menu. if anything goes wrong safe exit
        {
            try
            {
                StateMachine(States.Boot);
                StateMachine(States.MainMenu);
            }
            finally
            {
                IOException error = new IOException();
                SafeExit(error);
            }
        }

        public static void StateMachine(States state = States.Default) //tells the program where it is and what it should be doing
        {
            switch (state)
            {
                case States.Boot:
                    StateBoot();
                    break;
                case States.MainMenu:
                    currentState = States.MainMenu;
                    StateMain();
                    break;
                case States.Admin:
                    StateAdmin();
                    break;
                case States.Default:
                    Console.WriteLine("Something might've gone wrong when calling the state machine (case -1)");
                    break;
            }
        }

        public static void StateBoot() //boots up the discord stuff and prints an error if it can't
        {
            try
            {
                new Program().BotStart().GetAwaiter().GetResult();
            }
            catch
            {
                Console.WriteLine("No internet or some other problem, press any key to exit");
                Console.ReadKey();
                SafeExit(10);
            }
        }

        private async Task BotStart() //starts the bot and initiates all handlers and storages
        {
            Console.WriteLine("1");
            _client = new DiscordSocketClient(new DiscordSocketConfig() { AlwaysDownloadUsers = true });
            Console.WriteLine("2");
            string token = DiscordTools.WriteOrReadFile("botid.txt");
            Console.WriteLine("3");
            await _client.LoginAsync(TokenType.Bot, token);
            Console.WriteLine("4");
            await _client.StartAsync();
            Console.WriteLine("5");
            await _client.SetStatusAsync(UserStatus.Online);
            Console.WriteLine("6");
            _client.MessageReceived += DiscordTools.MessageHandler;
            Console.WriteLine("7");
            _client.Ready += DiscordTools.InitUserStorage;
            Console.WriteLine("8");
            while (!userStorageInit)
            {
            }
            //LoadFromFile();
            //Console.WriteLine("memory restored!");
        }

        private static void LoadFromFile() //loads all users from a file
        {
            if (File.Exists("User Storage.txt"))
            {
                Console.WriteLine("found a file");
                Console.ReadLine();
                string fileInput = File.ReadAllText("User Storage.txt");
                fileInput = Tools.Replace("all", fileInput, "\n", "");
                string[] fileInputArray = fileInput.Split('<');
                foreach (var user in fileInputArray)
                {
                    try
                    {
                        var userfulInfo = user.Split('>')[1];
                        var values = userfulInfo.Split(", ");
                        ulong id = Convert.ToUInt64(values[0]);
                        SocketUser sUser = _client.GetUser(id);
                        string userName = values[1];
                        string hashName = values[2];
                        group.Add(id, sUser, userName, hashName);
                    }
                    catch
                    {
                    }
                }
            }
        }

        private static void SaveToFile()//saves all users to file
        {
            File.WriteAllText("User Storage.txt", group.ToString());
        }

        public static void StateMain() //is the main menu screen input and output
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Hello World!");
                Console.WriteLine("type \"exit\" to end program or enter password to get to admin tools");
                string input = Tools.GetString();
                switch (input)
                {
                    case "password":
                        StateMachine(States.Admin);
                        break;
                    case "exit":
                        SafeExit(1);
                        break;
                }
            }

        }

        public static void StateAdmin() //is the admin tools called from the console
        {
            adminLoop = true;
            while (adminLoop)
            {
                Console.Clear();
                Console.WriteLine(TextStorage.adminText);
                MainSwitch(States.Admin, Tools.GetChar());
            }
        }

        private static void MainSwitch(States state, string input) //a collection of switches for doing different logic
        {
            switch (state)
            {
                case States.Admin:
                    switch (input)
                    {
                        case "r":
                        case "R":

                            adminLoop = false;
                            return;

                        case "1":
                            do
                            {
                                Console.Clear();
                                Console.WriteLine(group.ToString());
                                Console.WriteLine("Please enter the person you'd like to change.");
                                Console.WriteLine("type back to return to previous screen");
                                input = Console.ReadLine();
                                try
                                {
                                    int temp = int.Parse(input);
                                    group.Edit(temp, Attributes.IsAdmin, "true");
                                    break;
                                }
                                catch
                                {

                                    try
                                    {
                                        ulong temp = ulong.Parse(input);
                                        group.Edit(temp, Attributes.IsAdmin, "true");
                                        break;
                                    }
                                    catch
                                    {
                                        if (input == "back")
                                        {
                                            return;
                                        }
                                        group.Edit(input, Attributes.IsAdmin, "true");
                                        break;
                                    }

                                }
                            } while (true);
                            break;
                        default:
                            break;
                    }
                    break;
            }
        }

        public static void SafeExit(int exitCode = 0) //performs a safe exit and gives an exitcode
        {
            /* Exitcodes:
             * 0: no exit code defined
             * 1: safe exit
             * 10: possible internet error
             * 42: unhandled exception but managed to safely exit
             */

            SaveToFile();
            _client.SetStatusAsync(UserStatus.Offline);
            _client.LogoutAsync();
            Environment.Exit(exitCode);
        }

        public static void SafeExit(IOException error) //performs a safe exit and sends a message to me containing error information
        {
            /* Exitcodes:
             * 0: no exit code defined
             * 1: safe exit
             * 10: possible internet error
             * 42: unhandled exception but managed to safely exit
             */

            SaveToFile();
            _client.SetStatusAsync(UserStatus.Offline);
            _client.LogoutAsync();
            Environment.Exit(42);
        }
    }
}