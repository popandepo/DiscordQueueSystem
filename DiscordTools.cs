using Discord;
using Discord.WebSocket;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DiscordQueueSystem
{
    public class DiscordTools
    {

        public static async Task MessageHandler(SocketMessage message) //fires when anyone sends a message
        {
            if (!message.Author.IsBot)
            {
                int index = -1;
                switch (message.Author.Id)
                {
                    case 235921495291854850: //ADD ADMINS HERE
                        index = Program.group.Add(message.Author.Id, message.Author, message.Author.Username, message.Author.ToString(), false, true);
                        break;
                    default:
                        //index = Program.group.Add(message.Author.Id, message.Author, message.Author.Username, message.Author.ToString(), false);
                        break;
                }

                if (message.Content.StartsWith('!'))
                {
                    string[] command = { "-1" };
                    if (message.Content.Contains(' '))
                    {
                        command = message.Content.Split(' ');
                    }
                    else
                    {
                        command[0] = message.Content;
                    }

                    switch (command[0].Trim())
                    {
                        case "!broadcast":
                            index = Program.group.Find(message.Author.Id);
                            if (Program.group.Users[index].IsAdmin)
                            {
                                string messageToSend = command[2].Trim();
                                await Broadcast(messageToSend);
                            }
                            break;

                        case "!kill":
                            index = Program.group.Find(message.Author.Id);
                            if (Program.group.Users[index].IsAdmin)
                            {
                                Program.SafeExit();
                            }
                            break;

                        case "!join":
                            Program.group.Add(message.Author.Id, message.Author, message.Author.Username, message.Author.ToString(), true, false);
                            Console.WriteLine(Program.group.ToString());
                            await message.Author.SendMessageAsync($"You have joined the queue and are currently in position {Program.group.Players().Length}.\n" +
                                $"Please tell me your favourite weapons in order, if you have any. if not just wait for another message with the server ID\n" +
                                $"Example:\"!favourites hbg h sns hh\"\n" +
                                $"THIS FEATURE IS STILL BEING DEVELOPED AND DOES NOT CURRENTLY WORK");//DEV THINGS REMOVE WHEN DONE
                            break;

                        case "!favourites":
                            User[] players = Program.group.Players();
                            bool exists = false;
                            foreach (var player in players)
                            {
                                if (player.ID==message.Author.Id)
                                {
                                    exists = true;
                                }
                            }

                            if (exists)
                            {
                                //add the favourites to a "favourites" list or array in that user
                            }
                            break;

                        case "!leave":
                            Program.group.Remove(message.Author.Id);
                            break;

                        case "!pull":
                            index = Program.group.Find(message.Author.Id);
                            if (Program.group.Users[index].IsAdmin)
                            {
                                User[] pulledUsers = new User[0];
                                for (int i = 0; i < Convert.ToInt32(command[1]); i++)
                                {
                                    try {
                                    await Program.group.Players()[i].SocketUser.SendMessageAsync(command[2]);
                                    pulledUsers = Tools.Append(pulledUsers, Program.group.Players()[i]);
                                    Program.group.Remove(Program.group.Players()[i].ID);
                                    }
                                    catch
                                    {
                                    }
                                }
                                foreach (var admin in Program.group.Admins())
                                {
                                    await admin.SocketUser.SendMessageAsync($"These players have been pulled:");
                                    foreach (var user in pulledUsers)
                                    {
                                        await admin.SocketUser.SendMessageAsync($"Name: {user.UserName}, ID: {user.ID}");
                                    }
                                }
                            }
                            break;
                        case "!pullB": //MAKE THIS PULL BALANCED PEOPLE (DIVIDE TOTAL INTO 3 OR 4 sharp, blunt, ranged then pull the first slots in each of those)
                            index = Program.group.Find(message.Author.Id);
                            if (Program.group.Users[index].IsAdmin)
                            {
                                User[] pulledUsers = new User[0];
                                for (int i = 0; i < Convert.ToInt32(command[1]); i++)
                                {
                                    try
                                    {
                                        await Program.group.Players()[i].SocketUser.SendMessageAsync($"You were pulled for the {"placeholder"} weapon type, but any weapon is okay. server ID: {command[2]}"); //PLACEHOLDER REMOVE WHEN DONE
                                        pulledUsers = Tools.Append(pulledUsers, Program.group.Players()[i]);
                                        Program.group.Remove(Program.group.Players()[i].ID);
                                    }
                                    catch
                                    {
                                    }
                                }
                                foreach (var admin in Program.group.Admins())
                                {
                                    await admin.SocketUser.SendMessageAsync($"These players have been pulled:");
                                    foreach (var user in pulledUsers)
                                    {
                                        await admin.SocketUser.SendMessageAsync($"Name: {user.UserName}, ID: {user.ID}");
                                    }
                                }
                            }
                            break;
                        default:
                            Console.WriteLine("a message has been recieved");
                            break;
                    }
                }
            }
        }

        public static async Task<Task> Broadcast(string message) //sends a message to the default channel in all servers
        {
            var temp = Program.group.Users;
            foreach (var user in temp)
            {
                await user.SocketUser.SendMessageAsync(message);
            }
            return Task.CompletedTask;
        }

        public static async Task<Task> InitUserStorage() //starts the user storage
        {
            //Broadcast("I'm online! Harvesting...");
            await Program._client.DownloadUsersAsync(Program._client.Guilds);
            Program.group = new UserStorage();
            Console.WriteLine("Users are harvested!");
            Program.userStorageInit = true;
            return Task.CompletedTask;
        }

        public static string WriteOrReadFile(string fileName) //reads or writes a file
        {
            string token;

            if (File.Exists(fileName))
            {
                token = File.ReadAllText(fileName);
            }
            else
            {
                Console.WriteLine("Internal BotID missing, please provide BotID");
                token = Console.ReadLine();
                File.WriteAllText(fileName, token);
            }

            return token;
        }
    }
}
