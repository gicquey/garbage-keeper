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
            var currentTarget = GetTarget();
            if (timer > Settings.Instance.turretFireRate && currentTarget != null)
            {
                LookAtEnnemi(currentTarget.transform);
                Shoot(currentTarget);
                timer = 0;
            }
        }
    }

    private Ennemi GetTarget()
    {
        Ennemi currentTarget = null;
        float bestPathProgress = -1f;
        foreach(var ennemy in GetTargetableEnnemies())
        {
            if(currentTarget == null || (ennemy.GetPathProgress() > bestPathProgress))
            {
                currentTarget = ennemy;
                bestPathProgress = ennemy.GetPathProgress();
            }
        }
        return currentTarget;
    }

    private List<Ennemi> GetTargetableEnnemies()
    {
        var targetableEnnemies = new List<Ennemi>(WaveManager.Instance.AliveEnnemies);

        if(GetCurrentAmmo() != Settings.AmmoType.clothes)
        {
            targetableEnnemies.RemoveAll(ennemy => ennemy.ennemyType == EnnemyTypes.FLYING);
        }

        targetableEnnemies.RemoveAll(ennemy => Vector3.Distance(ennemy.transform.position, this.transform.position) > GetRange());
        return targetableEnnemies;
    }

    private Settings.AmmoType GetCurrentAmmo()
    {
        var currentAmmo = Settings.AmmoType.regular;
        if (clip.Count > 0)
            currentAmmo = clip.Peek();
        return currentAmmo;
    }

    public float GetRange()
    {
        float distance = Settings.Instance.turretsNormalRange;
        if (GetCurrentAmmo() == Settings.AmmoType.battery)
            distance = Settings.Instance.turretBatteryAmmoRange;
        return distance;
    }
}
