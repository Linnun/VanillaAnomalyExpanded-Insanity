using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VAEInsanity
{
    public class StatPart_Sanity : StatPart
    {
        public List<SanityFactor> factors;

        public override void TransformValue(StatRequest req, ref float val)
        {
            if (req.HasThing && req.Thing is Pawn pawn)
            {
                Need_Sanity sanity = pawn.needs.TryGetNeed<Need_Sanity>();
                if (sanity != null)
                {
                    val *= GetFactor(sanity.CurLevel);
                }
            }
        }

        public override string ExplanationPart(StatRequest req)
        {
            if (req.HasThing && req.Thing is Pawn pawn)
            {
                Need_Sanity sanity = pawn.needs.TryGetNeed<Need_Sanity>();
                if (sanity != null)
                {
                    float factor = GetFactor(sanity.CurLevel);
                    if (factor != 1f)
                    {
                        return "VAEI_SanityFactorExplanation".Translate(factor.ToStringPercent());
                    }
                }
            }
            return null;
        }

        private float GetFactor(float sanityLevel)
        {
            foreach (SanityFactor factor in factors)
            {
                if (sanityLevel >= factor.min && sanityLevel < factor.max)
                {
                    return factor.factor;
                }
            }
            return 1f;
        }
    }
}
