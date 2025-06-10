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
    public GameObject[] tutorialTexts;
    public GameObject tutorialUI;
    public GameObject enemyPrefab;
    public GameObject powerUpPrefab;
    public GameObject pauseScreen;
    public GameObject gameOverScreen;
    public GameObject effects;
    public GameObject crosshair;

    [Header("State")]
    private PlayerController _player;
    private int _tutorialIndex = 0;
    private int _health = 100;
    private int _waveNumber;
    private bool _isPaused;
    
    // Start --------------------------------------------------------------------------------------------

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        TogglePause(true, false);
        Global.ToggleCursor(false);
        NextTutorial(true);
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

    public void NextTutorial(bool reset)
    {
        if (reset)
        {
            _tutorialIndex = 0;
            tutorialUI.SetActive(true);
            Global.ToggleCursor(true);
            ToggleHUD(false);
        }

        foreach (GameObject tutorial in tutorialTexts) tutorial.SetActive(false);
        if (_tutorialIndex >= tutorialTexts.Length)
        {
            EndTutorial();
        }
        else
        {
            tutorialTexts[_tutorialIndex].SetActive(true);
        }
        _tutorialIndex++;
    }

    public void EndTutorial()
    {
        tutorialUI.SetActive(false);
        Global.ToggleCursor(false);
        TogglePause(false, false);
        ToggleHUD(true);
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void TakeDamage()
    {
        if (_isPaused) return;
        effects.GetComponent<PostProcessingController>().HurtEffect();
        _health -= 25;
        SetHealth(_health);
        if (_health > 0) return;
        // Game Over events
        Global.ToggleCursor(true);
        TogglePause(true, false);
        ToggleHUD(false);
        gameOverScreen.SetActive(true);
        scoreText.text = "Score: " + (_waveNumber - 1);
        Global.Log("Game Over");
    }
    
    public void TogglePause(bool paused, bool showPauseScreen)
    {
        _player.ToggleMovement(!paused);
        _isPaused = paused;
        pauseScreen.SetActive(_isPaused && showPauseScreen);
        crosshair.SetActive(!_isPaused);
    }

    public void Heal()
    {
        _health += 25;
        effects.GetComponent<PostProcessingController>().HealEffect();
        if (_health > 100) _health = 100;
        SetHealth(_health);
    }

    private void SpawnWave()
    {
        if (GameObject.FindGameObjectsWithTag("PowerUp").Length == 0)
        {
            Vector3 spawnPos = GetRandomPosition();
            spawnPos.y = -1.4f;
            Quaternion spawnRot = Quaternion.Euler(
                powerUpPrefab.transform.rotation.eulerAngles.x,
                Random.Range(0f, 360f),
                powerUpPrefab.transform.rotation.eulerAngles.z
            );

            Instantiate(powerUpPrefab, spawnPos, spawnRot);
        }
        
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

    private void ToggleHUD(bool showHUD)
    {
        crosshair.SetActive(showHUD);
        foreach (GameObject health in healthBar) health.SetActive(showHUD);
        waveText.gameObject.SetActive(showHUD);
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
