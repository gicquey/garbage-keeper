using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public List<Ennemi> waveContent;
    public int count { get { return waveContent.Count; } }
    public int timeBetweenSpawn;

}
