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
        //Si l'ennemi est au dela de la portée ou que la prochaine munition n'est pas une pile
        //on ne voit pas l'ennemi
        if (dist > Settings.Instance.turretsNormalRange || (clip.Count > 0 && clip[0] != Settings.AmmoType.battery))
            ret = -1;
        return ret;
    }

    Projectile GenerateProjectile(Settings.AmmoType ammotype)
    {
        switch (ammotype)
        {
            case(Settings.AmmoType.regular):
                return(((GameObject)Instantiate(Resources.Load("Prefabs/Projectiles/NormalProjectile"))).GetComponent<Projectile>());
            case(Settings.AmmoType.battery):
                return(((GameObject)Instantiate(Resources.Load("Prefabs/Projectiles/BatteryProjectile"))).GetComponent<Projectile>());
            case(Settings.AmmoType.clothes):
                return(((GameObject)Instantiate(Resources.Load("Prefabs/Projectiles/ClothesProjectile"))).GetComponent<Projectile>());
            case(Settings.AmmoType.explosive):
                return(((GameObject)Instantiate(Resources.Load("Prefabs/Projectiles/ExplosiveProjectile"))).GetComponent<Projectile>());
            case(Settings.AmmoType.poison):
                return(((GameObject)Instantiate(Resources.Load("Prefabs/Projectiles/PoisonProjectile"))).GetComponent<Projectile>());
            case(Settings.AmmoType.puddle):
                return(((GameObject)Instantiate(Resources.Load("Prefabs/Projectiles/PuddleProjectile"))).GetComponent<Projectile>());
            default:
                return(((GameObject)Instantiate(Resources.Load("Prefabs/Projectiles/NormalProjectile"))).GetComponent<Projectile>());
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
        projectile.transform.position = transform.position + Settings.Instance.bulletOffset;
        projectile.AimAtEnemy(e);
        projectile.ammoType = bullet;
        SoundHelper.Instance.play(AudioConfig.Instance.GetClipForSoundType(SoundTypes.SHOOT));
    }
    public void AddAmmo(int quantity, Settings.AmmoType type)
    {
        for (int i = 0; i < quantity && clip.Count < Settings.Instance.turretMaxAmmo; ++i)
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
            if (timer > Settings.Instance.turretFireRate && closest >= 0)
            {
                LookAtEnnemi(EnnemyGenerator.Instance.AliveEnnemies[closest].transform);
                Shoot(EnnemyGenerator.Instance.AliveEnnemies[closest]);
                timer -= Settings.Instance.turretFireRate;
            }
        }
    }
}
