using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VAEInsanity
{
    public class InteractionWorker_UnsettlingTalk : InteractionWorker
    {
        public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
        {
            return 0f;
        }

        public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
        {
            base.Interacted(initiator, recipient, extraSentencePacks, out letterText, out letterLabel, out letterDef, out lookTargets);
            var recipientSanity = recipient.needs.TryGetNeed<Need_Sanity>();
            if (recipientSanity != null)
            {
                recipientSanity.GainSanity(-0.03f, "VAEI_Interaction".Translate(DefsOf.VAEI_UnsettlingTalk.label, initiator.Named("PAWN")));
            }
        }
    }
}
