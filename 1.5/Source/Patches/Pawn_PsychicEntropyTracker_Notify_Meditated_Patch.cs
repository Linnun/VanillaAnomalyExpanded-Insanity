using HarmonyLib;
using RimWorld;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(Pawn_PsychicEntropyTracker), "Notify_Meditated")]
    public static class Pawn_PsychicEntropyTracker_Notify_Meditated_Patch
    {
        public static void Postfix(Pawn_PsychicEntropyTracker __instance)
        {
            __instance.pawn.SanityGainContinuously(0.02f / GenDate.TicksPerDay, "VAEI_Meditating".Translate());
        }
    }
}
