using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private static int stage;

    public int maxScore = 50;

    public int getScore = 5;

    public AudioSource bgmPlayer;
    public AudioClip bgm01;

    public Text ScoreText;
    public Text ResultText;

    [SerializeField]
    int score;

    bool endGame;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(stage);
        score = 0;
        ScoreText.text = "<b>Score: " + score + "</b>";

        stage = PlayerPrefs.GetInt("Stage");
        Debug.Log("Start Stage: " + stage);

        endGame = false;
    }

    private void Awake()
    {
        bgmPlayer.time = PlayerPrefs.GetFloat("bgmTime");
        bgmPlayer.volume = PlayerPrefs.GetFloat("bgmVolume");
        bgmPlayer.loop = true;
        bgmPlayer.Play();

        ResultText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetStage()
    {
        return stage;
    }

    public void GetScore()
    {
        score += getScore;
        ScoreText.text = "<b>Score: " + score + "</b>";

        if (score >= maxScore)
        {
            GameClear();
        }
    }

    public void GameClear()
    {
        if(!endGame)
        {
            ResultText.enabled = true;
            ResultText.text = "<b>Victory</b>";
            if (stage == 1)
            {
                stage = 2;

                StartCoroutine(AudioFadeOut(bgmPlayer, 1.0f, 0f));
                Invoke("StartGame", 3f);
            }
            else if (stage == 2)
            {
                StartCoroutine(AudioFadeOut(bgmPlayer, 1.0f, 0f));
                Invoke("BackToMenu", 3f);
            }

            endGame = true;

            PlayerPrefs.SetInt("Stage", stage);
            PlayerPrefs.SetFloat("bgmTime", bgmPlayer.time + 1.0f);
            PlayerPrefs.SetFloat("bgmVolume", bgmPlayer.volume);
            PlayerPrefs.Save();
        }
    }

    public void GameLost()
    {
        if(!endGame)
        {
            ResultText.enabled = true;
            ResultText.text = "<b>Defeat</b>";
            PlayerPrefs.SetInt("Stage", stage);
            PlayerPrefs.SetFloat("bgmTime", bgmPlayer.time + 1.0f);
            PlayerPrefs.SetFloat("bgmVolume", bgmPlayer.volume);
            Debug.Log(PlayerPrefs.GetFloat("bgmVolume"));
            PlayerPrefs.Save();

            StartCoroutine(AudioFadeOut(bgmPlayer, 1.0f, 0f));
            Invoke("StartGame", 2.5f);

            endGame = true;
        }
    }

    private void StartGame()
    {
        SceneManager.LoadSceneAsync("MainGame");
    }

    private void BackToMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    IEnumerator AudioFadeOut(AudioSource audioSource, float duration, float targetVolume)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > targetVolume)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;

            yield return null;
        }
        if (targetVolume == 0)
        {
            audioSource.Stop();
        }
    }
}
