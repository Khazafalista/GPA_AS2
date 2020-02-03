using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject ruleMenu;

    public AudioSource bgmPlayer;
    public AudioClip bgm;

    static bool first = true;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Stage", 1);
    }

    private void Awake()
    {
        mainMenu.SetActive(true);
        ruleMenu.SetActive(false);
        if(!first)
        {
            bgmPlayer.time = PlayerPrefs.GetFloat("bgmTime");
            bgmPlayer.volume = PlayerPrefs.GetFloat("bgmVolume");
            bgmPlayer.loop = true;
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.time = 0.0f;
            bgmPlayer.loop = true;
            bgmPlayer.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseStage(int stage)
    {
        if(stage == 1)
        {
            PlayerPrefs.SetInt("Stage", 1);
        }
        else if(stage == 2)
        {
            PlayerPrefs.SetInt("Stage", 2);
        }

        //Debug.Log(PlayerPrefs.GetInt("Stage"));
    }

    public void OnClickedStartBtn()
    {
        Invoke("ShowRule", 1.0f);
    }

    public void OnClickedExitBtn()
    {
        StartCoroutine(AudioFadeOut(bgmPlayer, 1.0f, 0f));
        Invoke("QuitGame", 1.5f);
    }

    public void OnClickedOKBtn()
    {
        PlayerPrefs.SetFloat("bgmVolume", bgmPlayer.volume);
        PlayerPrefs.SetFloat("bgmTime", bgmPlayer.time + 1.0f);
        Debug.Log(PlayerPrefs.GetFloat("bgmTime"));
        PlayerPrefs.Save();

        StartCoroutine(AudioFadeOut(bgmPlayer, 1.0f, 0f));
        Invoke("StartGame", 1.5f);
    }

    public void OnChangedVolume(float _volume)
    {
        bgmPlayer.volume = _volume;
    }

    private void StartGame()
    {
        SceneManager.LoadSceneAsync("MainGame");
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void ShowRule()
    {
        mainMenu.SetActive(false);
        ruleMenu.SetActive(true);
    }

    IEnumerator AudioFadeOut(AudioSource audioSource, float duration, float targetVolume)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > targetVolume)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;

            yield return null;
        }
        if(targetVolume == 0)
        {
            audioSource.Stop();
        }
    }

    /*IEnumerator FadeIn(AudioSource audioSource, float duration, float targetVolume)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / duration;

            yield return null;
        }
    }*/
} 
