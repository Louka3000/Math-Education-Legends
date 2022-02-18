using System.Collections;
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

    private float timer, lastTime;
    private int questionNumber, score, correctButton, rand, index, textButton;
    private bool gameOngoing;

    private void Start()
    {
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
        soundManager.PlaySound(selectSound);
        if (correctButton == button) //If answered question correctly
        {
            float timeDifference = timer - lastTime;
            score += 3 + (int)(Mathf.Clamp(7f - timeDifference, 0, 4));
            UpdateScore();
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
            score -= 3;
            UpdateScore();
        }
    }
    private IEnumerator SetNewTexts()
    {
        //disables buttons and pauses game
        gameOngoing = false;
        foreach (Button b in answerButtonsbuttons)
        {
            b.enabled = false;
        }

        //Sets answer buttons' texts
        correctButton = -1; index = 0; rand = 0;
        foreach (TextMeshProUGUI text in answerTexts)
        {
            if(index == 3 && correctButton == -1)
                rand = 0;
            else if (correctButton != -1)
                rand = Random.Range(1, 4);
            else
                rand = Random.Range(0, 4);

            switch (rand)
            {
                case 0:
                    textButton = (int)correctAnswers[questionNumber];
                    correctButton = index;
                    break;
                case 1:
                    textButton = (int)(correctAnswers[questionNumber] * Random.Range(1, 3) + Random.Range(-1, 3));
                    if (textButton == correctAnswers[questionNumber])
                        textButton--;
                    break;
                case 2:
                    textButton = (int)(correctAnswers[questionNumber] / 2 + Random.Range(1, 3));
                    if (textButton == correctAnswers[questionNumber])
                        textButton++;
                    break;
                case 3:
                    textButton = (int)(correctAnswers[questionNumber] + (Random.Range(-5, 2) - 1) * -1);
                    if (textButton == correctAnswers[questionNumber])
                        textButton++;
                    break;
            }
            text.SetText(textButton.ToString());
            index++;
        }

        //updates question text
        StopCoroutine("TypeByLetter");
        StartCoroutine("TypeByLetter");

        yield return new WaitForSeconds(0.2f);

        //enables buttons and resumes game
        foreach (Button b in answerButtonsbuttons)
        {
            b.enabled = true;
        }
        gameOngoing = true;
    }
    private void UpdateScore()
    {
        scoreText.SetText("Score :  " + score);
    }

    IEnumerator TypeByLetter()
    {
        questionText.text = "";
        foreach (char letter in questions[questionNumber])
        {
            yield return new WaitForSeconds(0.02f);
            questionText.text += letter;
        }
    }
}
