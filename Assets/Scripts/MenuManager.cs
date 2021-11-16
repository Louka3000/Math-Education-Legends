using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private GameObject title, settings;
    [SerializeField] private Button[] buttonsTitle;

    [SerializeField] private AudioClip selectSound;
    private float volume = 0.5f;
    
    public void StartGame()
    {
        soundManager.playSound(selectSound, volume);
        foreach(Button b in buttonsTitle)
        {
            b.enabled = false;
        }
        Invoke("LoadGame", 1f);
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
        Invoke("CloseApplication", 1f);
    }

    private void LoadGame()
    {
        SceneManager.LoadScene(1);
    }
    private void CloseApplication()
    {
        Application.Quit();
    }
}
