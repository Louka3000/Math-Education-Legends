using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject title, settings, fade;
    [SerializeField] private TMP_Text versionText, highScoreText;
    [SerializeField] private Button[] buttonsTitle;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] Toggle fullScreenToggle;
    [SerializeField] private AudioClip selectSound;
    
    private SoundManager soundManager;
    private float volume = 0.5f;
    private readonly int volumeDivider = 25;

    private void Awake()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }
    private void Start()
    {
        versionText.SetText("Version  " + Application.version);

        // Loads settings or create them if they don't exist
        if (PlayerPrefs.HasKey("volume"))
        {
            volume = PlayerPrefs.GetFloat("volume");
        }
        else
        {
            PlayerPrefs.SetFloat("volume", volume);
        }
        volumeSlider.value = volume * volumeDivider;// to use whole numbers for the Unity slider
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
        if (PlayerPrefs.HasKey("highscore"))
        {
            highScoreText.SetText("Meilleur score  " + PlayerPrefs.GetInt("highscore"));
        }
        else
        {
            PlayerPrefs.SetInt("highscore", 0);
            highScoreText.SetText("Meilleur score  " + 0);
        }
    }
    public void StartGame() // Called when "JOUER" is pressed
    {
        soundManager.PlaySound(selectSound);
        foreach(Button b in buttonsTitle)
        {
            b.enabled = false;
        }
        StartCoroutine(nameof(LoadGame));
    }
    public void OpenSettings() // Called when "PARAMETRE" is pressed
    {
        soundManager.PlaySound(selectSound);
        title.SetActive(false);
        settings.SetActive(true);
    }
    public void CloseSettings() // Called when "RETOUR" is pressed
    {
        soundManager.PlaySound(selectSound);
        title.SetActive(true);
        settings.SetActive(false);
    }
    public void LeaveGame() // Called when "QUITTER' is pressed
    {
        soundManager.PlaySound(selectSound);
        StartCoroutine(nameof(CloseApplication));
    }

    public void ChangeVolume() // Called when the volume slider's value changes
    {
        volume = volumeSlider.value / volumeDivider; // Slider goes from 0 to 10
        soundManager.UpdateVolume(volume);
        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.Save();
    }
    public void ToggleFullscreen() // Called when the checkmark for toggling fullscreen is toggled
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
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Game");
    }
    private IEnumerator CloseApplication()
    {
        fade.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        Application.Quit();
    }
}
