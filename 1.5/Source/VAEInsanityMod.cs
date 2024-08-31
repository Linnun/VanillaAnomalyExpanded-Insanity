using UnityEngine;
using Verse;

namespace VAEInsanity
{
    public class VAEInsanityMod : Mod
    {
        private VAEInsanityModSettings settings;

        public VAEInsanityMod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<VAEInsanityModSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            var ls = new Listing_Standard();
            ls.Begin(inRect);
            ls.CheckboxLabeled("VAEI_EnableSelfHarmContent".Translate(), ref VAEInsanityModSettings.selfHarmEnabled);
            ls.End();
        }
        public override string SettingsCategory()
        {
            return "VAEI_ModSettingsName".Translate();
        }
    }

    public class VAEInsanityModSettings : ModSettings
    {
        public static bool selfHarmEnabled = false;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref selfHarmEnabled, "selfHarmEnabled");
        }
    }
}
