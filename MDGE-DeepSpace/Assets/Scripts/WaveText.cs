using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveText : MonoBehaviour
{

    private TMPro.TextMeshProUGUI waveText;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        if (!waveText)
        {
            waveText = GetComponent<TMPro.TextMeshProUGUI>();
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        waveText.text = "Wave " + WaveManager.CurrentWaveNumber;
    }
}
