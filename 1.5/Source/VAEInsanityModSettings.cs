using RimWorld;
using System.Collections.Generic;
using Verse;
namespace VAEInsanity
{
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
            Scribe_Deep.Look(ref meditatingValue, "meditatingValue");

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
