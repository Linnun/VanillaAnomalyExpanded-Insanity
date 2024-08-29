using HarmonyLib;
using RimWorld;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(CompStudiable), "Study")]
    public static class CompStudiable_Study_Patch
    {
        public static void Postfix(CompStudiable __instance, Pawn studier, float anomalyKnowledgeAmount)
        {
            float sanityChange = anomalyKnowledgeAmount * -0.01f;
            studier.SanityGain(sanityChange, "VEAI_AnomalyStudy".Translate(__instance.parent.Label));
        }
    }
}
