using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DiscordQueueSystem.Config
{
    public class BotConfig
    {
        [JsonPropertyName("ChannelId")]
        public List<ulong> ChannelIDs { get; set; }

        [JsonPropertyName("AdminId")]
        public List<string> AdminId { get; set; }
    }
}
