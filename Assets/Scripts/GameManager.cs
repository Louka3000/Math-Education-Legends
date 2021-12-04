using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    [Header("QUESTIONS AND ANSWERS")]
    [SerializeField] [TextArea] private string[] questions;
    [SerializeField] [TextArea] private string winText;
    [SerializeField] private int[] correctButton;

    [Header("GAMEOBJECTS")]
    [SerializeField] private answerButtonsScript[] answerButtons;
    [SerializeField] private TextMeshProUGUI questionText, scoreText, timerText;
    [SerializeField] private TextMeshProUGUI[] answerTexts;
    [SerializeField] private GameObject answerPanel, fade;
    [SerializeField] private Animator fadeAnimator;

    [Header("SOUND")]
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private AudioClip selectSound;

    private float volume, timer, lastTime;
    private int questionNumber, score;
    private bool gameOngoing;

    private void Start()
    {
        volume = PlayerPrefs.GetFloat("volume", 0.5f);
        StartCoroutine("StartGame");
    }
    private IEnumerator StartGame()
    {
        SetNewTexts();
        fade.SetActive(true);
        fadeAnimator.SetBool("reverse", true);
        yield return new WaitForSeconds(0.6f);
        fade.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        gameOngoing = true;
    }
    private void Update()
    {
        if (gameOngoing)
        {
            timer += Time.deltaTime;
            timerText.SetText("Temps :  " + Math.Round(timer, 2));
        }
    }
    public void SubmitAnswer(int button)
    {
        soundManager.playSound(selectSound, volume);
        if (correctButton[questionNumber] == button) //If answered question correctly
        {
            float timeDifference = timer - lastTime;
            score += 3 + (int)(Mathf.Clamp(7f - timeDifference, 0, 4));
            lastTime = timer;
            questionNumber++;
            if (questionNumber == 10) //If finished answering questions
            {
                gameOngoing = false;
                Debug.Log("You won!!!");
                questionText.SetText(winText);
                answerPanel.SetActive(false);
                if (score > PlayerPrefs.GetInt("highscore"))//if the new score is higher than the highscore
                {
                    PlayerPrefs.SetInt("highscore", score);
                    PlayerPrefs.Save();
                }
                //TODO : make a high score announcement text
            }
            else //else go to next question
            {
                SetNewTexts();
                Debug.Log("Yes :D");
            }
        }
        else //If didn't answer correctly
        {
            score -= 2;
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
