/*
 * Author: Matěj Šťastný
 * Date created: 6/3/2025
 * GitHub link: https://github.com/matysta/dont-wake-up
 */

using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("References")] 
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI scoreText;
    public GameObject[] healthBar;
    public GameObject enemyPrefab;
    public GameObject pauseScreen;
    public GameObject gameOverScreen;
    public GameObject damageEffect;
    public GameObject crosshair;

    [Header("State")]
    private PlayerController _player;
    private int _health = 100;
    private int _waveNumber;
    private bool _isPaused;
    
    // Start --------------------------------------------------------------------------------------------

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Global.ToggleCursor(false);
    }

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

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void TakeDamage()
    {
        if (_isPaused) return;
        damageEffect.GetComponent<PostProcessingController>().TakeDamage();
        _health -= 25;
        SetHealth(_health);
        if (_health > 0) return;
        // Game Over events
        TogglePause(true, false);
        gameOverScreen.SetActive(true);
        scoreText.text = "Score: " + (_waveNumber - 1);
        waveText.alignment = TextAlignmentOptions.Center;
        waveText.gameObject.SetActive(false);
        Global.ToggleCursor(true);
        Global.Log("Game Over");
    }
    
    public void TogglePause(bool paused, bool showPauseScreen)
    {
        _player.ToggleMovement(!paused);
        _isPaused = paused;
        pauseScreen.SetActive(_isPaused && showPauseScreen);
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
    
    // Private ------------------------------------------------------------------------------------------

    private Vector3 GetRandomPosition()
    {
        float minDistance = 1.5f;
        Vector3 playerPos = _player.transform.position;
        Vector3 randomPos;

        do
        {
            float x = Random.Range(385f, 463f);
            float z = Random.Range(446f, 522f);
            randomPos = new Vector3(x, 1.46f, z);
        } while (Vector2.Distance(new Vector2(randomPos.x, randomPos.z), new Vector2(playerPos.x, playerPos.z)) < minDistance);

        return randomPos;
    }

    private void SetHealth(int percent)
    {
        if (percent > 100 || percent < 0) return;

        int totalPoints = healthBar.Length;
        int healthNum = (int)Math.Round((double)totalPoints * percent / 100);

        foreach (GameObject healthPoint in healthBar) healthPoint.SetActive(false);

        for (int i = 0; i < healthNum; i++)
        {
            healthBar[i].SetActive(true); // Use array indexing directly
        }
    }
}
