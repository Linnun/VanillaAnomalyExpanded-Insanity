using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VAEInsanity
{
    public class PsychicRitualEffect
    {
        public PsychicRitualDef_InvocationCircle ritual;
        public float invokerEffect, targetEffect, chanterEffect;
    }

    public abstract class SanityEffectBase
    {
        public float effect;
        public string description;
    }

    public class InteractionEffect : SanityEffectBase
    {
        public InteractionDef interaction;
    }

    public class HediffEffect : SanityEffectBase
    {
        public HediffDef hediff;
    }

    public class SanityEffectsDef : Def
    {
        public List<PsychicRitualEffect> psychicRitualEffects;
        public List<InteractionEffect> disturbingInitiatorEffects, interactionEffects;
        public List<HediffEffect> hediffEffects;
    }
}
