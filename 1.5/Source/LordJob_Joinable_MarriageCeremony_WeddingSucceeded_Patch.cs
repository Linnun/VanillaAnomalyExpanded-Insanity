using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(LordJob_Joinable_MarriageCeremony), "WeddingSucceeded")]
    public static class LordJob_Joinable_MarriageCeremony_WeddingSucceeded_Patch
    {
        public static void Prefix(LordJob_Joinable_MarriageCeremony __instance)
        {
            List<Pawn> ownedPawns = __instance.lord.ownedPawns;
            for (int i = 0; i < ownedPawns.Count; i++)
            {
                Pawn pawn = ownedPawns[i];
                var need = pawn.needs.TryGetNeed<Need_Sanity>();
                if (need != null)
                {
                    need.GainSanity(0.05f, "VAEI_Attending".Translate(GatheringDefOf.MarriageCeremony.label));
                }
            }
        }
    }
}
