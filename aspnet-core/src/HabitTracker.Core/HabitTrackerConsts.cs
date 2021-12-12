using HabitTracker.Debugging;

namespace HabitTracker
{
    public class HabitTrackerConsts
    {
        public const string LocalizationSourceName = "HabitTracker";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "3136ca6c641e443fa3a42a44de2509e0";
    }
}
