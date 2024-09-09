using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;
using static Verse.Widgets;
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
            BeginScrollView(inRect, ref scrollPosition, viewRect);

            Vector2 pos = new Vector2(inRect.x, inRect.y); // Initialize position

            // Consistent label offset for list items
            float labelXOffsetForListItems = 10;

            // Draw the checkbox for self-harm content
            CheckboxLabeled(new Rect(pos.x, pos.y, widthWithoutScrollBar - 5, 24), "VAEI_EnableSelfHarmContent".Translate(), ref VAEInsanityModSettings.selfHarmEnabled);
            pos.y += 24;

            // Draw the label for sanity loss effects
            Label(new Rect(pos.x, pos.y, widthWithoutScrollBar, 24), "VAEI_SanityLossEffects".Translate());
            pos.y += 24;

            DrawSection(ref pos, widthWithoutScrollBar, labelXOffsetForListItems, "VAEI_General".Translate(),
                (ref Vector2 position, float width, float labelOffset) =>
                {
                    DrawCheckboxAndSlider(ref VAEInsanityModSettings.twistedMeatValue.enabled,
                        ref VAEInsanityModSettings.twistedMeatValue.sanityValue,
                        ref position, width, "VEAI_EatingTwistedMeat".Translate(), -0.1f, -0.001f,
                        labelOffset);

                    DrawCheckboxAndSlider(ref VAEInsanityModSettings.marriageCeremonyValue.enabled,
                        ref VAEInsanityModSettings.marriageCeremonyValue.sanityValue,
                        ref position, width, "VAEI_MarriageCeremony".Translate(), 0f, 0.1f,
                        labelOffset);

                    DrawCheckboxAndSlider(ref VAEInsanityModSettings.partyValue.enabled,
                        ref VAEInsanityModSettings.partyValue.sanityValue,
                        ref position, width, "VAEI_Party".Translate(), 0f, 0.1f,
                        labelOffset);
                }, 3);

            DrawList(ref pos, widthWithoutScrollBar, "VAEI_InteractionEffects".Translate(), VAEInsanityModSettings.interactionEffects, labelXOffsetForListItems, -0.1f, 0.1f);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_NonDisturbingInitiatorEffects".Translate(), VAEInsanityModSettings.nonDisturbingInitiatorEffects, labelXOffsetForListItems, 0f, 0.1f);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_DisturbingInitiatorEffects".Translate(), VAEInsanityModSettings.disturbingInitiatorEffects, labelXOffsetForListItems, -0.1f, -0.001f);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_HediffEffects".Translate(), VAEInsanityModSettings.hediffEffects, labelXOffsetForListItems, -0.1f, -0.001f);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_UsedThingsEffects".Translate(), VAEInsanityModSettings.usedThingsEffects, labelXOffsetForListItems, -0.1f, -0.001f);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_RitualEffects".Translate(), VAEInsanityModSettings.ritualEffects, labelXOffsetForListItems, 0f, 0.1f);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_InvokerEffects".Translate(), VAEInsanityModSettings.invokerEffects, labelXOffsetForListItems, -0.1f, -0.001f);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_TargetEffects".Translate(), VAEInsanityModSettings.targetEffects, labelXOffsetForListItems, -0.1f, -0.001f);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_ChanterEffects".Translate(), VAEInsanityModSettings.chanterEffects, labelXOffsetForListItems, -0.1f, -0.001f);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_SuppressingEntities".Translate(), VAEInsanityModSettings.suppressingEntities, labelXOffsetForListItems, -0.1f, -0.001f);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_StudyingEntities".Translate(), VAEInsanityModSettings.studyingEntities, labelXOffsetForListItems, -0.1f, -0.001f);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_KillingEntities".Translate(),
                VAEInsanityModSettings.killingEntities, labelXOffsetForListItems, 0f, 0.1f, listHeightOffset: 24 * 2);
            pos.y -= 10;
            DrawCheckboxAndSlider(ref VAEInsanityModSettings.killingShamblerValue.enabled,
                ref VAEInsanityModSettings.killingShamblerValue.sanityValue,
                ref pos, widthWithoutScrollBar, "VAEI_KillingShambler".Translate(), 0f, 0.1f, labelXOffsetForListItems);

            DrawCheckboxAndSlider(ref VAEInsanityModSettings.killingNociosphereValue.enabled,
                ref VAEInsanityModSettings.killingNociosphereValue.sanityValue,
                ref pos, widthWithoutScrollBar, "VAEI_KillingNociosphere".Translate(), 0f, 0.1f, labelXOffsetForListItems);
            pos.y += 10;


            sectionScrollHeight = pos.y - inRect.y;
            EndScrollView();
        }

        private delegate void DrawSettingsDelegate(ref Vector2 pos, float widthWithoutScrollBar, float labelXOffsetForListItems);

        private void DrawSection(ref Vector2 pos, float widthWithoutScrollBar, float labelXOffsetForListItems, 
            string sectionLabel, DrawSettingsDelegate drawSettingsFunc, int elementsCount)
        {
            float elementHeight = 24f; // Height of each element
            float sectionHeight = elementsCount * elementHeight + 10; // Dynamically calculate height

            // Draw the section label
            Label(new Rect(pos.x, pos.y, widthWithoutScrollBar, 24), sectionLabel);
            pos.y += 24;

            // Draw the section for sliders or other UI elements
            Rect sectionRect = new Rect(pos.x, pos.y, widthWithoutScrollBar, sectionHeight);
            DrawMenuSection(sectionRect);
            pos.y += 5;

            // Call the passed-in function to draw the settings for this section
            drawSettingsFunc(ref pos, widthWithoutScrollBar, labelXOffsetForListItems);

            pos.y += 10; // Space after the section
        }

        // Updated DrawList to accept min and max values for the sliders
        private void DrawList<T>(ref Vector2 pos, float width, string sectionLabel, Dictionary<T, SanityEffect> list,
            float labelXOffset, float minSliderValue, float maxSliderValue, float listHeightOffset = 0) where T : Def
        {
            // Draw section label
            Label(new Rect(pos.x, pos.y, width, 24), sectionLabel);
            pos.y += 24;

            // Calculate the height of the list section based on the number of items
            float listHeight = (list.Count * 24 + 10) + listHeightOffset;

            // Define the menu section rect and draw it
            Rect listRect = new Rect(pos.x, pos.y, width, listHeight);
            DrawMenuSection(listRect);
            pos.y += 5;

            // Draw each item in the list
            foreach (var kvp in list.ToList())
            {
                var enabled = kvp.Value.enabled;
                var value = kvp.Value.sanityValue;
                var defLabel = (string)kvp.Key.LabelCap;
                if (kvp.Key is RitualOutcomeEffectDef ritualDef)
                {
                    var behaviour = DefDatabase<RitualPatternDef>.AllDefs.FirstOrDefault(x => x.ritualOutcomeEffect == ritualDef
                    && x.shortDescOverride.NullOrEmpty() is false);
                    if (behaviour != null)
                    {
                        defLabel = behaviour.shortDescOverride.CapitalizeFirst();
                    }
                    else
                    {
                        defLabel = ritualDef.defName;
                    }
                }

                // Draw based on isSingleSlider flag
                if (kvp.Value.isSingleSlider)
                {
                    // Draw checkbox and slider for single value with provided min/max values
                    DrawCheckboxAndSlider(ref enabled, ref value, ref pos, width, defLabel, minSliderValue, maxSliderValue, labelXOffset);
                }
                else
                {
                    // Use FloatRange for other cases with provided min/max values
                    DrawCheckboxAndFloatRange(ref enabled, ref value, ref pos, width, defLabel, minSliderValue, maxSliderValue, labelXOffset);
                }

                // Update the dictionary with new values
                list[kvp.Key] = new SanityEffect
                {
                    enabled = enabled,
                    sanityValue = value,
                    isSingleSlider = kvp.Value.isSingleSlider
                };
            }

            pos.y += 10;
        }

        public void DrawCheckboxAndFloatRange(ref bool checkboxValue, ref FloatRange rangeValue, ref Vector2 pos, float width,
            string label, float minRangeValue, float maxRangeValue, float labelXOffset = 0)
        {
            // Draw the checkbox
            float checkboxXOffset = width - 30;
            Checkbox(new Vector2(checkboxXOffset, pos.y), ref checkboxValue);

            // Set consistent label width and slider width
            float labelWidth = 250;  // Adjust based on layout needs
            float sliderWidth = width - labelWidth - 50 - labelXOffset - 5;

            // Adjust label position and draw the FloatRange slider next to the label
            GUI.color = checkboxValue ? Color.white : Color.grey;
            Label(new Rect(pos.x + labelXOffset, pos.y, labelWidth, 24),
                          label + ": " + rangeValue.min.ToStringPercent() + " - " + rangeValue.max.ToStringPercent());

            FloatRange(new Rect(pos.x + labelXOffset + labelWidth + 5, pos.y - 5, sliderWidth, 24),
                (int)pos.y, ref rangeValue, minRangeValue, maxRangeValue, null);

            GUI.color = Color.white;

            pos.y += 24; // Increment y position for the next item, keeping the label and slider on the same line
        }

        public void DrawCheckboxAndSlider(ref bool checkboxValue, ref FloatRange sliderValue, ref Vector2 pos, float width,
            string label, float minSliderValue, float maxSliderValue, float labelXOffset = 0)
        {
            // Draw the checkbox
            float checkboxXOffset = width - 30;
            Checkbox(new Vector2(checkboxXOffset, pos.y), ref checkboxValue);

            // Set consistent label width and slider width
            float labelWidth = 250;  // Adjust based on layout needs
            float sliderWidth = width - labelWidth - 50 - labelXOffset;

            // Adjust label position with the offset and draw the slider next to the label
            GUI.color = checkboxValue ? Color.white : Color.grey;
            Label(new Rect(pos.x + labelXOffset, pos.y, labelWidth, 24),
                          label + ": " + sliderValue.max.ToStringPercent());

            var value = HorizontalSlider(new Rect(pos.x + labelXOffset + labelWidth, pos.y, sliderWidth, 24),
                sliderValue.max, minSliderValue, maxSliderValue, middleAlignment: true);
            sliderValue = new FloatRange(value);
            GUI.color = Color.white;

            pos.y += 24; // Increment y position for the next item, keeping the label and slider on the same line
        }

        public static void FloatRange(Rect rect, int id, ref FloatRange range, float min = 0f, float max = 1f, string labelKey = null, ToStringStyle valueStyle = ToStringStyle.FloatTwo, float gap = 0f, GameFont sliderLabelFont = GameFont.Small, Color? sliderLabelColor = null)
        {
            Rect rect2 = rect;
            rect2.xMin += 8f;
            rect2.xMax -= 8f;
            GUI.color = sliderLabelColor ?? RangeControlTextColor;
            string text = range.min.ToStringByStyle(valueStyle) + " - " + range.max.ToStringByStyle(valueStyle);
            if (labelKey != null)
            {
                text = labelKey.Translate(text);
            }
            GameFont font = Text.Font;
            Text.Font = sliderLabelFont;
            Text.Anchor = TextAnchor.UpperLeft;
            Rect position = new Rect(rect2.x, rect2.yMax - 8f - 1f, rect2.width, 2f);
            GUI.DrawTexture(position, BaseContent.WhiteTex);
            float num = rect2.x + rect2.width * Mathf.InverseLerp(min, max, range.min);
            float num2 = rect2.x + rect2.width * Mathf.InverseLerp(min, max, range.max);
            GUI.color = Color.white;
            GUI.DrawTexture(new Rect(num, rect2.yMax - 8f - 2f, num2 - num, 4f), BaseContent.WhiteTex);
            float num3 = num;
            float num4 = num2;
            Rect position2 = new Rect(num3 - 16f, position.center.y - 8f, 16f, 16f);
            GUI.DrawTexture(position2, FloatRangeSliderTex);
            Rect position3 = new Rect(num4 + 16f, position.center.y - 8f, -16f, 16f);
            GUI.DrawTexture(position3, FloatRangeSliderTex);
            if (curDragEnd != 0 && (Event.current.type == EventType.MouseUp || Event.current.rawType == EventType.MouseDown))
            {
                draggingId = 0;
                curDragEnd = RangeEnd.None;
                SoundDefOf.DragSlider.PlayOneShotOnCamera();
                Event.current.Use();
            }
            bool flag = false;
            if (Mouse.IsOver(rect) || draggingId == id)
            {
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && id != draggingId)
                {
                    draggingId = id;
                    float x = Event.current.mousePosition.x;
                    if (x < position2.xMax)
                    {
                        curDragEnd = RangeEnd.Min;
                    }
                    else if (x > position3.xMin)
                    {
                        curDragEnd = RangeEnd.Max;
                    }
                    else
                    {
                        float num5 = Mathf.Abs(x - position2.xMax);
                        float num6 = Mathf.Abs(x - (position3.x - 16f));
                        curDragEnd = ((num5 < num6) ? RangeEnd.Min : RangeEnd.Max);
                    }
                    flag = true;
                    Event.current.Use();
                    SoundDefOf.DragSlider.PlayOneShotOnCamera();
                }
                if (flag || (curDragEnd != 0 && UnityGUIBugsFixer.MouseDrag()))
                {
                    float value = (Event.current.mousePosition.x - rect2.x) / rect2.width * (max - min) + min;
                    value = Mathf.Clamp(value, min, max);
                    if (curDragEnd == RangeEnd.Min)
                    {
                        if (value != range.min)
                        {
                            range.min = Mathf.Min(value, max - gap);
                            if (range.max < range.min + gap)
                            {
                                range.max = range.min + gap;
                            }
                            CheckPlayDragSliderSound();
                        }
                    }
                    else if (curDragEnd == RangeEnd.Max && value != range.max)
                    {
                        range.max = Mathf.Max(value, min + gap);
                        if (range.min > range.max - gap)
                        {
                            range.min = range.max - gap;
                        }
                        CheckPlayDragSliderSound();
                    }
                    if (Event.current.type == EventType.MouseDrag)
                    {
                        Event.current.Use();
                    }
                }
            }
            Text.Font = font;
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
        public static SanityEffect marriageCeremonyValue = new SanityEffect(0.05f);
        public static SanityEffect partyValue = new SanityEffect(0.02f);
        public static SanityEffect killingShamblerValue = new SanityEffect(0.01f);
        public static SanityEffect killingNociosphereValue = new SanityEffect(0.05f);
        public static SanityEffect meditatingValue = new SanityEffect(0.02f);

        public static Dictionary<ThingDef, SanityEffect> suppressingEntities = new();
        public static Dictionary<ThingDef, SanityEffect> killingEntities = new();
        public static Dictionary<ThingDef, SanityEffect> studyingEntities = new();

        public static Dictionary<InteractionDef, SanityEffect> disturbingInitiatorEffects = new();
        public static Dictionary<InteractionDef, SanityEffect> interactionEffects = new();
        public static Dictionary<InteractionDef, SanityEffect> nonDisturbingInitiatorEffects = new();

        public static Dictionary<HediffDef, SanityEffect> hediffEffects = new();
        public static Dictionary<ThingDef, SanityEffect> usedThingsEffects = new();
        public static Dictionary<RitualOutcomeEffectDef, SanityEffect> ritualEffects = new();

        public static Dictionary<PsychicRitualDef_InvocationCircle, SanityEffect> invokerEffects = new();
        public static Dictionary<PsychicRitualDef_InvocationCircle, SanityEffect> targetEffects = new();
        public static Dictionary<PsychicRitualDef_InvocationCircle, SanityEffect> chanterEffects = new();

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref selfHarmEnabled, "selfHarmEnabled");
            Scribe_Deep.Look(ref twistedMeatValue, "twistedMeatValue");
            Scribe_Deep.Look(ref marriageCeremonyValue, "marriageCeremonyValue");
            Scribe_Deep.Look(ref partyValue, "partyValue");

            Scribe_Collections.Look(ref suppressingEntities, "suppressingEntities", LookMode.Def, LookMode.Deep);
            Scribe_Collections.Look(ref killingEntities, "killingEntities", LookMode.Def, LookMode.Deep);
            Scribe_Collections.Look(ref studyingEntities, "studyingEntities", LookMode.Def, LookMode.Deep);

            Scribe_Collections.Look(ref disturbingInitiatorEffects, "disturbingInitiatorEffects", LookMode.Def, LookMode.Deep);
            Scribe_Collections.Look(ref interactionEffects, "interactionEffects", LookMode.Def, LookMode.Deep);
            Scribe_Collections.Look(ref nonDisturbingInitiatorEffects, "nonDisturbingInitiatorEffects", LookMode.Def, LookMode.Deep);

            Scribe_Collections.Look(ref hediffEffects, "hediffEffects", LookMode.Def, LookMode.Deep);
            Scribe_Collections.Look(ref usedThingsEffects, "usedThingsEffects", LookMode.Def, LookMode.Deep);
            Scribe_Collections.Look(ref ritualEffects, "ritualEffects", LookMode.Def, LookMode.Deep);

            Scribe_Collections.Look(ref invokerEffects, "invokerEffects", LookMode.Def, LookMode.Deep);
            Scribe_Collections.Look(ref targetEffects, "targetEffects", LookMode.Def, LookMode.Deep);
            Scribe_Collections.Look(ref chanterEffects, "chanterEffects", LookMode.Def, LookMode.Deep);

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

                invokerEffects ??= new();
                targetEffects ??= new();
                chanterEffects ??= new();
                twistedMeatValue ??= new SanityEffect(-0.01f);
                marriageCeremonyValue ??= new SanityEffect(0.05f);
                partyValue ??= new SanityEffect(0.02f);
                killingShamblerValue ??= new SanityEffect(0.01f);
                killingNociosphereValue ??= new SanityEffect(0.05f);
                meditatingValue ??= new SanityEffect(0.02f);

            }
        }
    }

}
