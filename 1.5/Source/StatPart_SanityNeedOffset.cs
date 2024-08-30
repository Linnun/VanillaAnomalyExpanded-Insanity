using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace VAEInsanity
{
    [HotSwappable]
    public class StatPart_SanityNeedOffset : StatPart
    {
        private const float LabyrinthEffect = -0.01f;
        private const float UnnaturalDarknessEffect = -0.12f;

        public override void TransformValue(StatRequest req, ref float val)
        {
            TryGetSanityValue(req.Thing as Pawn, out val, out _);
        }

        public override string ExplanationPart(StatRequest req)
        {
            if (TryGetSanityValue(req.Thing as Pawn, out var val, out var explanation))
            {
                var part = "VAEI_StatsReport_SanityGainPerDay".Translate(val.ToStringWithSign());
                if (explanation.Length > 0)
                {
                    part += "\n" + "VAEI_PassiveEffects".Translate(explanation);
                }
                return part;
            }
            return null;
        }

        public static bool TryGetSanityValue(Pawn pawn, out float val, out StringBuilder explanation)
        {
            explanation = new StringBuilder();
            val = 0f;
            var sanity = pawn?.needs?.TryGetNeed<Need_Sanity>();
            if (sanity != null)
            {
                sanity.HandleSanityLevel(ref val, explanation);
                foreach (var hediff in pawn.health.hediffSet.hediffs)
                {
                    foreach (var def in DefDatabase<SanityEffectsDef>.AllDefs)
                    {
                        if (def.hediffEffects != null)
                        {
                            foreach (var effect in def.hediffEffects)
                            {
                                if (effect.hediff == hediff.def)
                                {
                                    sanity.AddEffect(ref val, effect, explanation, "VAEI_AnomalyBodypart".Translate(hediff.Label, effect.effect.RandomInRange.ToStringPercentSigned("F2")));
                                }
                            }
                        }
                    }
                }

                if (pawn.duplicate.duplicateOf != int.MinValue)
                {
                    var comp = Current.Game.GetComponent<GameComponent_PawnDuplicator>();
                    if (comp.duplicates.TryGetValue(pawn.duplicate.duplicateOf, out var duplicates))
                    {
                        if (duplicates.pawns.Any(x => x.Dead is false && x != pawn))
                        {
                            sanity.AddEffect(ref val, -0.01f, explanation, 
                                "VAEI_BeingDuplicated".Translate((-0.01f).ToStringPercentSigned("F2")));
                        }
                    }
                }

                if (Find.Anomaly.TryGetUnnaturalCorpseTrackerForHaunted(pawn, out var _))
                {
                    sanity.AddEffect(ref val, -0.01f, explanation,
                        "VAEI_UnnaturalCorpse".Translate((-0.01f).ToStringPercentSigned("F2")));
                }

                if (pawn.Spawned)
                {
                    if (pawn.Map.generatorDef == MapGeneratorDefOf.Labyrinth)
                    {
                        sanity.AddEffect(ref val, LabyrinthEffect, explanation, "VAEI_BeingInLabyrinth".Translate(LabyrinthEffect.ToStringPercentSigned("F2")));
                    }

                    if (GameCondition_UnnaturalDarkness.InUnnaturalDarkness(pawn))
                    {
                        sanity.AddEffect(ref val, UnnaturalDarknessEffect, explanation, 
                            "VAEI_BeingInUnnaturalDarkness".Translate(UnnaturalDarknessEffect.ToStringPercentSigned("F2")));
                    }
                }
            }

            return explanation.Length > 0;
        }
    }
}
