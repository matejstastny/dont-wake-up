/*
 * Author: Matěj Šťastný
 * Date created: 6/8/2025
 * GitHub link: https://github.com/matysta/dont-wake-up
 */

using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MenuButton : MonoBehaviour
{
    private MenuManager _menuManager;

    private void Start()
    {
        _menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
    }

    private void OnMouseUp()
    {
        Global.Log("Start button clicked");
        _menuManager.SwitchToGame();
    }
}
