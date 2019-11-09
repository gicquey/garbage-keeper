using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tourelle : MonoBehaviour
{
    public transform[] ennemiPos;

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
        for (int i = 0; i < ennemiPos.Length; ++i)
        {
            float distance = Vector3.Distance(ennemiPos[i], transform.position);
            if (-1 == dist ||  distance < dist)
            {
                dist = distance;
            }
        }
        return ret;
    }

    // Update is called once per frame
    void Update()
    {
        int closest = GetClosestEnnemiId();
        LookAtEnnemi(ennemiPos[closest]);
    }
}
