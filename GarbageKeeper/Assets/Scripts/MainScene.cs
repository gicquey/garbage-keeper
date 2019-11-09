using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    public List<Transform> checkpoints;

    public void Awake()
    {
        GameManager.Instance.mainScene = this;
    }

    public void RefreshLifeIndicator()
    {

    }
}
