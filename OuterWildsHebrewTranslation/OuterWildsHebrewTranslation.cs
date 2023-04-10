using HebrewTranslation;
using OWML.Common;
using OWML.ModHelper;

namespace OuterWildsHebrewTranslation
{
    public class OuterWildsHebrewTranslation : ModBehaviour
    {
        private void Awake()
        {
            // You won't be able to access OWML's mod helper in Awake.
            // So you probably don't want to do anything here.
            // Use Start() instead.
        }

        private void Start()
        {
            // Starting here, you'll have access to OWML's mod helper.
            ModHelper.Console.WriteLine($"Hebrew translation was loaded successfully!", MessageType.Success);

            var api = ModHelper.Interaction.TryGetModApi<ILocalizationAPI>("xen.LocalizationUtility");
            api.RegisterLanguage(this, "Hebrew", "assets/Translation.xml");
            api.AddLanguageFont(this, "Hebrew", "assets/rubikbundle", "Assets/Rubik-Regular.ttf");
            //api.AddLanguageFixer(this, )
        }
    }
}