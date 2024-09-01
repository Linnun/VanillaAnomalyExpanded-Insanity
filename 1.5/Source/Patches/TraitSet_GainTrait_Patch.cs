using HarmonyLib;
using RimWorld;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(TraitSet), "GainTrait")]
    public static class TraitSet_GainTrait_Patch
    {
        public static void Postfix(Pawn ___pawn, Trait trait)
        {
            Log.Message("trait: " + trait.def);
            if (trait.def == DefsOf.VAEI_Distractable)
            {
                var def = Rand.Bool ? DefsOf.VAEI_Focused : DefsOf.VAEI_Distracted;
                Hediff hediff = HediffMaker.MakeHediff(def, ___pawn);
                ___pawn.health.AddHediff(hediff);
                Log.Message("Added hediff: " + hediff + " - " +
                    hediff.TryGetComp<HediffComp_CycleFocusDistracted>()
                    + " - " + ___pawn.health.hediffSet.GetFirstHediffOfDef(def));
            }
        }
    }
}
