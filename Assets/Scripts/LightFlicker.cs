using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{
    private const float MinFlickerInterval = 2f;
    private const float MaxFlickerInterval = 5f;
    private const int MinFlickers = 2;
    private const int MaxFlickers = 5;
    private const float FlickerSpeed = 0.08f;

    private Light _spotlight;
    private float _nextFlickerTime;

    private void Start()
    {
        _spotlight = GetComponent<Light>();
        ScheduleNextFlicker();
    }

    private void Update()
    {
        if (!(Time.time >= _nextFlickerTime)) return;
        StartCoroutine(FlickerBurst());
        ScheduleNextFlicker();
    }

    private void ScheduleNextFlicker()
    {
        _nextFlickerTime = Time.time + Random.Range(MinFlickerInterval, MaxFlickerInterval);
    }

    private IEnumerator FlickerBurst()
    {
        int flickerCount = Random.Range(MinFlickers, MaxFlickers + 1);
        for (int i = 0; i < flickerCount; i++)
        {
            _spotlight.enabled = !_spotlight.enabled;
            yield return new WaitForSeconds(FlickerSpeed);
        }
        _spotlight.enabled = true;
    }
}