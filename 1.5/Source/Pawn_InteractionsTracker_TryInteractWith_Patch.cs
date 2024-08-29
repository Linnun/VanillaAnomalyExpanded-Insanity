using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(Pawn_InteractionsTracker), "TryInteractWith")]
    public static class Pawn_InteractionsTracker_TryInteractWith_Patch
    {
        public static void Postfix(Pawn_InteractionsTracker __instance, bool __result, Pawn recipient,
            InteractionDef intDef)
        {
            if (intDef == InteractionDefOf.OccultTeaching)
            {
                recipient.SanityGain(-0.005f, "VAEI_OccultTeaching".Translate(__instance.pawn.Named("PAWN")));
            }
            foreach (var def in DefDatabase<SanityEffectsDef>.AllDefs)
            {
                if (def.interactionEffects != null)
                {
                    foreach (var effect in def.interactionEffects)
                    {
                        if (effect.interaction == intDef)
                        {
                            recipient.SanityGain(effect, "VAEI_DisturbingInteraction".Translate(intDef.label, __instance.pawn.Named("PAWN")));
                        }
                    }
                }
            }
            if (__instance.pawn.story.IsDisturbing)
            {
                foreach (var def in DefDatabase<SanityEffectsDef>.AllDefs)
                {
                    if (def.disturbingInitiatorEffects != null)
                    {
                        foreach (var effect in def.disturbingInitiatorEffects)
                        {
                            if (effect.interaction == intDef)
                            {
                                recipient.SanityGain(effect, "VAEI_DisturbingInteraction".Translate(intDef.label, __instance.pawn.Named("PAWN")));
                            }
                        }
                    }
                }
            }
        }
    }
}
