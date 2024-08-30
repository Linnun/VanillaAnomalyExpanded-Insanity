using HarmonyLib;
using RimWorld;
using RimWorld.Utility;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(VoidAwakeningUtility), "DisruptTheLink")]
    public static class VoidAwakeningUtility_DisruptTheLink_Patch
    {
        public static void Postfix(Pawn pawn)
        {
            foreach (var pawn2 in PawnsFinder.AllMaps_SpawnedPawnsInFaction(pawn.Faction))
            {
                var need = pawn2.needs?.TryGetNeed<Need_Sanity>();
                if (need != null)
                {
                    need.GainSanity(1f, "VAEI_VoidClosing".Translate());
                }
            }
        }
    }
}
