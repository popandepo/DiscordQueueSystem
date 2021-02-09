using Discord.WebSocket;

namespace DiscordQueueSystem
{
    public class User
    {
        public User(ulong id, SocketUser socketUser, string userName, string hashName, bool wantsToPlay, string[]? weapons, string pulledReason, bool isAdmin)
        {
            ID = id;
            SocketUser = socketUser;
            UserName = userName;
            HashName = hashName;
            WantsToPlay = wantsToPlay;
            Weapons = weapons;
            PulledReason = pulledReason;
            IsAdmin = isAdmin;
            //GS SNS DB LS H HH L GL SA CB IG B LBG HBG
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

        public string[] Weapons
        { get; set; }

        public string PulledReason
        { get; set; }

        public bool IsAdmin
        { get; set; }
    }
}
