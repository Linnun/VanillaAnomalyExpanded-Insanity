using Verse;

namespace VAEInsanity
{
    public class SanityEffect : IExposable
    {
        public FloatRange sanityValue;
        public bool enabled = true;
        public SanityEffect()
        {

        }

        public SanityEffect(FloatRange value)
        {
            this.sanityValue = value;
        }
        public SanityEffect(float value)
        {
            this.sanityValue = new FloatRange(value);
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref sanityValue, "sanityValue");
            Scribe_Values.Look(ref enabled, "enabled", true);
        }
    }
}
