using HarmonyLib;
using RimWorld;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(Thing), "IngestedCalculateAmounts")]
    public static class Thing_IngestedCalculateAmounts_Patch
    {
        public static void Postfix(Thing __instance, Pawn ingester, ref int numTaken)
        {
            if (__instance.def == ThingDefOf.Meat_Twisted)
            {
                float sanityChange = -(numTaken / 80f);
                ingester.SanityGain(sanityChange, "VEAI_ConsumedTwistedMeat".Translate());
            }
            else if (__instance.TryGetComp<CompIngredients>() is CompIngredients compIngredients && compIngredients.ingredients.Contains(ThingDefOf.Meat_Twisted))
            {
                ingester.SanityGain(-0.01f, "VEAI_ConsumedTwistedMeatAsIngredient".Translate());
            }
        }
    }
}
