using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI waveText;
    public GameObject enemyPrefab;

    private int _waveNumber;
    
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag($"Enemy").Length <= 0)
        {
            SpawnWave();
        }
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
