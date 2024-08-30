using HarmonyLib;
using LudeonTK;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VAEInsanity
{
    [StaticConstructorOnStartup]
    public static class Utils
    {
        static Utils()
        {
            new Harmony("VAEInsanityMod").PatchAll();
        }

        [DebugAction("Pawns", "Sanity +10%", false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresAnomaly = true, displayPriority = -1000)]
        private static void SanityPlus10(Pawn p)
        {
            var need = p.needs.TryGetNeed<Need_Sanity>();
            need.GainSanity(0.1f, "using DEV: Sanity +10%");
            DebugActionsUtility.DustPuffFrom(p);
        }

        [DebugAction("Pawns", "Sanity -10%", false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresAnomaly = true, displayPriority = -1000)]
        private static void SanityMinus10(Pawn p)
        {
            var need = p.needs.TryGetNeed<Need_Sanity>();
            need.GainSanity(-0.1f, "using DEV: Sanity -10%");
            DebugActionsUtility.DustPuffFrom(p);
        }

        public static void SanityGain(this Pawn pawn, SanityEffectBase effect, string reason)
        {
            var need = pawn?.needs?.TryGetNeed<Need_Sanity>();
            if (need != null)
            {
                if (effect.description.NullOrEmpty() is false)
                {
                    reason = effect.description;
                }
                need.GainSanity(effect.effect.RandomInRange, reason);
            }
        }

        public static void SanityGain(this Pawn pawn, float sanityGain, string reason)
        {
            var need = pawn?.needs?.TryGetNeed<Need_Sanity>();
            if (need != null)
            {
                need.GainSanity(sanityGain, reason);
            }
        }

        public static void SanityGainContinuously(this Pawn pawn, float sanityGain, string reason)
        {
            var need = pawn?.needs?.TryGetNeed<Need_Sanity>();
            if (need != null)
            {
                var lastRecord = need.records.Last();
                if (lastRecord.reason == reason)
                {
                    lastRecord.UpdateRecord(sanityGain);
                }
                else
                {
                    need.GainSanity(sanityGain, reason, doMessage: false);
                }
            }
        }
    }
}
