using HarmonyLib;
using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(RitualOutcomeEffectWorker_FromQuality), "GetQuality")]
    public static class RitualOutcomeEffectWorker_FromQuality_GetQuality_Patch
    {
        public static void Postfix(float __result, RitualOutcomeEffectWorker_FromQuality __instance, LordJob_Ritual jobRitual, float progress)
        {
            if (progress > 0)
            {
                foreach (var def in DefDatabase<SanityEffectsDef>.AllDefs)
                {
                    if (def.ritualEffects != null)
                    {
                        foreach (var effect in def.ritualEffects)
                        {
                            if (effect.ritual == __instance.def)
                            {
                                foreach (var pawn in jobRitual.totalPresenceTmp.Keys)
                                {
                                    var gain = Mathf.Lerp(effect.effect.min, effect.effect.max, __result);
                                    pawn.SanityGain(gain, "VAEI_Attending".Translate(jobRitual.Ritual.name));
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(Pawn_PsychicEntropyTracker), "Notify_Meditated")]
    public static class Pawn_PsychicEntropyTracker_Notify_Meditated_Patch
    {
        public static void Postfix(Pawn_PsychicEntropyTracker __instance)
        {
            __instance.pawn.SanityGainContinuously(0.02f / GenDate.TicksPerDay, "VAEI_Meditating".Translate());
        }
    }
}
