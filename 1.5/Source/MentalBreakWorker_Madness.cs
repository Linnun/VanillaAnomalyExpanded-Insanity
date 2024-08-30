using RimWorld;
using Verse;
using Verse.AI;

namespace VAEInsanity
{
    public class MentalBreakWorker_Madness : MentalBreakWorker
    {
        public override bool BreakCanOccur(Pawn pawn)
        {
            var need = pawn.needs.TryGetNeed<Need_Sanity>();
            if (need is null || need.CurLevel > 0)
            {
                return false;
            }
            return base.BreakCanOccur(pawn);
        }
    }
}
