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
                        index = Program.group.Add(message.Author.Id, message.Author, message.Author.Username, message.Author.ToString(), false, null, "", true);
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
                        string messageToLower = message.Content.ToLower();
                        command = messageToLower.Split(' ');
                    }
                    else
                    {
                        command[0] = message.Content.ToLower();
                    }

                    await CommandHandler(message, index, command);
                }
            }
        }

        private static async Task<int> CommandHandler(SocketMessage message, int index, string[] command)
        {
            switch (command[0].Trim())
            {
                case "!broadcast":
                    index = await Broadcast(message, command);
                    break;

                case "!kill":
                    index = Kill(message);
                    break;

                case "!join":
                    await Join(message);
                    break;

                case "!favourites":
                    Favourites(message, command);
                    break;

                case "!leave":
                    Program.group.Remove(message.Author.Id);
                    break;

                case "!pull":
                    try
                    {
                        index = await Pull(message, Convert.ToInt32(command[1].Trim()),command[2]);

                    }
                    catch
                    {
                        index = await Pull(message, Convert.ToInt32(command[1].Trim()));

                    }
                    break;

                case "!pullB": //pull 1 sharp, 1 blunt and 1 ranged. the first one in each category which have the highest rated weapon type
                    index = await PullB(message, command);
                    break;

                default:
                    Console.WriteLine("a message has been recieved");
                    break;
            }

            return index;
        }

        
        private static async Task<int> Pull(SocketMessage message, int amount)
        {
            int index = Program.group.Find(message.Author.Id);
            if (Program.group.Users[index].IsAdmin)
            {
                User[] pulledUsers = new User[0];
                for (int i = 0; i < amount; i++)
                {
                    try
                    {
                        pulledUsers = Tools.Append(pulledUsers, Program.group.Players()[i]);
                        Program.group.Remove(Program.group.Players()[i].ID);
                    }
                    catch
                    {
                    }
                }
                int p = 1;
                foreach (var player in Program.group.Players())
                {
                    p++;
                    await player.SocketUser.SendMessageAsync($"You are now in position {p}");
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

            return index;
        }

        private static async Task<int> Pull(SocketMessage message, int amount, string text)
        {
            int index = Program.group.Find(message.Author.Id);
            if (Program.group.Users[index].IsAdmin)
            {
                User[] pulledUsers = new User[0];
                for (int i = 0; i < amount; i++)
                {
                    try
                    {
                        await Program.group.Players()[i].SocketUser.SendMessageAsync(text);
                        pulledUsers = Tools.Append(pulledUsers, Program.group.Players()[i]);
                        Program.group.Remove(Program.group.Players()[i].ID);
                    }
                    catch
                    {
                    }
                }
                int p = 1;
                foreach (var player in Program.group.Players())
                {
                    p++;
                    await player.SocketUser.SendMessageAsync($"You are now in position {p}");
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

            return index;
        }
        
        private static async Task<int> PullB(SocketMessage message, string[] command)
        {
            int index = Program.group.Find(message.Author.Id);
            User[] sharp = Program.group.Sharp();
            User[] blunt = Program.group.Blunt();
            User[] ranged = Program.group.Ranged();
            User[]? sortedS = null;
            User[]? sortedB = null;
            User[]? sortedR = null;
            string[]? pulledWeaponS = null;
            string[]? pulledWeaponB = null;
            string[]? pulledWeaponR = null;
            if (Program.group.Users[index].IsAdmin)
            {
                User[]? pulledUsers = null;
                for (int i = 0; i < 1; i++)
                {
                    try
                    {


                        foreach (var suser in sharp)
                        {
                            for (int s = 0; s < suser.Weapons.Length; s++)
                            {
                                if (suser.Weapons[s] == "gs" || suser.Weapons[s] == "sns" || suser.Weapons[s] == "db" || suser.Weapons[s] == "ls" || suser.Weapons[s] == "l" || suser.Weapons[s] == "gl" || suser.Weapons[s] == "sa" || suser.Weapons[s] == "cb" || suser.Weapons[s] == "ig")
                                {
                                    sortedS = Tools.Append(sortedS, suser);
                                    pulledWeaponS = Tools.Append(pulledWeaponS, suser.Weapons[s]);
                                }

                            }
                        }
                        foreach (var buser in blunt)
                        {
                            for (int b = 0; b < buser.Weapons.Length; b++)
                            {
                                if (buser.Weapons[b] == "h" || buser.Weapons[b] == "hh")
                                {
                                    sortedB = Tools.Append(sortedB, buser);
                                    pulledWeaponB = Tools.Append(pulledWeaponB, buser.Weapons[b]);
                                }

                            }
                        }
                        foreach (var ruser in ranged)
                        {
                            for (int r = 0; r < ruser.Weapons.Length; r++)
                            {
                                if (ruser.Weapons[r] == "b" || ruser.Weapons[r] == "lbg" || ruser.Weapons[r] == "hbg")
                                {
                                    sortedR = Tools.Append(sortedR, ruser);
                                    pulledWeaponR = Tools.Append(pulledWeaponR, ruser.Weapons[r]);
                                }

                            }
                        }
                        try
                        {
                            Program.group.Remove(sortedS[0].ID);
                        }
                        catch { }
                        try
                        {
                            Program.group.Remove(sortedB[0].ID);
                        }
                        catch { }
                        try
                        {
                            Program.group.Remove(sortedR[0].ID);
                        }
                        catch { }

                        pulledUsers[0] = sortedS[0] ?? Program.group.Players()[0] ?? null;
                        if (pulledUsers[0] == Program.group.Players()[0])
                        {
                            Program.group.Remove(pulledUsers[0].ID);
                        }

                        pulledUsers[1] = sortedB[0] ?? Program.group.Players()[0] ?? null;
                        if (pulledUsers[1] == Program.group.Players()[0])
                        {
                            Program.group.Remove(pulledUsers[0].ID);
                        }

                        pulledUsers[2] = sortedR[0] ?? Program.group.Players()[0] ?? null;
                        if (pulledUsers[2] == Program.group.Players()[0])
                        {
                            Program.group.Remove(pulledUsers[0].ID);
                        }

                        pulledUsers[0].PulledReason = pulledWeaponS[0] ?? "";
                        pulledUsers[1].PulledReason = pulledWeaponB[0] ?? "";
                        pulledUsers[2].PulledReason = pulledWeaponR[0] ?? "";

                        foreach (var user in pulledUsers)
                        {
                            if (user.PulledReason != null)
                            {
                                await user.SocketUser.SendMessageAsync($"You have been pulled for using {user.PulledReason}, but any weapon is okay to use.\nServer ID:{command[1]}. Have fun!");
                            }
                            else
                            {
                                await user.SocketUser.SendMessageAsync($"You have been pulled!\nServer ID:{command[1]}. Have fun!");

                            }
                        }

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
                        await admin.SocketUser.SendMessageAsync($"Name: {user.UserName}, ID: {user.ID}, Weapon: {user.PulledReason}");
                    }
                }
            }

            return index;
        }

        private static void Favourites(SocketMessage message, string[] command)
        {
            int temp = -1;
            User[] players = Program.group.Players();
            for (int i = 0; i < players.Length; i++)
            {
                User? player = players[i];
                if (player.ID == message.Author.Id)
                {
                    temp = i;
                    break;
                }
            }

            if (temp != -1)
            {
                for (int i = 1; i < command.Length; i++)
                {
                    Tools.Append(Program.group.Users[temp].Weapons, command[i]);
                }


            }
        }

        private static async Task Join(SocketMessage message)
        {
            Program.group.Add(message.Author.Id, message.Author, message.Author.Username, message.Author.ToString(), true, null, "", false);
            Console.WriteLine(Program.group.ToString());
            await message.Author.SendMessageAsync($"You have joined the queue and are currently in position {Program.group.Players().Length}.\n" +
                $"Please tell me your favourite weapons in order, as many as you want. If you don't have any favourites just wait for another message with the server ID\n" +
                $"Example:\"!favourites hbg h sns hh\"\n" +
                $"THIS FEATURE IS STILL BEING DEVELOPED AND DOES NOT CURRENTLY WORK FULLY");//DEV THINGS REMOVE WHEN DONE
        }

        private static int Kill(SocketMessage message)
        {
            int index = Program.group.Find(message.Author.Id);
            if (Program.group.Users[index].IsAdmin)
            {
                Program.SafeExit();
            }

            return index;
        }

        private static async Task<int> Broadcast(SocketMessage message, string[] command)//sends a message to every user
        {
            int index = Program.group.Find(message.Author.Id);
            if (Program.group.Users[index].IsAdmin)
            {
                string messageToSend = command[2].Trim();
                await MessageAll(messageToSend);
            }

            return index;
        } 

        public static async Task<Task> MessageAll(string message) //sends a message to every user currently stored
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
