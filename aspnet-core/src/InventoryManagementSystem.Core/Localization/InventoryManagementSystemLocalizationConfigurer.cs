using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace InventoryManagementSystem.Localization
{
    public static class InventoryManagementSystemLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(InventoryManagementSystemConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(InventoryManagementSystemLocalizationConfigurer).GetAssembly(),
                        "InventoryManagementSystem.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
