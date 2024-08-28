using HarmonyLib;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(Hediff), "PostRemoved")]
    public static class Hediff_PostRemoved_Patch
    {
        public static void Postfix(Hediff __instance)
        {
            if (__instance.pawn.RaceProps.Humanlike && 
                __instance.pawn.health.hediffSet.hediffs.Any((Hediff a) => a.def == __instance.def) is false)
            {
                var trait = __instance.pawn.story.traits.GetTrait(DefsOf.VAEI_Inhumanized);
                if (trait != null)
                {
                    __instance.pawn.story.traits.RemoveTrait(trait);
                }
            }
        }
    }
}
