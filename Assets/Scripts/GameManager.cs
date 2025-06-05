using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Game Started");
    }

    void Update()
    {
        if (GameObject.FindGameObjectsWithTag($"Enemy").Length <= 0)
        {
            
        }
    }
}