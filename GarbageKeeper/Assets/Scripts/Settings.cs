using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    private static Settings _instance;

    public static Settings Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Settings();
            return _instance;
        }
    }

    #region Sound
    public float soundVolume = 0.5f;
    public float musicVolume = 0.5f;
    public bool soundEnable = true;
    public bool musicEnable = true;
    #endregion

    #region Game
    public int initialMoney = 100;
    public int turretCost = 50;
    public int moneyGainedOnRecycleTurret = 25;

    public float lifeMax = 100f;
    public float lifeLostPerEnnemy = 2f;

    public float baseEnnemyMaxLife = 50f;
    public float baseEnnemyMoveSpeed = 0.01f;

    public enum AmmoType
    {
        regular,
        explosive,
        poison,
        puddle,
        clothes,
        battery
    };

    public enum Elements
    {
        organic,
        chimical,
        fabric,
        solid,
    };

    public Dictionary<EffectTypes, float> TimeToLiveByEffectType = new Dictionary<EffectTypes, float>()
    {
        { EffectTypes.DAMAGE_OVER_TIME, 3f},
        { EffectTypes.SLOW_DOWN, 5f }
    };

    public Dictionary<EffectTypes, float> TimeBetweenActivationsByEffectType = new Dictionary<EffectTypes, float>()
    {
        { EffectTypes.DAMAGE_OVER_TIME, 1f},
        { EffectTypes.SLOW_DOWN, 0f }
    };

    public Dictionary<AmmoType, float> DamageByAmmoType = new Dictionary<AmmoType, float>()
    {
        {AmmoType.regular, 1f },
        {AmmoType.explosive, 1f },
        {AmmoType.poison, 1f },
        {AmmoType.puddle, 1f },
        {AmmoType.clothes, 1f },
        {AmmoType.battery, 1f }
    };

    public float damageOverTimeDamageByActivation = 1f;
    public float explosionSideEffectDamage = 1f;
    public float slowDownSpeedModifier = 0.5f;
    public float projectilesSpeed = 0.1f;
    public float turretsNormalRange = 3f;
    public float explosionsRange = 10f;
    public int turretMaxAmmo = 10;
    public float turretFireRate = 1f;
    public Vector3 bulletOffset = new Vector3(0, 1, 0);
    #endregion
}
