using System.Collections;
using System.Collections.Generic;
using static Projectile;
using UnityEngine;

public class Tourelle : MonoBehaviour
{
    public Ennemi[] ennemis;
    public Transform[] ennemiPos;
    private List<Settings.AmmoType> clip = new List<Settings.AmmoType>();
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void LookAtEnnemi(Transform closestEnnemi)
    {
        transform.LookAt(closestEnnemi);
    }

    float GetClosestEnnemiId()
    {
        float dist = -1f;
        float ret = -1f;
        for (int i = 0; i < ennemis.Length; ++i)
        {
            float distance = Vector3.Distance(ennemis[i].transform.position, transform.position);
            if (-1 == dist ||  distance < dist)
            {
                dist = distance;
            }
        }
        if (dist < Settings.turretsNormalRange){
            ret = dist;
        }
        else
            ret = -1f;
        return ret;
    }

    public void Shoot(Ennemi e)
    {
        Projectile projectile = new Projectile();
        projectile.AimAtEnemy(e);
        Settings.AmmoType bullet = Settings.AmmoType.regular;
        if (clip.Count > 0)
        {
            bullet = clip[0];
            clip.RemoveAt(0);
        }
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
        float closest = GetClosestEnnemiId();
        if (timer > Settings.turretFireRate && closest >= 0)
        {
            LookAtEnnemi(ennemis[(int)closest].transform);
            Shoot(ennemis[(int)closest]);
            timer -= Settings.turretFireRate;
        }
        
    }
}
