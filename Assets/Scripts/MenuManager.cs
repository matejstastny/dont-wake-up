/*
 * Author: Matěj Šťastný
 * Date created: 6/4/2025
 * GitHub link: https://github.com/matysta/dont-wake-up
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("References")]
    private AudioSource _audioSource;

    // Start --------------------------------------------------------------------------------------------
    
    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "Menu") return;
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true;
        _audioSource.Stop();
        _audioSource.Play(0);
    }

    // Room Switching -----------------------------------------------------------------------------------
    
    public void SwitchToMenu()
    {
        SceneManager.LoadScene("Menu");
        _audioSource.Stop();
        _audioSource.Play(0);
    }
    
    public void SwitchToGame()
    {
        _audioSource.Stop();
        SceneManager.LoadScene("Main");
    }

    public void OpenGithub()
    {
        Application.OpenURL("https://github.com/matysta/dont-wake-up");
    }
}
