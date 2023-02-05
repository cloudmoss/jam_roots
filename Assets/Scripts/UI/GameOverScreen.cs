using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public static GameOverScreen Current { get; private set; }

    [SerializeField] private GameObject[] _showAfterAnimation;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Button _mainMenuButton;

    private Image _bg;

    private void Awake() {
        Current = this;
        _bg = GetComponent<Image>();

        foreach (var obj in _showAfterAnimation)
        {
            obj.SetActive(false);
        }

        _mainMenuButton.onClick.AddListener(() => {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        });
        _mainMenuButton.gameObject.SetActive(false);
    }

    private void Start() {
        Player.Current.OnDeath += Trigger;
        gameObject.SetActive(false);
    }

    public void Trigger()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        StartCoroutine(Show());
        _scoreText.text = "0";
        _bg.color = Color.clear;
    }

    IEnumerator Show()
    {
        Debug.Log("Showing game over screen");

        gameObject.SetActive(true);
        float t = 0;
        while (t < 1)
        {
            t += (Time.deltaTime * 0.66f);
            transform.localScale = Vector3.one * BounceEasing(t, 0, 1, 1);
            yield return null;
        }

        t = 0;
        while (t < 1)
        {
            t += (Time.deltaTime / 4f);
            _bg.color = Color.Lerp(Color.clear, Color.black, t * t);
            yield return null;
        }

        foreach (var obj in _showAfterAnimation)
        {
            yield return new WaitForSeconds(1f);
            obj.SetActive(true);
        }

        var scoreDisplay = 0;
        var score = EnemyController.Current.score;

        while (scoreDisplay < score)
        {
            scoreDisplay += Mathf.Max(1, (score - scoreDisplay) / 10);
            _scoreText.text = scoreDisplay.ToString();
            
            t = 0f;

            _scoreText.transform.localScale = Vector3.one * 1.3f;

            while (t < 1)
            {
                t += (Time.deltaTime * 10f);
                _scoreText.transform.localScale = Vector3.Lerp(Vector3.one * 1.3f, Vector3.one, t * t);
                yield return null;
            }
        }

        yield return new WaitForSeconds(1f);

        _mainMenuButton.gameObject.SetActive(true);

    }

    // t = current time
    // b = start value
    // c = change in value
    // d = duration
    float BounceEasing(float t, float b, float c, float d)
    {
        if ((t /= d) < (1 / 2.75f))
        {
            return c * (7.5625f * t * t) + b;
        }
        else if (t < (2 / 2.75f))
        {
            return c * (7.5625f * (t -= (1.5f / 2.75f)) * t + .75f) + b;
        }
        else if (t < (2.5 / 2.75))
        {
            return c * (7.5625f * (t -= (2.25f / 2.75f)) * t + .9375f) + b;
        }
        else
        {
            return c * (7.5625f * (t -= (2.625f / 2.75f)) * t + .984375f) + b;
        }
    }
}
