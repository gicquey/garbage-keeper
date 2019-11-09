public class Effect
{
    public float TimeToLive { get; private set; }
    public EffectTypes EffectType { get; private set; }

    public Effect(EffectTypes effectType)
    {
        EffectType = effectType;
        TimeToLive = Settings.Instance.TimeToLiveByEffectType[effectType];
    }

    public void ReduceTimeToLive(float timeAmount)
    {
        TimeToLive -= timeAmount;
    }
}