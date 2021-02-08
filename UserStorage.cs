using Discord.WebSocket;
using System;

namespace DiscordQueueSystem
{
    public class UserStorage
    {
        public User[] Users
        { get; set; }

        public UserStorage()
        {
        }
        public string ToString(int index = -1)
        {
            string output = "";

            if (index != -1)
            {
                output += Users[index].ID.ToString();
                output += ", ";
                output += Users[index].UserName;
                output += ", ";
                output += Users[index].HashName;
                output += ", ";
                output += Users[index].WantsToPlay;
                output += ", ";
                output += Users[index].IsAdmin;
                output += ".";
            }
            else
            {
                for (int i = 0; i < Users.Length; i++)
                {
                    output += $"{i + 1}: >{Users[i].ID}, {Users[i].UserName}, {Users[i].HashName}, {Users[i].WantsToPlay}, {Users[i].IsAdmin}<\n";
                }
            }
            return output;

        }
        public int Find<T>(T input)
        {
            try
            {
                for (int i = 0; i < Users.Length; i++)
                {
                    if (Users[i].ID.Equals(input))
                    {
                        return i;
                    }
                    else if (Users[i].UserName.Equals(input))
                    {
                        return i;
                    }
                }
                return -1;
            }
            catch
            {
                return -1;
            }
        }
        public int Edit<T, U>(T searchterm, Attributes target, U replacement)
        {
            int index = Find(searchterm);
            try
            {
                index = Int32.Parse(searchterm.ToString());
                index--;
            }
            catch
            {

            }
            switch (target)
            {
                case Attributes.ID:
                    Users[index].ID = ulong.Parse(replacement.ToString());
                    return index;
                case Attributes.UserName:
                    Users[index].UserName = replacement.ToString();
                    return index;
                case Attributes.HashName:
                    Users[index].HashName = replacement.ToString();
                    return index;
                case Attributes.IsAdmin:
                    Users[index].IsAdmin = bool.Parse(replacement.ToString());
                    return index;
                default:
                    return -1;
            }
        }
        public int Add(ulong id = 0, SocketUser socketUser = null, string userName = "0", string hashName = "0", bool wantsToPlay = true, bool isAdmin = false)
        {
            int index = Find(id);
            if (index == -1)
            {
                User temp = new User(id, socketUser, userName, hashName, wantsToPlay, isAdmin);
                Users = Tools.Append(Users, temp);

                return Users.Length - 1;

            }
            else return index;

        }

        public User[] Admins()
        {
            User[] output = new User[0];
            foreach (var user in Users)
            {
                if (!user.WantsToPlay)
                {
                    output = Tools.Append(output, user);
                }
            }
            return output;
        }

        public User[] Players()
        {
            User[] output = new User[0];
            foreach (var user in Users)
            {
                if (user.WantsToPlay)
                {
                    output = Tools.Append(output,user);
                }
            }
            return output;
        }

        public void Remove(ulong id)
        {
            int index = Find(id);
            if (index != -1)
            {
                for (int i = index; i < Users.Length - 1; i++)
                {
                    try
                    {
                        Users[i] = Users[i + 1];
                    }
                    catch
                    {

                    }
                }
                return;
            }
        }
    }
}
