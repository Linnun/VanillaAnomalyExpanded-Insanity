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
        private const float LowSanityEffect = -0.01f;
        private const float HighSanityEffect = 0.01f;
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
                HandleSanityLevel(sanity.CurLevel, val, explanation);

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
                                    AddEffect(ref val, effect, explanation, "VAEI_AnomalyBodypart".Translate(hediff.Label, effect.effect.RandomInRange.ToStringPercentSigned("F2")));
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
                            AddEffect(ref val, -0.01f, explanation, 
                                "VAEI_BeingDuplicated".Translate((-0.01f).ToStringPercentSigned("F2")));
                        }
                    }
                }

                if (Find.Anomaly.TryGetUnnaturalCorpseTrackerForHaunted(pawn, out var _))
                {
                    AddEffect(ref val, -0.01f, explanation,
                        "VAEI_UnnaturalCorpse".Translate((-0.01f).ToStringPercentSigned("F2")));
                }

                if (pawn.Spawned)
                {
                    if (pawn.Map.generatorDef == MapGeneratorDefOf.Labyrinth)
                    {
                        AddEffect(ref val, LabyrinthEffect, explanation, "VAEI_BeingInLabyrinth".Translate(LabyrinthEffect.ToStringPercentSigned("F2")));
                    }

                    if (GameCondition_UnnaturalDarkness.InUnnaturalDarkness(pawn))
                    {
                        AddEffect(ref val, UnnaturalDarknessEffect, explanation, 
                            "VAEI_BeingInUnnaturalDarkness".Translate(UnnaturalDarknessEffect.ToStringPercentSigned("F2")));
                    }
                }
            }

            return explanation.Length > 0;
        }

        private static void HandleSanityLevel(float currentSanityLevel, float val, StringBuilder explanation)
        {
            if (currentSanityLevel <= 0.1f)
            {
                AddEffect(ref val, LowSanityEffect, explanation, "VAEI_LowSanity".Translate(LowSanityEffect.ToStringPercentSigned("F2")));
            }
            else if (currentSanityLevel >= 0.9f)
            {
                AddEffect(ref val, HighSanityEffect, explanation, "VAEI_HighSanity".Translate(HighSanityEffect.ToStringPercentSigned("F2")));
            }
        }

        private static void AddEffect(ref float val, SanityEffectBase effect, StringBuilder explanation, string message)
        {
            var value = effect.effect.RandomInRange;
            val += value;
            if (effect.description.NullOrEmpty() is false)
            {
                message = effect.description + ": " + value.ToStringPercentSigned("F2");
            }
            explanation.AppendLine(message);
        }

        private static void AddEffect(ref float val, float effect, StringBuilder explanation, string message)
        {
            val += effect;
            explanation.AppendLine(message);
        }
    }
}
