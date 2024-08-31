using RimWorld;
using Verse;

namespace VAEInsanity
{
    public class InteractionWorker_SanityInsult : InteractionWorker
    {
        public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
        {
            var sanity = initiator.needs.TryGetNeed<Need_Sanity>();
            if (sanity != null)
            {
                if (sanity.CurLevel <= 0.5f)
                {
                    return 0.007f * NegativeInteractionUtility.NegativeInteractionChanceFactor(initiator, recipient);
                }
            }
            return 0f;
        }
    }
}
