using HarmonyLib;
using RimWorld;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(PitGate), "BeginCollapsing")]
    public static class PitGate_BeginCollapsing_Patch
    {
        public static void Postfix(PitGate __instance)
        {
            foreach (var pawn in __instance.Map.mapPawns.AllHumanlike)
            {
                var need = pawn.needs?.TryGetNeed<Need_Sanity>();
                if (need != null)
                {
                    need.GainSanity(0.05f, "VAEI_PitgateCollapsed".Translate());
                }
            }
        }
    }
}
