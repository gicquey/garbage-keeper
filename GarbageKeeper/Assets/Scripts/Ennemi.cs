using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Ennemi : MonoBehaviour
{
    public float maxLife = 50f;
    public float baseMovingSpeed = 0.05f;

    private Transform _lastCheckpointReached = null;
    private Transform _nextCheckpoint = null;
    private float _currentLife;
    private float _currentSpeedModifier;
    private List<Effect> _currentEffects = new List<Effect>();
    private Dictionary<EffectTypes, float> _timesSinceEffectActivations = new Dictionary<EffectTypes, float>();

    private bool _dying = false;

    private void Awake()
    {
        _currentLife = maxLife;
        _timesSinceEffectActivations[EffectTypes.DAMAGE_OVER_TIME] = 0f;
        _timesSinceEffectActivations[EffectTypes.SLOW_DOWN] = 0f;
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
            if (Vector3.Distance(this.transform.position, _nextCheckpoint.position) < 0.1F)
            {
                ChangeDirection();
                return;
            }

            var travelDirection = Vector3.Normalize(_nextCheckpoint.position - _lastCheckpointReached.position);
            var movingSpeed = baseMovingSpeed * _currentSpeedModifier;
            this.transform.Translate(
                movingSpeed * travelDirection.x,
                movingSpeed * travelDirection.y,
                movingSpeed * travelDirection.z,
                Space.World);

            var directionOfCheckpoint = Vector3.Normalize(_nextCheckpoint.position - this.transform.position);

            if (Vector3.Dot(directionOfCheckpoint, travelDirection) < 0)
            {
                //If the ennemy has passed the checkpoint, we take him back on it
                this.transform.position = _nextCheckpoint.position;
            }   
        }

    }

    private void ApplyEffects()
    {
        var effectTypesToApplyThisFrame = new List<EffectTypes>();
        foreach (var entry in _timesSinceEffectActivations.ToList()) //Modifying inside loop, so we use a copy
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
                _currentEffects.Add(new Effect(EffectTypes.DAMAGE_OVER_TIME));
                break;

            case Settings.AmmoType.puddle:
                _currentEffects.Add(new Effect(EffectTypes.SLOW_DOWN));
                break;

            case Settings.AmmoType.explosive:
                foreach(var ennemyInRange in GameObject.FindObjectsOfType<Ennemi>().Where(ennemy => Vector3.Distance(this.transform.position, ennemy.transform.position) <= Settings.Instance.explosionsRange))
                {
                    ennemyInRange.TakeDamage(Settings.Instance.explosionSideEffectDamage);
                }
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
            this.GetComponent<Animator>().SetTrigger("DyingDamage");
        }
    }

    private void StartWalking()
    {
        GetComponent<Animator>().SetBool("Walking", true);
        _nextCheckpoint = GameManager.Instance.mainScene.checkpoints[0];
        ChangeDirection();
    }

    private void ChangeDirection()
    {
        _lastCheckpointReached = _nextCheckpoint;
        int indexOfCheckpoint = GameManager.Instance.mainScene.checkpoints.IndexOf(_lastCheckpointReached);
        if(indexOfCheckpoint == GameManager.Instance.mainScene.checkpoints.Count - 1)
        {
            ReachPathEnd();
        }
        else
        {
            this.transform.position = _nextCheckpoint.position;
            _lastCheckpointReached = _nextCheckpoint;
            _nextCheckpoint = GameManager.Instance.mainScene.checkpoints[indexOfCheckpoint + 1];
            this.transform.LookAt(_nextCheckpoint);
        }
    }

    private void ReachPathEnd()
    {
        _dying = true;
        this.GetComponent<Animator>().SetTrigger("DyingEnd");
        _nextCheckpoint = null;
    }

    public void OnDyingDamageAnimationOver()
    {
        Destroy(this.gameObject);
    }

    //Branch to event in dying animation
    public void OnDyingEndAnimationOver()
    {
        GameManager.Instance.LoseLife(Settings.Instance.lifeLostPerEnnemy);
        Destroy(this.gameObject);
    }
}
