using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    public Transform tilesRoot;
    public Transform checkpointsRoot;
    public Transform topTilesTopCorner;

    public List<Transform> checkpoints;

    public void Awake()
    {
        GameManager.Instance.mainScene = this;
    }

    public void RefreshLifeIndicator()
    {

    }
}
