/*
 * Author: Matěj Šťastný
 * Date created: 6/4/2025
 * GitHub link: https://github.com/matysta/dont-wake-up
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true;
        _audioSource.Stop();
        _audioSource.Play(0);
    }

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
}
