using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text _healthText;
    [SerializeField] private Text _waveText;

    void Start()
    {
        Player.Current.OnDamageTaken += UpdateHealthText;
        StartCoroutine(UpdateWaveText());
    }

    void UpdateHealthText()
    {
        _healthText.text = "Health: " + Player.Current.Health.ToString("F0") + "/" + Player.Current.MaxHealth.ToString("F0");
    }

    IEnumerator UpdateWaveText()
    {
        while(true)
        {
            System.TimeSpan ts = System.TimeSpan.FromSeconds(EnemyController.Current.WaveTimer);
            var timer = ts.ToString("mm\\:ss");
            _waveText.text = string.Format("Wave: {0}\n<size=20>Next wave in {1}</size>", EnemyController.Current.Wave, timer);

            yield return new WaitForSeconds(0.15f);
        }
    }
}
