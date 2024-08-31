using HarmonyLib;
using RimWorld;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(CompUsable), "UsedBy")]
    public static class CompUsable_UsedBy_Patch
    {
        public static void Postfix(CompUsable __instance, Pawn p)
        {
            foreach (var def in DefDatabase<SanityEffectsDef>.AllDefs)
            {
                if (def.usedThingsEffects != null)
                {
                    foreach (var thingEffect in def.usedThingsEffects)
                    {
                        if (__instance.parent.def == thingEffect.thing)
                        {
                            p.SanityGain(thingEffect, "VAEI_UsingThing".Translate(thingEffect.thing.label));
                        }
                    }
                }
            }
        }
    }
}
