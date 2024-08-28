using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VAEInsanity
{
    [HarmonyPatch(typeof(Pawn_InteractionsTracker), "TryInteractWith")]
    public static class Pawn_InteractionsTracker_TryInteractWith_Patch
    {
        public static Dictionary<string, float> sanityEffects = new Dictionary<string, float>
        {
            { "Chitchat", -0.01f },
            { "DeepTalk", -0.02f },
            { "KindWords", -0.03f },
            { "Nuzzle", -0.02f },
            { "RomanceAttempt", -0.05f },
            { "MarriageProposal", -0.05f },
            { "BabyPlay", -0.04f },
            { "Counsel_Success", -0.05f },
            { "Reassure", -0.05f },
            { "VSIE_Vent", -0.03f },
            { "VFEE_RoyalGossip", -0.01f }
        };
        public static void Postfix(Pawn_InteractionsTracker __instance, bool __result, Pawn recipient,
            InteractionDef intDef)
        {
            if (intDef == InteractionDefOf.OccultTeaching)
            {
                recipient.SanityGain(-0.005f, "VAEI_OccultTeaching".Translate(__instance.pawn.Named("PAWN")));
            }
            else if (__instance.pawn.story.IsDisturbing && sanityEffects.TryGetValue(intDef.defName, out var effect))
            {
                recipient.SanityGain(effect, "VAEI_DisturbingInteraction".Translate(intDef.label, __instance.pawn.Named("PAWN")));
            }
        }
    }
}
