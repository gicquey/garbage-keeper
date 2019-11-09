public class Settings
{
    private static Settings _instance;

    public static Settings Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Settings();
            return _instance;
        }
    }

    #region Sound
    public float soundVolume = 0.5f;
    public float musicVolume = 0.5f;
    public bool soundEnable = true;
    public bool musicEnable = true;
    #endregion

    #region Game
    public float lifeMax = 100f;
    public float lifeLostPerEnnemy = 2f;
    #endregion
}
