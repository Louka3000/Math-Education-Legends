using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class LetterByLetterText : MonoBehaviour
{
    [SerializeField] float letterPause = 0.02f;
    Text textComp;
    string text;
    private void Awake()
    {
        textComp = gameObject.GetComponent<Text>();
    }
    private void Start()
    {
        text = textComp.text;
        textComp.text = "";
        StartCoroutine("Type");
    }
    IEnumerator Type()
    {
        foreach(char letter in text)
        {
            yield return new WaitForSeconds(letterPause);
            textComp.text += letter;
        }
    }
}
