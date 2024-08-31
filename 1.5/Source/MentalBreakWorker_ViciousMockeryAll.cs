using Verse;
using Verse.AI;

namespace VAEInsanity
{
    public class MentalBreakWorker_ViciousMockeryAll : MentalBreakWorker
    {
        public override float CommonalityFor(Pawn pawn, bool moodCaused = false)
        {
            Need_Sanity sanity = pawn.needs?.TryGetNeed<Need_Sanity>();
            if (sanity == null)
            {
                return 0f;
            }
            float sanityLevel = sanity.CurLevel;
            if (sanityLevel > 0.75f)
            {
                return 0f;
            }
            else if (sanityLevel > 0.25f)
            {
                return 1f;
            }
            return 1.5f;
        }
    }
}
