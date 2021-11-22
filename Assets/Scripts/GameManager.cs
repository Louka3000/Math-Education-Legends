using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

public class GameManager : MonoBehaviour
{
    [SerializeField] [TextArea] private string[] questions;
    [SerializeField] private float[] answers;
    [SerializeField] TMP_Text questionText;
    [SerializeField] TMP_InputField answerInputField;

    private int questionNumber;

    private void Start()
    {
        questionText.SetText(questions[0]);
    }
    public void SubmitAnswer() //Called when the "CONFIRMER" button is pressed
    {
        if (float.Parse(answerInputField.text) == answers[questionNumber])
        {
            questionNumber++;
            questionText.SetText(questions[questionNumber]);
            answerInputField.text = "";
            Debug.Log("Yes :D");
        }
        else
        {
            Debug.Log("No :(");
        }
    }
}
