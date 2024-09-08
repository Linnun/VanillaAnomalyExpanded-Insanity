using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace VAEInsanity
{
    [HotSwappable]
    public class VAEInsanityMod : Mod
    {
        private VAEInsanityModSettings settings;

        public VAEInsanityMod(ModContentPack content) : base(content)
        {
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                this.settings = GetSettings<VAEInsanityModSettings>();
            });
        }

        private float sectionScrollHeight = int.MaxValue;
        private Vector2 scrollPosition = Vector2.zero;

        public override void DoSettingsWindowContents(Rect inRect)
        {
            float widthWithoutScrollBar = inRect.width - 16;
            var viewRect = new Rect(inRect.x, inRect.y, widthWithoutScrollBar, sectionScrollHeight);
            sectionScrollHeight = 0;
            Widgets.BeginScrollView(inRect, ref scrollPosition, viewRect);

            Vector2 pos = new Vector2(inRect.x, inRect.y); // Initialize position

            // Consistent label offset for list items
            float labelXOffsetForListItems = 10;

            // Draw the checkbox for self-harm content
            Widgets.CheckboxLabeled(new Rect(pos.x, pos.y, widthWithoutScrollBar - 5, 24), "VAEI_EnableSelfHarmContent".Translate(), ref VAEInsanityModSettings.selfHarmEnabled);
            pos.y += 24;

            // Draw the label for sanity loss effects
            Widgets.Label(new Rect(pos.x, pos.y, widthWithoutScrollBar, 24), "VAEI_SanityLossEffects".Translate());
            pos.y += 24;

            // Draw the twisted meat slider WITHOUT offset for label
            DrawCheckboxAndSlider(ref VAEInsanityModSettings.twistedMeatValue.enabled,
                ref VAEInsanityModSettings.twistedMeatValue.sanityValue,
                ref pos, widthWithoutScrollBar, "VEAI_EatingTwistedMeat".Translate(), -0.1f, -0.001f);


            DrawList(ref pos, widthWithoutScrollBar, "VAEI_DisturbingInitiatorEffects".Translate(), VAEInsanityModSettings.disturbingInitiatorEffects, labelXOffsetForListItems);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_InteractionEffects".Translate(), VAEInsanityModSettings.interactionEffects, labelXOffsetForListItems);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_NonDisturbingInitiatorEffects".Translate(), VAEInsanityModSettings.nonDisturbingInitiatorEffects, labelXOffsetForListItems);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_HediffEffects".Translate(), VAEInsanityModSettings.hediffEffects, labelXOffsetForListItems);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_UsedThingsEffects".Translate(), VAEInsanityModSettings.usedThingsEffects, labelXOffsetForListItems);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_RitualEffects".Translate(), VAEInsanityModSettings.ritualEffects, labelXOffsetForListItems);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_SuppressingEntities".Translate(), VAEInsanityModSettings.suppressingEntities, labelXOffsetForListItems);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_StudyingEntities".Translate(), VAEInsanityModSettings.studyingEntities, labelXOffsetForListItems);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_KillingEntities".Translate(), VAEInsanityModSettings.killingEntities, labelXOffsetForListItems);

            sectionScrollHeight = pos.y - inRect.y;
            Widgets.EndScrollView();
        }

        private void DrawList<T>(ref Vector2 pos, float width, string sectionLabel, Dictionary<T, SanityEffect> list,
            float labelXOffset) where T : Def
        {
            // Draw section label
            Widgets.Label(new Rect(pos.x, pos.y, width, 24), sectionLabel);
            pos.y += 24;  // Reduced padding after label

            // Calculate the height of the list section based on the number of items
            float listHeight = list.Count * 24 + 10;  // Restored item height to 30, reduced padding

            // Define the menu section rect and draw it
            Rect listRect = new Rect(pos.x, pos.y, width, listHeight);
            Widgets.DrawMenuSection(listRect);
            pos.y += 5;  // Reduced padding inside the section

            // Draw each item in the list with checkbox and slider
            foreach (var kvp in list.ToList())
            {
                var enabled = kvp.Value.enabled;
                var value = kvp.Value.sanityValue.RandomInRange;

                // Draw checkbox and slider with offset for label
                var defLabel = kvp.Key is RitualOutcomeEffectDef ritualDef ? kvp.Key.defName : (string)kvp.Key.LabelCap;
                DrawCheckboxAndSlider(ref enabled, ref value, ref pos, width, defLabel, -0.1f, -0.001f, labelXOffset);

                // Update the dictionary with new values
                list[kvp.Key] = new SanityEffect
                {
                    enabled = enabled,
                    sanityValue = new FloatRange(value),
                };
            }

            // Update pos.y after the list section
            pos.y += 10;  // Reduced padding after the list section
        }

        public void DrawCheckboxAndSlider(ref bool checkboxValue, ref FloatRange sliderValue, ref Vector2 pos, float width,
            string label, float minSliderValue, float maxSliderValue, float labelXOffset = 0)
        {
            // Set consistent checkbox and slider width
            float checkboxXOffset = width - 30;
            Widgets.Checkbox(new Vector2(checkboxXOffset, pos.y), ref checkboxValue);

            // Adjust label position with the offset and draw slider
            GUI.color = checkboxValue ? Color.white : Color.grey;
            sliderValue = SliderLabeled(new Rect(pos.x + labelXOffset, pos.y, width - 50 - labelXOffset, 24),
                label + ": " + sliderValue.ToStringPercent(), sliderValue, minSliderValue, maxSliderValue, checkboxValue);
            GUI.color = Color.white;

            // Move y position down for the next element
            pos.y += 24;
        }

        public float SliderLabeled(Rect rect, string label, float val, float min, float max, bool active)
        {
            // Ensure consistent label and slider width division
            Widgets.Label(rect.LeftPart(0.4f), label);
            float result = val;
            if (active)
            {
                result = Widgets.HorizontalSlider(rect.RightPart(0.6f), val, min, max, middleAlignment: true);
            }
            return result;
        }

        public override string SettingsCategory()
        {
            return "VAEI_ModSettingsName".Translate();
        }
    }

    public class VAEInsanityModSettings : ModSettings
    {
        public static bool selfHarmEnabled = false;
        public static SanityEffect twistedMeatValue = new SanityEffect(-0.01f);

        public static Dictionary<ThingDef, SanityEffect> suppressingEntities = new();
        public static Dictionary<ThingDef, SanityEffect> killingEntities = new();
        public static Dictionary<ThingDef, SanityEffect> studyingEntities = new();

        public static Dictionary<InteractionDef, SanityEffect> disturbingInitiatorEffects = new();
        public static Dictionary<InteractionDef, SanityEffect> interactionEffects = new();
        public static Dictionary<InteractionDef, SanityEffect> nonDisturbingInitiatorEffects = new();

        public static Dictionary<HediffDef, SanityEffect> hediffEffects = new();
        public static Dictionary<ThingDef, SanityEffect> usedThingsEffects = new();
        public static Dictionary<RitualOutcomeEffectDef, SanityEffect> ritualEffects = new();

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref selfHarmEnabled, "selfHarmEnabled");
            Scribe_Deep.Look(ref twistedMeatValue, "twistedMeatValue");

            Scribe_Collections.Look(ref suppressingEntities, "suppressingEntities", LookMode.Def, LookMode.Deep);
            Scribe_Collections.Look(ref killingEntities, "killingEntities", LookMode.Def, LookMode.Deep);
            Scribe_Collections.Look(ref studyingEntities, "studyingEntities", LookMode.Def, LookMode.Deep);

            Scribe_Collections.Look(ref disturbingInitiatorEffects, "disturbingInitiatorEffects", LookMode.Def, LookMode.Deep);
            Scribe_Collections.Look(ref interactionEffects, "interactionEffects", LookMode.Def, LookMode.Deep);
            Scribe_Collections.Look(ref nonDisturbingInitiatorEffects, "nonDisturbingInitiatorEffects", LookMode.Def, LookMode.Deep);

            Scribe_Collections.Look(ref hediffEffects, "hediffEffects", LookMode.Def, LookMode.Deep);
            Scribe_Collections.Look(ref usedThingsEffects, "usedThingsEffects", LookMode.Def, LookMode.Deep);
            Scribe_Collections.Look(ref ritualEffects, "ritualEffects", LookMode.Def, LookMode.Deep);

            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                suppressingEntities ??= new();
                killingEntities ??= new();
                studyingEntities ??= new();

                disturbingInitiatorEffects ??= new();
                interactionEffects ??= new();
                nonDisturbingInitiatorEffects ??= new();

                hediffEffects ??= new();
                usedThingsEffects ??= new();
                ritualEffects ??= new();

                if (twistedMeatValue is null)
                {
                    twistedMeatValue = new SanityEffect(-0.01f);
                }
            }
        }
    }


}
