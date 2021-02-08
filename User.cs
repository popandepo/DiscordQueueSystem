using Discord.WebSocket;

namespace DiscordQueueSystem
{
    public class User
    {
        public User(ulong id, SocketUser socketUser, string userName, string hashName, bool wantsToPlay, bool isAdmin)
        {
            ID = id;
            SocketUser = socketUser;
            UserName = userName;
            HashName = hashName;
            WantsToPlay = wantsToPlay;
            IsAdmin = isAdmin;
        }

        public ulong ID
        { get; set; }

        public SocketUser SocketUser
        { get; set; }

        public string UserName
        { get; set; }

        public string HashName
        { get; set; }

        public bool WantsToPlay
        { get; set; }

        public bool IsAdmin
        { get; set; }
    }
}
