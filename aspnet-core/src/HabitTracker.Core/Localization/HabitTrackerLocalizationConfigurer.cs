using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace HabitTracker.Localization
{
    public static class HabitTrackerLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(HabitTrackerConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(HabitTrackerLocalizationConfigurer).GetAssembly(),
                        "HabitTracker.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
