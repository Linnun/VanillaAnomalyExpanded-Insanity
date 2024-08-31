using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI.Group;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(LordToil_PsychicRitual), "RitualCompleted")]
    public static class LordToil_PsychicRitual_RitualCompleted_Patch
    {
        public static void Prefix(LordToil_PsychicRitual __instance)
        {
            foreach (var def in DefDatabase<SanityEffectsDef>.AllDefs)
            {
                if (def.psychicRitualEffects != null)
                {
                    foreach (var effectDef in def.psychicRitualEffects)
                    {
                        if (__instance.def == effectDef.ritual)
                        {
                            if (effectDef.invokerEffect != 0)
                            {
                                var invoker = __instance.RitualData.psychicRitual.assignments.FirstAssignedPawn(effectDef.ritual.InvokerRole);
                                invoker.SanityGain(effectDef.invokerEffect, "VAEI_InvokerEffect".Translate(__instance.RitualData.psychicRitual.def.label));
                            }
                            if (effectDef.targetEffect != 0)
                            {
                                var target = __instance.RitualData.psychicRitual.assignments.FirstAssignedPawn(effectDef.ritual.TargetRole);
                                target.SanityGain(effectDef.invokerEffect, "VAEI_TargetEffect".Translate(__instance.RitualData.psychicRitual.def.label));
                            }
                            if (effectDef.chanterEffect != 0)
                            {
                                var chanter = __instance.RitualData.psychicRitual.assignments.FirstAssignedPawn(effectDef.ritual.ChanterRole);
                                chanter.SanityGain(effectDef.invokerEffect, "VAEI_ChanterEffect".Translate(__instance.RitualData.psychicRitual.def.label));
                            }
                        }
                    }
                }
            }
        }
    }
}
