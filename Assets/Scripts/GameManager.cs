using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("QUESTIONS AND ANSWERS")]
    [SerializeField] [TextArea] private string[] questions;
    [SerializeField] private float[] correctAnswers;
    [SerializeField] [TextArea] private string winText;

    [Header("GAMEOBJECTS")]
    [SerializeField] private TextMeshProUGUI[] answerTexts;
    [SerializeField] private TextMeshProUGUI questionText, scoreText, timerText;
    [SerializeField] private Button[] answerButtonsbuttons;
    [SerializeField] private GameObject answerPanel, fade;
    [SerializeField] private Animator fadeAnimator;

    [Header("SOUND")]
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private AudioClip selectSound;

    private float volume, timer, lastTime;
    private int questionNumber, score, correctButton;
    private bool gameOngoing;

    private void Start()
    {
        volume = PlayerPrefs.GetFloat("volume", 0.5f);
        soundManager.bgm.volume = volume;
        StartCoroutine("StartGame");
    }
    private IEnumerator StartGame()
    {
        StartCoroutine("SetNewTexts");
        fade.SetActive(true);
        fadeAnimator.SetBool("reverse", true);
        yield return new WaitForSeconds(1f);
        fade.SetActive(false);
        yield return new WaitForSeconds(0.1f);
    }
    private void Update()
    {
        if (gameOngoing)
        {
            timer += Time.deltaTime;
            float time = (float)Math.Round(timer, 1);
            timerText.SetText("Temps :  " + time);
            if ((int)time == time) timerText.text += ".0";
        }
    }
    public void SubmitAnswer(int button)
    {
        soundManager.playSound(selectSound, volume);
        if (correctButton == button) //If answered question correctly
        {
            float timeDifference = timer - lastTime;
            score += 3 + (int)(Mathf.Clamp(7f - timeDifference, 0, 4));
            lastTime = timer;
            questionNumber++;
            if (questionNumber == 10) //If finished answering questions
            {
                gameOngoing = false;
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
                StartCoroutine("SetNewTexts");
            }
        }
        else //If didn't answer correctly
        {
            score -= 2;
            scoreText.SetText("Score :  " + score);
        }
    }
    private IEnumerator SetNewTexts()
    {
        gameOngoing = false;
        foreach (Button b in answerButtonsbuttons)
        {
            b.enabled = false;
        }

        int i = 0, rand;
        correctButton = -1;

        foreach (TextMeshProUGUI text in answerTexts)
        {
            if(i == 3 && correctButton == -1)
            {
                rand = 0;
            }
            else if(correctButton != -1)
            {
                rand = Random.Range(1, 4);
            }
            else
            {
                rand = Random.Range(0, 4);
            }
            switch (rand)
            {
                case 0:
                    text.SetText(correctAnswers[i].ToString());
                    correctButton = i;
                    break;
                case 1:
                    text.SetText((correctAnswers[i] * Random.Range(1, 2) + 2).ToString());
                    break;
                case 2:
                    text.SetText((correctAnswers[i] / Random.Range(1, 2) + (Random.Range(-3, 2))).ToString());
                    break;
                case 3:
                    text.SetText((correctAnswers[i] + (Random.Range(1, 2) + 2)).ToString());
                    break;
            }
            i++;
        }

        scoreText.SetText("Score :  " + score);
        questionText.SetText(questions[questionNumber]);

        yield return new WaitForSeconds(0.2f);

        foreach (Button b in answerButtonsbuttons)
        {
            b.enabled = true;
        }
        gameOngoing = true;
    }
}
