using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public bool showExplosionRangeAroundExplosives = false;

    public int currentMoney;

    private static DebugManager _instance = null;
    public static DebugManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        currentMoney = GameManager.Instance.CurrentMoney;
    }
}
