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

    private float _currentLife = 0;

    private GameManager()
    {
        _currentLife = Settings.Instance.lifeMax;
    }

    public void LoseLife(float amount)
    {
        _currentLife -= amount;
        mainScene.RefreshLifeIndicator();
        if(_currentLife < 0)
        {
            //Game over
        }
    }
}
