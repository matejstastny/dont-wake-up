/*
 * Author: Matěj Šťastný
 * Date created: 6/4/2025
 * GitHub link: https://github.com/matysta/dont-wake-up
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{
    public void SwitchToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    
    public void SwitchToGame()
    {
        SceneManager.LoadScene("Main");
    }
}
