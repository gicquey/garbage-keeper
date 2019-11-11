using System.Collections;
using System.Collections.Generic;
using static Projectile;
using UnityEngine;

public class Tourelle : MonoBehaviour
{
    private Stack<Settings.AmmoType> clip = new Stack<Settings.AmmoType>();
    private float timer = 0f;
    public Transform projectilSpawn;

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
        for (int i = 0; i < WaveManager.Instance.AliveEnnemies.Count; ++i)
        {
            float distance = Vector3.Distance(WaveManager.Instance.AliveEnnemies[i].transform.position, transform.position);
            if (-1 == dist ||  distance < dist)
            {
                dist = distance;
                ret = i;
            }
        }
        //Si l'ennemi est au dela de la portée ou que la prochaine munition n'est pas une pile
        //on ne voit pas l'ennemi
        //if (dist > Settings.Instance.turretsNormalRange || (clip.Count > 0 && clip[0] != Settings.AmmoType.battery))
        //    ret = -1;
        return ret;
    }

    Projectile GenerateProjectile(Settings.AmmoType ammotype)
    {
        switch (ammotype)
        {
            case(Settings.AmmoType.regular):
                return Instantiate(((GameObject)Resources.Load("Prefabs/Projectiles/NormalProjectile")).GetComponent<Projectile>(), projectilSpawn.position,transform.rotation).GetComponent<Projectile>();
            case(Settings.AmmoType.battery):
                return Instantiate(((GameObject)Resources.Load("Prefabs/Projectiles/BatteryProjectile")).GetComponent<Projectile>(), projectilSpawn.position, transform.rotation).GetComponent<Projectile>();
            case (Settings.AmmoType.clothes):
                return Instantiate(((GameObject)Resources.Load("Prefabs/Projectiles/ClothesProjectile")).GetComponent<Projectile>(), projectilSpawn.position, transform.rotation).GetComponent<Projectile>();
            case (Settings.AmmoType.explosive):
                return Instantiate(((GameObject)Resources.Load("Prefabs/Projectiles/ExplosiveProjectile")).GetComponent<Projectile>(), projectilSpawn.position, transform.rotation).GetComponent<Projectile>();
            case (Settings.AmmoType.poison):
                return Instantiate(((GameObject)Resources.Load("Prefabs/Projectiles/PoisonProjectile")).GetComponent<Projectile>(), projectilSpawn.position, transform.rotation).GetComponent<Projectile>();
            case (Settings.AmmoType.puddle):
                return Instantiate(((GameObject)Resources.Load("Prefabs/Projectiles/PuddleProjectile")).GetComponent<Projectile>(), projectilSpawn.position, transform.rotation).GetComponent<Projectile>();
            default:
                return Instantiate(((GameObject)Resources.Load("Prefabs/Projectiles/NormalProjectile")).GetComponent<Projectile>(), projectilSpawn.position, transform.rotation).GetComponent<Projectile>();
        }
    }

    public void Shoot(Ennemi e)
    {
        Settings.AmmoType bullet = Settings.AmmoType.regular;
        if (clip.Count > 0)
        {
            bullet = clip.Pop();
        }
        Projectile projectile = GenerateProjectile(bullet);
        
        projectile.AimAtEnemy(e);
        projectile.ammoType = bullet;
        SoundHelper.Instance.play(AudioConfig.Instance.GetClipForSoundType(SoundTypes.SHOOT));
    }
    public void AddAmmo(int quantity, Settings.AmmoType type)
    {
        for (int i = 0; i < quantity && clip.Count < Settings.Instance.turretMaxAmmo; ++i)
        {
            clip.Push(type);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (WaveManager.Instance.AliveEnnemies != null)
        {
            int closest = GetClosestEnnemiId();
            LookAtEnnemi(WaveManager.Instance.AliveEnnemies[closest].transform);
            if (timer > Settings.Instance.turretFireRate && closest >= 0)
            {
                LookAtEnnemi(WaveManager.Instance.AliveEnnemies[closest].transform);
                Shoot(WaveManager.Instance.AliveEnnemies[closest]);
                timer = 0;
            }
        }
    }
}
