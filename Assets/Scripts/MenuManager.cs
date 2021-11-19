using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private GameObject title, settings, fade;
    [SerializeField] private TMP_Text versionText;
    [SerializeField] private Button[] buttonsTitle;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] Toggle fullScreenToggle;
    [SerializeField] private AudioClip selectSound;

    private float volume = 0.5f;

    private void Start()
    {
        versionText.text = Application.version;
        if (PlayerPrefs.HasKey("volume"))
        {
            volume = PlayerPrefs.GetFloat("volume");
        }
        else
        {
            PlayerPrefs.SetFloat("volume", volume);
        }
        volumeSlider.value = volume * 10;// *10 pour utiliser l'arrondissement des sliders Unity.
        if (PlayerPrefs.HasKey("fullscreen"))
        {
            if (PlayerPrefs.GetInt("fullscreen") == 1)
            {
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            }
            else
            {
                Screen.fullScreenMode = FullScreenMode.Windowed;
            }
        }
        else
        {
            PlayerPrefs.SetInt("fullscreen", 1);
        }
    }
    public void StartGame()
    {
        soundManager.playSound(selectSound, volume);
        foreach(Button b in buttonsTitle)
        {
            b.enabled = false;
        }
        StartCoroutine("LoadGame");
    }
    public void OpenSettings()
    {
        soundManager.playSound(selectSound, volume);
        title.SetActive(false);
        settings.SetActive(true);
    }
    public void CloseSettings()
    {
        soundManager.playSound(selectSound, volume);
        title.SetActive(true);
        settings.SetActive(false);
    }
    public void LeaveGame()
    {
        soundManager.playSound(selectSound, volume);
        StartCoroutine("CloseApplication");
    }
    public void ChangeVolume()
    {
        volume = volumeSlider.value / 10;// /10 parce que le slider va de 0 � 10 et on veut une valeur de 0 � 1.
        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.Save();
    }
    public void ToggleFullscreen()
    {
        if (fullScreenToggle.isOn)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            PlayerPrefs.SetInt("fullscreen", 1);
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            PlayerPrefs.SetInt("fullscreen", 0);
        }
        PlayerPrefs.Save();
    }
    private IEnumerator LoadGame()
    {
        fade.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadScene("Game");
    }
    private IEnumerator CloseApplication()
    {
        fade.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        Application.Quit();
    }
}
