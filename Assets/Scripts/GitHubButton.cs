/*
 * Author: Matěj Šťastný
 * Date created: 6/9/2025
 * GitHub link: https://github.com/matysta/dont-wake-up
 */

using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class GitHubButton : MonoBehaviour
{
    private MenuManager _menuManager;

    private void Start()
    {
        _menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
    }

    private void OnMouseUp()
    {
        Global.Log("Github button clicked");
        _menuManager.OpenGithub();
    }
}