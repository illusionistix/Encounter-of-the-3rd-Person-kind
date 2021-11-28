using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;


public class StartManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundFXVolumeSlider;
    [SerializeField] private Slider ambienceVolumeSlider;
    [SerializeField] private RawImage brandImage;
    [SerializeField] private ParticleSystem startPs;

    //private bool isMusicStarting;


    /*
    private void Awake()
    {
        if (MainManager.Instance.isGameStarting)
        {
            isMusicStarting = true;
            StartCoroutine(MusicFadeInTimer());
        }
    }*/


    private void Start()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        startPs = startPs.GetComponent<ParticleSystem>();
        startPs.Play();

        /*if (MainManager.Instance == null)
        {
            musicVolumeSlider.value = 0f;
            soundFXVolumeSlider.value = 0.5f;
            ambienceVolumeSlider.value = 0.5f;

            isMusicStarting = true;
            
            StartCoroutine(MusicFadeInTimer());
        }
        else
        {*/
        musicVolumeSlider.value = MainManager.Instance.musicVolume;
        soundFXVolumeSlider.value = MainManager.Instance.soundFXVolume;
        ambienceVolumeSlider.value = MainManager.Instance.ambienceVolume;
        //}
        
    }

    private void Update()
    {
        /*
        if (isMusicStarting)
        {
            musicVolumeSlider.value += Time.deltaTime * 0.025f;
        }*/
        

        KeyToMainMenu();

        MainManager.Instance.musicVolume = musicVolumeSlider.value;
        MainManager.Instance.soundFXVolume = soundFXVolumeSlider.value;
        MainManager.Instance.ambienceVolume = ambienceVolumeSlider.value;
    }


    /*
    IEnumerator MusicFadeInTimer()
    {
        yield return new WaitForSeconds(20);

        isMusicStarting = false;
        musicVolumeSlider.value = 0.5f;
        MainManager.Instance.isGameStarting = false;
    }*/


    public void StartGame()
    {
        MainManager.Instance.LoadDataFromFile();

        //Debug.Log(MainManager.Instance.screenshotsPath);

        //musicVolumeSlider.value = 0.5f;
        MainManager.Instance.isGameStarting = false;
        MainManager.Instance.isGameActive = true;
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        MainManager.Instance.SaveDataToFile();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }


    private void KeyToMainMenu()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            mainMenu.SetActive(true);
            brandImage.color = new Color(brandImage.color.r, brandImage.color.g, brandImage.color.b, 0.2f);
            optionsMenu.SetActive(false);
        }
    }


    public void ButtonToMainMenu()
    {
        mainMenu.SetActive(true);
        brandImage.color = new Color(brandImage.color.r, brandImage.color.g, brandImage.color.b, 0.2f);
        optionsMenu.SetActive(false);
    }


    public void ButtonToOptionsMenu()
    {
        mainMenu.SetActive(false);
        brandImage.color = new Color(brandImage.color.r, brandImage.color.g, brandImage.color.b, 0f);
        optionsMenu.SetActive(true);
    }

}