using UnityEngine;
using static Settings;

public class Projectile : MonoBehaviour
{
    public AmmoType ammoType;

    private Ennemi _targetedEnemy;

    public void AimAtEnemy (Ennemi enemy)
    {
        _targetedEnemy = enemy;
    }

    // Update is called once per frame
    void Update()
    {
        if(_targetedEnemy != null)
        {
            if (Vector3.Distance(this.transform.position, _targetedEnemy.transform.position) < 0.1F)
            {
                _targetedEnemy.ApplyProjectileEffect(this);
                Destroy(this.gameObject);
                return;
            }

            var travelDirection = Vector3.Normalize(_targetedEnemy.transform.position - this.transform.position);
            this.transform.Translate(
                Settings.Instance.projectilesSpeed * travelDirection.x,
                Settings.Instance.projectilesSpeed * travelDirection.y,
                Settings.Instance.projectilesSpeed * travelDirection.z,
                Space.World);

            var directionOfCheckpoint = Vector3.Normalize(_targetedEnemy.transform.position - this.transform.position);

            if (Vector3.Dot(directionOfCheckpoint, travelDirection) < 0)
            {
                //If the projectile has passed the enemy we take him back on it
                this.transform.position = _targetedEnemy.transform.position;
            }
        }
        else
        {
            //Projectiles get a target the frame they're created. If they don't have one, their target no longer exists and they must disappear
            Destroy(this.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (DebugManager.Instance.gameObject.activeSelf && DebugManager.Instance.showExplosionRangeAroundExplosives && ammoType == AmmoType.explosive)
        {
            Gizmos.DrawSphere(this.transform.position, Settings.Instance.explosionsRange);
        }
    }
}
