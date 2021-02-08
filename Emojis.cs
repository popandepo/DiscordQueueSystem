using Discord;

namespace DiscordQueueSystem
{
    class Emojis
    {
        public static Emoji Sword = new Emoji("⚔️");
        public static Emoji Shield = new Emoji("🛡️");
        public static Emoji Wand = new Emoji("🪄");
        public static Emoji Bag = new Emoji("💼");
        public static Emoji Shoes = new Emoji("👟");
        public static Emoji TurnBack = new Emoji("↩️");
        public static Emoji CheckMark = new Emoji("✅");
        public static Emoji CrossMark = new Emoji("❎");
        public static Emoji BackButton = new Emoji("◀️");
        public static Emoji NextButton = new Emoji("▶️");
        public static Emoji QuestionMark = new Emoji("❓");
        public static Emoji Flag = new Emoji("🏁");
        public static Emoji[] Numbers = { //Access using index
            new Emoji("0️⃣"),
            new Emoji("1️⃣"),
            new Emoji("2️⃣"),
            new Emoji("3️⃣"),
            new Emoji("4️⃣"),
            new Emoji("5️⃣"),
            new Emoji("6️⃣"),
            new Emoji("7️⃣"),
            new Emoji("8️⃣"),
            new Emoji("9️⃣"),
            new Emoji("🔟")
        };

        public static IEmote[] loadOrOverwrite = new IEmote[] { CheckMark, CrossMark, Flag };
        public static IEmote[] mainCombatReactions = new IEmote[] { Sword, Shield, Wand, Bag, Shoes, QuestionMark, Flag };
    }
}
