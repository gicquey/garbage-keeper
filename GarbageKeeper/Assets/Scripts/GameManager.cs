public class GameManager
{
    public MainScene mainScene;

    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameManager();
            return _instance;
        }
    }

    public int CurrentMoney { get; private set; }
    public float CurrentLife  { get; private set;}

    private GameManager()
    {
        Initialize();
    }

    public void Initialize()
    {
        CurrentMoney = Settings.Instance.initialMoney;
        CurrentLife = Settings.Instance.lifeMax;
    }

    public bool CanBuyTurret()
    {
        return CurrentMoney >= Settings.Instance.turretCost;
    }

    public void BuyTurret()
    {
        SoundHelper.Instance.play(AudioConfig.Instance.GetClipForSoundType(SoundTypes.BUILD_TURRET));
        UpdateMoney(-Settings.Instance.turretCost);
    }

    public void RecycleTurret()
    {
        SoundHelper.Instance.play(AudioConfig.Instance.GetClipForSoundType(SoundTypes.DESTROY_TURRET));
        UpdateMoney(Settings.Instance.moneyGainedOnRecycleTurret);
    }

    public void UpdateMoney(int money)
    {
        CurrentMoney += money;

    }

    public void LoseLife(float amount)
    {
        CurrentLife -= amount;
        mainScene.RefreshLifeIndicator();
        if(CurrentLife <= 0)
        {
            mainScene.gameOver.Show();
        }
    }
}
