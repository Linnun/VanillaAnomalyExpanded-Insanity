using HarmonyLib;
using RimWorld;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(Pawn), "Kill")]
    public static class Pawn_Kill_Patch
    {
        public static void Postfix(Pawn __instance, DamageInfo? dinfo, Hediff exactCulprit = null)
        {
            if (dinfo.HasValue && dinfo.Value.Instigator is Pawn instigator)
            {
                if (VAEInsanityModSettings.killingEntities.TryGetValue(__instance.def, out var option) && option.enabled)
                {

                }
                if (__instance.IsShambler)
                {
                    var need = instigator.needs?.TryGetNeed<Need_Sanity>();
                    if (need != null && need.killedShamblers.Contains(__instance) is false)
                    {
                        need.killedShamblers.Add(__instance);
                        need.GainSanity(0.01f, "VAEI_KillingThing".Translate(MutantDefOf.Shambler.label));
                    }
                }
                else if (__instance.kindDef == PawnKindDefOf.Nociosphere && instigator.Faction is not null)
                {
                    foreach (var pawn in PawnsFinder.AllMaps_SpawnedPawnsInFaction(instigator.Faction))
                    {
                        var need = pawn.needs?.TryGetNeed<Need_Sanity>();
                        if (need != null)
                        {
                            need.GainSanity(0.05f, "VAEI_WeKilledNociosphere".Translate());
                        }
                    }
                }
                else if (VAEInsanityModSettings.killingEntities.TryGetEffect(__instance.def, out var effect))
                {
                    instigator.SanityGain(effect, "VAEI_KillingThing".Translate(__instance.def.label));
                }
            }
        }
    }
}
