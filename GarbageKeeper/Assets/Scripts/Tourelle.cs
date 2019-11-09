using System.Collections;
using System.Collections.Generic;
using static Projectile;
using UnityEngine;

public class Tourelle : MonoBehaviour
{
    public Ennemi[] ennemis;
    public transform[] ennemiPos;
    private List<Settings.AmmoType> clip;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void LookAtEnnemi(transform closestEnnemi)
    {
        transform.LookAt(closestEnnemi);
    }

    int GetClosestEnnemiId()
    {
        float dist = -1;
        ret = -1;
        for (int i = 0; i < ennemis.Length; ++i)
        {
            float distance = Vector3.Distance(ennemis[i].transform, transform.position);
            if (-1 == dist ||  distance < dist)
            {
                dist = distance;
            }
        }
        dist < turretsNormalRange ? ret = dist : ret = -1;
        return ret;
    }

    public void Shoot(Ennemi e)
    {
        Projectile projectile;
        projectile.AimAtEnemy(e);
        Settings.AmmoType bullet = Settings.AmmoType.regular;
        if (clip.Count > 0)
        {
            bullet = clip[0];
            clip.RemoveAt(0);
        }
        projectile.ammoType(bullet);
    }
    public void AddAmmo(int quantity, Settings.AmmoType type)
    {
        for (int i = 0; i < quantity && clip.Length < Settings.turretMaxAmmo; ++i)
        {
            clip.Add(type);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        int closest = GetClosestEnnemiId();
        if (timer > Settings.turretFireRate && closest >= 0)
        {
            LookAtEnnemi(ennemis[closest]);
            Shoot(ennemis[closest]);
            timer -= Settings.turretFireRate;
        }
        
    }
}
