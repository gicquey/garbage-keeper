using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EnnemyTypes
{
    NORMAL,
    FLYING
}

[RequireComponent(typeof(Animator))]
public class Ennemi : MonoBehaviour
{
    public EnnemyTypes ennemyType;
    public float maxLifeMultiplier = 1;
    public float baseSpeedMultiplier = 1;

    private Vector3? _lastCheckpointReached = null;
    private Vector3? _nextCheckpoint = null;
    public float _currentLife;
    private float _currentSpeedModifier;
    private List<Effect> _currentEffects = new List<Effect>();
    private Dictionary<EffectTypes, float> _timesSinceEffectActivations = new Dictionary<EffectTypes, float>();

    private bool _dying = false;

    private void Awake()
    {
        _currentLife = maxLifeMultiplier * Settings.Instance.baseEnnemyMaxLife;
        _timesSinceEffectActivations[EffectTypes.DAMAGE_OVER_TIME] = 0f;
        _timesSinceEffectActivations[EffectTypes.SLOW_DOWN] = 0f;
    }

    private void Start()
    {
        StartWalking();
    }

    private void Update()
    {
        _currentSpeedModifier = 1f;
        foreach(var effect in _currentEffects)
        {
            effect.ReduceTimeToLive(Time.deltaTime);
        }
        _currentEffects.RemoveAll(effect => effect.TimeToLive <= 0);
        ApplyEffects();

        if (_dying)
            return;

        if(_nextCheckpoint != null)
        {
            if (Vector3.Distance(this.transform.position, _nextCheckpoint.Value) < 0.1F)
            {
                ChangeDirection();
                return;
            }

            var travelDirection = Vector3.Normalize(_nextCheckpoint.Value - _lastCheckpointReached.Value);
            var movingSpeed = baseSpeedMultiplier * Settings.Instance.baseEnnemyMoveSpeed * _currentSpeedModifier;
            this.transform.Translate(
                movingSpeed * travelDirection.x,
                movingSpeed * travelDirection.y,
                movingSpeed * travelDirection.z,
                Space.World);

            var directionOfCheckpoint = Vector3.Normalize(_nextCheckpoint.Value - this.transform.position);

            if (Vector3.Dot(directionOfCheckpoint, travelDirection) < 0)
            {
                //If the ennemy has passed the checkpoint, we take him back on it
                this.transform.position = _nextCheckpoint.Value;
            }   
        }

    }

    private void ApplyEffects()
    {
        var effectTypesToApplyThisFrame = new List<EffectTypes>();
        foreach (var entry in _timesSinceEffectActivations.ToList().Where(testedEntry => _currentEffects.Exists(effect => effect.EffectType == testedEntry.Key))) //Modifying inside loop, so we use a copy
        {
            _timesSinceEffectActivations[entry.Key] = entry.Value + Time.deltaTime;
            if(_timesSinceEffectActivations[entry.Key] >= Settings.Instance.TimeBetweenActivationsByEffectType[entry.Key])
            {
                _timesSinceEffectActivations[entry.Key] = 0f;
                effectTypesToApplyThisFrame.Add(entry.Key);
            }
        }

        foreach(var effectType in effectTypesToApplyThisFrame)
        {
            ApplyEffect(effectType);
        }
    }

    private void ApplyEffect(EffectTypes effectType)
    {
        switch(effectType)
        {
            case EffectTypes.DAMAGE_OVER_TIME:
                TakeDamage(Settings.Instance.damageOverTimeDamageByActivation);
                break;

            case EffectTypes.SLOW_DOWN:
                _currentSpeedModifier = Settings.Instance.slowDownSpeedModifier;
                break;
        }
    }

    public void ApplyProjectileEffect(Projectile projectile)
    {
        TakeDamage(Settings.Instance.DamageByAmmoType[projectile.ammoType]);
        switch(projectile.ammoType)
        {
            case Settings.AmmoType.poison :
                SoundHelper.Instance.play(AudioConfig.Instance.GetClipForSoundType(SoundTypes.IMPACT_POISON));
                _currentEffects.Add(new Effect(EffectTypes.DAMAGE_OVER_TIME));
                _timesSinceEffectActivations[EffectTypes.DAMAGE_OVER_TIME] = Settings.Instance.TimeBetweenActivationsByEffectType[EffectTypes.DAMAGE_OVER_TIME];
                break;

            case Settings.AmmoType.puddle:
                _currentEffects.Add(new Effect(EffectTypes.SLOW_DOWN));
                SoundHelper.Instance.play(AudioConfig.Instance.GetClipForSoundType(SoundTypes.IMPACT_PUDDLE));
                _timesSinceEffectActivations[EffectTypes.SLOW_DOWN] = Settings.Instance.TimeBetweenActivationsByEffectType[EffectTypes.SLOW_DOWN];
                break;

            case Settings.AmmoType.explosive:
                foreach(var ennemyInRange in WaveManager.Instance.AliveEnnemies.Where(ennemy => Vector3.Distance(this.transform.position, ennemy.transform.position) <= Settings.Instance.explosionsRange))
                {
                    ennemyInRange.TakeDamage(Settings.Instance.explosionSideEffectDamage);
                }
                break;

            case Settings.AmmoType.battery:
                SoundHelper.Instance.play(AudioConfig.Instance.GetClipForSoundType(SoundTypes.IMPACT_BATTERY));
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        if (_dying)
            return;

        _currentLife -= damage;
        if(_currentLife <= 0)
        {
            _dying = true;
            SoundHelper.Instance.play(AudioConfig.Instance.GetClipForSoundType(SoundTypes.DIE_DAMAGE));
            WaveManager.Instance.RemoveEnnemy(this);
            this.GetComponent<Animator>().SetTrigger("DyingDamage");
            OnDyingDamageAnimationOver(); //Remove when DyingDamage calls it
        }
    }

    private void StartWalking()
    {
        GetComponent<Animator>().SetBool("Walking", true);
        _nextCheckpoint = GameManager.Instance.mainScene.Checkpoints[0];
        ChangeDirection();
    }

    private void ChangeDirection()
    {
        _lastCheckpointReached = _nextCheckpoint;
        int indexOfCheckpoint = GameManager.Instance.mainScene.Checkpoints.IndexOf(_lastCheckpointReached.Value);
        if(indexOfCheckpoint == GameManager.Instance.mainScene.Checkpoints.Count - 1)
        {
            ReachPathEnd();
        }
        else
        {
            this.transform.position = _nextCheckpoint.Value;
            _lastCheckpointReached = _nextCheckpoint;
            _nextCheckpoint = GameManager.Instance.mainScene.Checkpoints[indexOfCheckpoint + 1];
            this.transform.LookAt(_nextCheckpoint.Value);
        }
    }

    private void ReachPathEnd()
    {
        SoundHelper.Instance.play(AudioConfig.Instance.GetClipForSoundType(SoundTypes.DIE_END));
        WaveManager.Instance.RemoveEnnemy(this);
        _dying = true;
        this.GetComponent<Animator>().SetTrigger("DyingEnd");
        _nextCheckpoint = null;
        OnDyingEndAnimationOver(); //TODO Remove when DyingEnd calls it
    }

    public void OnDyingDamageAnimationOver()
    {
        InventoryManager.Instance.ObtainResourcesForEnnemy(ennemyType);
        Destroy(this.gameObject);
    }

    //Branch to event in dying animation
    public void OnDyingEndAnimationOver()
    {
        GameManager.Instance.LoseLife(Settings.Instance.lifeLostPerEnnemy);
        Destroy(this.gameObject);
    }
}
