using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("MapScene");
        GameManager.Instance.Initialize();
    }
}