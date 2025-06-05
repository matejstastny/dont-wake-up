using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{
    public void SwitchToMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }
    
    public void SwitchToGame()
    {
        SceneManager.LoadScene("Main");
    }
}
