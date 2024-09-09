using RimWorld;
using Verse;

namespace VAEInsanity
{
    public class VoidDrawing : Filth, IObservedThoughtGiver
    {
        public HistoryEventDef GiveObservedHistoryEvent(Pawn observer)
        {
            return null;
        }

        public Thought_Memory GiveObservedThought(Pawn observer)
        {
            var sanity = observer.needs.TryGetNeed<Need_Sanity>();
            if (sanity is null)
            {
                return null;
            }
            Thought_MemoryObservation obj = (Thought_MemoryObservation)ThoughtMaker.MakeThought(DefsOf.VAEI_VoidDrawings);
            obj.Target = this;
            if (VAEInsanityModSettings.observingVoidDrawings.TryGetEffect(out var effect))
            {
                sanity.GainSanity(effect, "VAEI_ObservingVoidDrawings".Translate());
            }
            return obj;
        }
    }
}
