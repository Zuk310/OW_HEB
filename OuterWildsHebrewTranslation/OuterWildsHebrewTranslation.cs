using HebrewTranslation;
using OWML.Common;
using OWML.ModHelper;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

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
            api.AddLanguageFont(this, "Hebrew", "assets/hebrewbundle", "Assets/Rubik-Regular.ttf");
			api.AddLanguageFixer("Hebrew", Fix);
        }

		public string Fix(string s)
		{
			// For now we're using this fixer for Arabic because it handles having the language be right-to-left
			// In the future we can find a better specialized version for Hebrew
			s = ArabicSupport.ArabicFixer.Fix(s);

			// We have to undo some things done by the fixer which affect text tags

			// Text tags also end up going rtl but then they stop working so we flip them all
			s = SwapSubStrings(s, ">", "<");

			foreach (Match match in Regex.Matches(s, "<[^/].*?>"))
			{
				var tag = match.Value;

				// If it doesn't find it then it'll be -1 which will be the min which is bad
				var equalsIndex = tag.IndexOf('=');
				equalsIndex = equalsIndex == -1 ? int.MaxValue : equalsIndex;

				var endIndex = Math.Min(tag.IndexOf('>'), equalsIndex);

				var closeTag = "</" + tag.Substring(1, endIndex - 1) + ">";

				s = SwapSubStrings(s, tag, closeTag);
			}

			foreach (Match match in Regex.Matches(s, "<size=.*?>"))
			{
				var tag = match.Value;

				// The fixer also translates the numbers in tags so we have to switch them back to hindu-arabic numerals
				var replaced = tag;
				foreach (var keyValuePair in easternToWesternNumeral)
				{
					replaced = replaced.Replace(keyValuePair.Key, keyValuePair.Value);
				}

				s = s.Replace(tag, replaced);
			}

			return s;
		}

		public string SwapSubStrings(string s, string substring1, string substring2)
		{
			var placeholder = new Guid().ToString();
			s = s.Replace(substring1, placeholder);
			s = s.Replace(substring2, substring1);
			s = s.Replace(placeholder, substring2);
			return s;
		}

		public Dictionary<char, char> easternToWesternNumeral = new Dictionary<char, char>()
		{
			{ (char)0x0660, '0' },
			{ (char)0x0661, '1' },
			{ (char)0x0662, '2' },
			{ (char)0x0663, '3' },
			{ (char)0x0664, '4' },
			{ (char)0x0665, '5' },
			{ (char)0x0666, '6' },
			{ (char)0x0667, '7' },
			{ (char)0x0668, '8' },
			{ (char)0x0669, '9' },
		};
	}
}