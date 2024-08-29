﻿using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(JobDriver_ActivitySuppression), "TrySuppress")]
    public static class JobDriver_ActivitySuppression_TrySuppress_Patch
    {
        public static void Postfix(ref Toil __result, JobDriver_ActivitySuppression __instance)
        {
            __result.AddPreTickAction(delegate
            {
                __instance.pawn.SanityGainContinuously(-0.01f / GenDate.TicksPerDay, "VAEI_SuppressingEntities".Translate(__instance.ThingToSuppress.Label));
            });
        }
    }
}
