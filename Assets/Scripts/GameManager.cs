/*
 * Author: Matěj Šťastný
 * Date created: 6/3/2025
 * GitHub link: https://github.com/matysta/dont-wake-up
 */

using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    // Constants ----------------------------------------------------------------------------------------

    private const int MaxHealth = 100;
    private const int DamageAmount = 25;
    private const int HealAmount = 25;
    private const float PowerUpSpawnChance = 0.8f; // 0 to 1
    private const float PowerUpYPos = -1.4f;
    private const float MinEnemySpawnDistance = 3f;

    private static readonly Vector2 EnemySpawnXRange = new Vector2(385f, 463f);
    private static readonly Vector2 EnemySpawnZRange = new Vector2(446f, 522f);

    // References ---------------------------------------------------------------------------------------

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
    private int _health = MaxHealth;
    private int _tutorialIndex;
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
        if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0 && !_isPaused)
        {
            SpawnWave();
        }
    }

    // Accessors ----------------------------------------------------------------------------------------

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

        foreach (GameObject tutorial in tutorialTexts)
            tutorial.SetActive(false);

        if (_tutorialIndex >= tutorialTexts.Length)
        {
            EndTutorial();
        }
        else
        {
            tutorialTexts[_tutorialIndex].SetActive(true);
            _tutorialIndex++;
        }
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
        _health -= DamageAmount;
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
        _health += HealAmount;
        if (_health > MaxHealth) _health = MaxHealth;

        effects.GetComponent<PostProcessingController>().HealEffect();
        SetHealth(_health);
    }

    private void SpawnWave()
    {
        if (GameObject.FindGameObjectsWithTag("PowerUp").Length == 0 && Random.value < PowerUpSpawnChance)
        {
            Vector3 spawnPos = GetRandomPosition();
            spawnPos.y = PowerUpYPos;

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
        Vector3 playerPos = _player.transform.position;
        Vector3 randomPos;

        do
        {
            float x = Random.Range(EnemySpawnXRange.x, EnemySpawnXRange.y);
            float z = Random.Range(EnemySpawnZRange.x, EnemySpawnZRange.y);
            randomPos = new Vector3(x, 1.46f, z);
        } 
        while (Vector2.Distance(new Vector2(randomPos.x, randomPos.z), new Vector2(playerPos.x, playerPos.z)) < MinEnemySpawnDistance);

        return randomPos;
    }

    private void ToggleHUD(bool showHUD)
    {
        crosshair.SetActive(showHUD);
        foreach (GameObject health in healthBar) 
            health.SetActive(showHUD);

        waveText.gameObject.SetActive(showHUD);
    }

    private void SetHealth(int percent)
    {
        if (percent > MaxHealth || percent < 0) return;

        int totalPoints = healthBar.Length;
        int activeHealth = Mathf.RoundToInt(totalPoints * percent / 100f);

        for (int i = 0; i < totalPoints; i++)
            healthBar[i].SetActive(i < activeHealth);
    }
}
