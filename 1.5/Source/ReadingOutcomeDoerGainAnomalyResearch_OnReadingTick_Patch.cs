using HarmonyLib;
using RimWorld;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(ReadingOutcomeDoerGainAnomalyResearch), "OnReadingTick")]
    public static class ReadingOutcomeDoerGainAnomalyResearch_OnReadingTick_Patch
    {
        public static void Postfix(Pawn reader)
        {
            reader.SanityGainContinuously(-0.035f / GenDate.TicksPerDay, "VAEI_ReadingTome".Translate());
        }
    }
}
