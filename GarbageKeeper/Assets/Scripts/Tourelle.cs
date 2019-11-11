using System.Collections;
using System.Collections.Generic;
using static Projectile;
using UnityEngine;
using UnityEngine.UI;

public class Tourelle : MonoBehaviour
{
    private Stack<Settings.AmmoType> clip = new Stack<Settings.AmmoType>();
    private float timer = 0f;
    public Transform projectilSpawn;
    public Animator poubelleAnimator;
    public Image ammoTypeFeedBack;

    public Sprite regular;
    public Sprite battery;
    public Sprite clothes;
    public Sprite explosive;
    public Sprite poison;
    public Sprite puddle;

    // Start is called before the first frame update
    void Start()
    {

        ammoTypeFeedBack.sprite = Resources.Load<Sprite>("Sprite/regular");
    }

    void LookAtEnnemi(Transform closestEnnemi)
    {
        transform.LookAt(new Vector3(closestEnnemi.position.x, transform.position.y, closestEnnemi.position.z));
    }

    int GetClosestEnnemiId()
    {
        float dist = -1f;
        int ret = -1;
        for (int i = 0; i < WaveManager.Instance.AliveEnnemies.Count; ++i)
        {
            float distance = Vector3.Distance(WaveManager.Instance.AliveEnnemies[i].transform.position, transform.position);
            if (-1 == dist || distance < dist)
            {
                dist = distance;
                ret = i;
            }
        }
        return ret;
    }

    Projectile GenerateProjectile(Settings.AmmoType ammotype)
    {
        switch (ammotype)
        {
            case (Settings.AmmoType.regular):
                ammoTypeFeedBack.sprite = regular;
                return Instantiate(((GameObject)Resources.Load("Prefabs/Projectiles/NormalProjectile")).GetComponent<Projectile>(), projectilSpawn.position, transform.rotation).GetComponent<Projectile>();
            case (Settings.AmmoType.battery):
                ammoTypeFeedBack.sprite = battery;
                return Instantiate(((GameObject)Resources.Load("Prefabs/Projectiles/BatteryProjectile")).GetComponent<Projectile>(), projectilSpawn.position, transform.rotation).GetComponent<Projectile>();
            case (Settings.AmmoType.clothes):
                ammoTypeFeedBack.sprite = clothes;
                return Instantiate(((GameObject)Resources.Load("Prefabs/Projectiles/ClothesProjectile")).GetComponent<Projectile>(), projectilSpawn.position, transform.rotation).GetComponent<Projectile>();
            case (Settings.AmmoType.explosive):
                ammoTypeFeedBack.sprite = explosive;
                return Instantiate(((GameObject)Resources.Load("Prefabs/Projectiles/ExplosiveProjectile")).GetComponent<Projectile>(), projectilSpawn.position, transform.rotation).GetComponent<Projectile>();
            case (Settings.AmmoType.poison):
                ammoTypeFeedBack.sprite = poison;
                return Instantiate(((GameObject)Resources.Load("Prefabs/Projectiles/PoisonProjectile")).GetComponent<Projectile>(), projectilSpawn.position, transform.rotation).GetComponent<Projectile>();
            case (Settings.AmmoType.puddle):
                ammoTypeFeedBack.sprite = puddle;
                return Instantiate(((GameObject)Resources.Load("Prefabs/Projectiles/PuddleProjectile")).GetComponent<Projectile>(), projectilSpawn.position, transform.rotation).GetComponent<Projectile>();
            default:
                ammoTypeFeedBack.sprite = regular;
                return Instantiate(((GameObject)Resources.Load("Prefabs/Projectiles/NormalProjectile")).GetComponent<Projectile>(), projectilSpawn.position, transform.rotation).GetComponent<Projectile>();
        }
    }

    public void Shoot(Ennemi e)
    {
        poubelleAnimator.SetTrigger("Shoot");

        StartCoroutine(ShootCoroutine(e, poubelleAnimator));

    }

    IEnumerator ShootCoroutine(Ennemi e, Animator anim)
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + anim.GetCurrentAnimatorStateInfo(0).normalizedTime - 0.2f);

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
