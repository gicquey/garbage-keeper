using System.Collections;
using System.Collections.Generic;
using static Projectile;
using UnityEngine;

public class Tourelle : MonoBehaviour
{
    private List<Settings.AmmoType> clip = new List<Settings.AmmoType>();
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    void LookAtEnnemi(Transform closestEnnemi)
    {
        transform.LookAt(new Vector3(closestEnnemi.position.x, transform.position.y, closestEnnemi.position.z) );
    }

    int GetClosestEnnemiId()
    {
        float dist = -1f;
        int ret = -1;
        for (int i = 0; i < EnnemyGenerator.Instance.AliveEnnemies.Count; ++i)
        {
            float distance = Vector3.Distance(EnnemyGenerator.Instance.AliveEnnemies[i].transform.position, transform.position);
            if (-1 == dist ||  distance < dist)
            {
                dist = distance;
                ret = i;
            }
        }
        if (dist > Settings.turretsNormalRange)
            ret = -1;
        return ret;
    }

    Projectile GenerateProjectile(Settings.AmmoType ammotype)
    {
        switch (ammotype)
        {
            case(Settings.AmmoType.regular):
                return(((GameObject)Instantiate(Resources.Load("Prefabs/Projectiles/NormalProjectile"))).GetComponent<Projectile>());
                break;
            case(Settings.AmmoType.battery):
                return(((GameObject)Instantiate(Resources.Load("Prefabs/Projectiles/BatteryProjectile"))).GetComponent<Projectile>());
            break;
            case(Settings.AmmoType.clothes):
                return(((GameObject)Instantiate(Resources.Load("Prefabs/Projectiles/ClothesProjectile"))).GetComponent<Projectile>());
            break;
            case(Settings.AmmoType.explosive):
                return(((GameObject)Instantiate(Resources.Load("Prefabs/Projectiles/ExplosiveProjectile"))).GetComponent<Projectile>());
            break;
            case(Settings.AmmoType.poison):
                return(((GameObject)Instantiate(Resources.Load("Prefabs/Projectiles/PoisonProjectile"))).GetComponent<Projectile>());
            break;
            case(Settings.AmmoType.puddle):
                return(((GameObject)Instantiate(Resources.Load("Prefabs/Projectiles/PuddleProjectile"))).GetComponent<Projectile>());
            break;
            default:
                return(((GameObject)Instantiate(Resources.Load("Prefabs/Projectiles/NormalProjectile"))).GetComponent<Projectile>());
            break;
        }
    }

    public void Shoot(Ennemi e)
    {
        Settings.AmmoType bullet = Settings.AmmoType.regular;
        if (clip.Count > 0)
        {
            bullet = clip[0];
            clip.RemoveAt(0);
        }
        Projectile projectile = GenerateProjectile(bullet);
        projectile.AimAtEnemy(e);
        projectile.ammoType = bullet;
    }
    public void AddAmmo(int quantity, Settings.AmmoType type)
    {
        for (int i = 0; i < quantity && clip.Count < Settings.turretMaxAmmo; ++i)
        {
            clip.Add(type);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (EnnemyGenerator.Instance.AliveEnnemies != null)
        {
            int closest = GetClosestEnnemiId();
            if (timer > Settings.turretFireRate && closest >= 0)
            {
                LookAtEnnemi(EnnemyGenerator.Instance.AliveEnnemies[closest].transform);
                Shoot(EnnemyGenerator.Instance.AliveEnnemies[closest]);
                timer -= Settings.turretFireRate;
            }
        }
    }
}
