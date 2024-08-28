using HarmonyLib;
using RimWorld;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(Hediff), "PostAdd")]
    public static class Hediff_PostAdd_Patch
    {
        public static void Postfix(Hediff __instance, DamageInfo? dinfo)
        {
            if (__instance.def == HediffDefOf.Inhumanized && __instance.pawn.RaceProps.Humanlike
                && __instance.pawn.story.traits.HasTrait(DefsOf.VAEI_Inhumanized) is false)
            {
                __instance.pawn.story.traits.GainTrait(new Trait(DefsOf.VAEI_Inhumanized));
            }
        }
    }
}
