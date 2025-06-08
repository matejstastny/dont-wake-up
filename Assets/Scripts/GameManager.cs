using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI waveText;
    public GameObject enemyPrefab;
    public GameObject pauseScreen;
    public GameObject crosshair;

    private int _waveNumber;
    private bool _isPaused;

    private void Update()
    {
        if (GameObject.FindGameObjectsWithTag($"Enemy").Length <= 0)
        {
            SpawnWave();
        }
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;
        pauseScreen.SetActive(_isPaused);
        crosshair.SetActive(!_isPaused);
    }

    public bool IsPaused()
    {
        return _isPaused;
    }

    private void SpawnWave()
    {
        _waveNumber++;
        waveText.text = "Wave: " + _waveNumber;
        for (int i = 0; i < _waveNumber; i++)
        {
            Instantiate(enemyPrefab, GetRandomPosition(), enemyPrefab.transform.rotation);
        }
    }

    private Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(385f, 463f);
        float randomZ = Random.Range(446f, 522f);
        return new Vector3(randomX, 1.46f, randomZ);
    }
}
