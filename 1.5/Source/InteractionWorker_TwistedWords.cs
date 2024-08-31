using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VAEInsanity
{
    public class InteractionWorker_TwistedWords : InteractionWorker
    {
        public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
        {
            return 0f;
        }

        public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
        {
            base.Interacted(initiator, recipient, extraSentencePacks, out letterText, out letterLabel, out letterDef, out lookTargets);
            var sanity = recipient.needs.TryGetNeed<Need_Sanity>();
            if (sanity != null)
            {
                sanity.GainSanity(-0.025f, "VAEI_Interaction".Translate(DefsOf.VAEI_TwistedWords.label, initiator.Named("PAWN")));
            }
        }
    }
}
