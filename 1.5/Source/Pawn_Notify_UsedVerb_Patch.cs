using HarmonyLib;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(Pawn), "Notify_UsedVerb")]
    public static class Pawn_Notify_UsedVerb_Patch
    {
        public static void Postfix(Pawn pawn, Verb verb)
        {
            foreach (var def in DefDatabase<SanityEffectsDef>.AllDefs)
            {
                if (def.usedThingsEffects != null)
                {
                    foreach (var thingEffect in def.usedThingsEffects)
                    {
                        if (verb.EquipmentSource?.def == thingEffect.thing)
                        {
                            pawn.SanityGain(thingEffect, "VAEI_UsingThing".Translate(thingEffect.thing.label));
                        }
                    }
                }
            }
        }
    }
}
