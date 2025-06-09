/*
 * Author: Matěj Šťastný
 * Date created: 6/3/2025
 * GitHub link: https://github.com/matysta/dont-wake-up
 */

using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI waveText;
    public GameObject enemyPrefab;
    public GameObject pauseScreen;
    public GameObject hurtEffect;
    public GameObject crosshair;

    [Header("State")]
    private int _waveNumber;
    private bool _isPaused;
    
    // Update -------------------------------------------------------------------------------------------

    private void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
        {
            SpawnWave();
        }
    }
    
    // Accessors -----------------------------------------------------------------------------------------

    public bool IsPaused() => _isPaused;
    
    // Events -------------------------------------------------------------------------------------------

    public void TakeDamage()
    {
        hurtEffect.GetComponent<PostProcessingController>().TakeDamage();
    }
    
    public void TogglePause()
    {
        _isPaused = !_isPaused;
        pauseScreen.SetActive(_isPaused);
        crosshair.SetActive(!_isPaused);
    }

    private void SpawnWave()
    {
        _waveNumber++;
        waveText.text = $"Wave: {_waveNumber}";

        for (int i = 0; i < _waveNumber; i++)
        {
            Instantiate(enemyPrefab, GetRandomPosition(), enemyPrefab.transform.rotation);
        }
    }

    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(385f, 463f);
        float z = Random.Range(446f, 522f);
        return new Vector3(x, 1.46f, z);
    }
}
