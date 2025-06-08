/*
 * Author: Matěj Šťastný
 * Date created: 6/4/2025
 * GitHub link: https://github.com/matysta/dont-wake-up
 */

using UnityEngine;

public class MouseController : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}