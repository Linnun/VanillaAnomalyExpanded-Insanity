using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VAEInsanity
{
    public class InteractionWorker_VoidSelfHarm : InteractionWorker
    {
        public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
        {
            return 0f;
        }

        public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
        {
            base.Interacted(initiator, recipient, extraSentencePacks, out letterText, out letterLabel, out letterDef, out lookTargets);
            DoSelfHarm(initiator);
        }

        public static void DoSelfHarm(Pawn initiator)
        {
            var outsideParts = initiator.health.hediffSet.GetNotMissingParts()
                .Where(x => x.depth == BodyPartDepth.Outside).ToList();
            var parts = outsideParts.Where(x => HasParent(x, BodyPartDefOf.Shoulder)).ToList();
            parts.AddRange(outsideParts.Where(x => x.def == BodyPartDefOf.Leg || HasParent(x, BodyPartDefOf.Leg)));
            if (parts.Any())
            {
                var cuts = Rand.Range(1, 6);
                for (var i = 0; i < cuts; i++)
                {
                    var part = parts.RandomElement();
                    initiator.TakeDamage(new DamageInfo(DamageDefOf.Cut, 1, 1, instigator: initiator, hitPart: part));
                }
                initiator.needs.mood.thoughts.memories.TryGainMemory(DefsOf.VAEI_VoidHarmed);
            }
        }

        private static bool HasParent(BodyPartRecord record, BodyPartDef parent)
        {
            if (record.parent.def == parent)
            {
                return true;
            }
            if (record.parent != null)
            {
                return HasParent(record.parent, parent);
            }
            return false;
        }
    }
}
