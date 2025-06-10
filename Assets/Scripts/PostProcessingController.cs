/*
 * Author: Matěj Šťastný
 * Date created: 6/8/2025
 * GitHub link: https://github.com/matysta/dont-wake-up
 */

using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingController : MonoBehaviour
{
    [Header("References")]
    private PostProcessVolume _volume;
    private Vignette  _vignette;
    
    [Header("State")]
    private float _intensity;
    
    // Start --------------------------------------------------------------------------------------------
    
    private void Start()
    {
        _volume = GetComponent<PostProcessVolume>();
        _volume.profile.TryGetSettings(out _vignette);

        if (!_vignette)
        {
            Debug.LogError("PostProcessVolume not found");
        }
        else
        {
            _vignette.enabled.Override(false);
        }
    }
    
    // Controllers --------------------------------------------------------------------------------------

    public void HurtEffect()
    {
        PlayEffect(Color.red);
    }
    
    public void HealEffect()
    {
        PlayEffect(Color.green);
    }
    
    // Private --------------------------------------------------------------------------------------
    
    private void PlayEffect(Color c)
    {
        StartCoroutine(EffectRoutine(c));
    }
    
    private IEnumerator EffectRoutine(Color c)
    {
        _intensity = 0.5f;
        _vignette.enabled.Override(true);
        _vignette.intensity.Override(_intensity);
        _vignette.color.Override(c);
        yield return new WaitForSeconds(0.4f);

        while (_intensity > 0)
        {
            _intensity -= 0.01f;
            if (_intensity < 0) _intensity = 0;
            _vignette.intensity.Override(_intensity);
            yield return new WaitForSeconds(0.07f);
        }
        
        _vignette.enabled.Override(false);
    }
}
