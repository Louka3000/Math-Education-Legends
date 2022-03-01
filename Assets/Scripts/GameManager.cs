using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("QUESTIONS & ANSWERS")]
    [SerializeField] [TextArea] private string[] questions;
    [SerializeField] private float[] correctAnswers;
    [SerializeField] [TextArea] private string winText;

    [Header("GAMEOBJECTS & COMPONENTS")]
    [SerializeField] private TextMeshProUGUI[] answerTexts;
    [SerializeField] private TextMeshProUGUI questionText, scoreText, timerText;
    [SerializeField] private Button[] answerButtons;
    [SerializeField] private GameObject answerPanel, fade, titleScreenButton;

    [Header("OTHER")]
    [SerializeField] private AudioClip selectSound;

    private Animator fadeAnimator;
    private SoundManager soundManager;
    private float timer, lastTime;
    private int questionNumber, score, correctButton, rand, index, textButton;
    private bool gameOngoing;

    private void Awake()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        fadeAnimator = fade.GetComponent<Animator>();
    }
    private void Start()
    {
        StartCoroutine(nameof(StartGame));
    }
    private IEnumerator StartGame()
    {
        StartCoroutine(nameof(SetNewTexts));
        fade.SetActive(true);
        fadeAnimator.SetBool("reverse", true);
        yield return new WaitForSeconds(1f);
        fade.SetActive(false);
        yield return new WaitForSeconds(0.1f);
    }
    private void Update()
    {
        // Timer
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
        if (correctButton == button) // If answered question correctly
        {
            StopCoroutine(nameof(TypeByLetter));
            float timeDifference = timer - lastTime;
            score += 3 + (int)(Mathf.Clamp(7f - timeDifference, 0, 4));
            UpdateScore();
            lastTime = timer;
            questionNumber++;
            if (questionNumber == questions.Length) // If finished answering questions
            {
                gameOngoing = false;
                questionText.SetText(winText);
                answerPanel.SetActive(false);
                if (score > PlayerPrefs.GetInt("highscore"))// If the new score is higher than the highscore
                {
                    PlayerPrefs.SetInt("highscore", score);
                    PlayerPrefs.Save();
                    questionText.text += Environment.NewLine + "Nouveau record : " + score;
                }
                titleScreenButton.SetActive(true);
            }
            else // else go to next question
            {
                StartCoroutine(nameof(SetNewTexts));
            }
        }
        else // If didn't answer correctly
        {
            score -= 3;
            UpdateScore();
        }
    }
    private IEnumerator SetNewTexts()
    {
        // Disables buttons and pauses game
        gameOngoing = false;
        foreach (Button b in answerButtons)
        {
            b.enabled = false;
        }

        // Sets answer buttons' texts
        correctButton = -1; index = 0; rand = 0;
        foreach (TextMeshProUGUI text in answerTexts)
        {
            if(index == 3 && correctButton == -1)
                rand = 0;
            else if (correctButton != -1)
                rand = Random.Range(1, 4);
            else
                rand = Random.Range(0, 4);

            switch (rand) // Random stuff to make 3 false answers kinda random...
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

        // Updates question text
        StartCoroutine(nameof(TypeByLetter));

        yield return new WaitForSeconds(0.2f);

        // Enables buttons and resumes game
        foreach (Button b in answerButtons)
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
            // TODO play sound effect
        }
    }

    public void GoToTitleScreen()
    {
        soundManager.PlaySound(selectSound);
        StartCoroutine(nameof(ToTitleScreen));
    }
    private IEnumerator ToTitleScreen()
    {
        fade.SetActive(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Title Screen");
    }
}
