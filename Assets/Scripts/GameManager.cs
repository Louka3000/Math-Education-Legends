using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] [TextArea] private string[] questions;
    [SerializeField] [TextArea] private string winText;
    [SerializeField] private TextMeshProUGUI questionText, scoreText;
    [SerializeField] private answerButtonsScript[] answerButtons;
    [SerializeField] private TextMeshProUGUI[] answerTexts;
    [SerializeField] private int[] correctButton;
    private int questionNumber, score;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private AudioClip selectSound;
    private float volume;
    [SerializeField] private GameObject answerPanel;

    private void Start()
    {
        volume = PlayerPrefs.GetFloat("volume", 0.5f);
        SetNewTexts();
    }
    public void SubmitAnswer(int button)
    {
        soundManager.playSound(selectSound, volume);
        if (correctButton[questionNumber] == button) //If answered question correctly
        {
            score += 5;
            questionNumber++;
            if (questionNumber == 10) //If answered all question correctly
            {
                Debug.Log("You won!!!");
                questionText.SetText(winText);
                answerPanel.SetActive(false);
            }
            else //else go to next question
            {
                SetNewTexts();
                Debug.Log("Yes :D");
            }
        }
        else //If didn't answer correctly
        {
            score--;
            scoreText.SetText("Score :  " + score);
            Debug.Log("No :(");
        }
    }
    private void SetNewTexts()
    {
        int i = 0;
        foreach (TextMeshProUGUI text in answerTexts)
        {
            text.SetText(answerButtons[i].answers[questionNumber]);
            i++;
        }
        scoreText.SetText("Score :  " + score);
        questionText.SetText(questions[questionNumber]);
    }
}
