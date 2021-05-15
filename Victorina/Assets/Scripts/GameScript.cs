using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    public Text TextQuestion;

    public Text[] AnswerQuestion;
    public QuestiongList[] questions;
    QuestiongList currentQuestion;
    public GameObject headPanel;
    public Button[] answerBttns = new Button[4];

    public Button PlayGame;

    public Sprite[] TFicons = new Sprite[2];
    public Image TFicon;
    public Text TFtext;

    List <object> qList;
    
    int RandQuestion;

    public void OnClickPlay()
    {
        if (!PlayGame.GetComponent<Animator>().enabled) PlayGame.GetComponent<Animator>().enabled=true;
         else PlayGame.GetComponent<Animator>().SetTrigger("In");
        qList = new List<object>(questions);
        questionGenerate();
        if (!headPanel.GetComponent<Animator>().enabled) headPanel.GetComponent<Animator>().enabled=true;
        else headPanel.GetComponent<Animator>().SetTrigger("In");
    }

    void questionGenerate()
    {
        if (qList.Count > 0)
        {
        RandQuestion = Random.Range(0,qList.Count);
        currentQuestion = qList[RandQuestion] as QuestiongList;
        TextQuestion.text = currentQuestion.question;
        List <string> answers = new List<string>(currentQuestion.answers);
            for (int i=0; i < currentQuestion.answers.Length; i++)
            {
                int rand = Random.Range(0,answers.Count);
                AnswerQuestion[i].text = answers[rand];
                answers.RemoveAt(rand);
            }
                   StartCoroutine(AnimationButtons());
        }
        else 
        {
            print("Game over");
        }
    }

     IEnumerator AnimationButtons()
{
    yield return new WaitForSeconds(1);
    for (int i=0; i < answerBttns.Length;i++) answerBttns[i].interactable = false;
    int a=0;
    while (a < answerBttns.Length)
    {
        if (!answerBttns[a].gameObject.activeSelf) answerBttns[a].gameObject.SetActive(true);
        else answerBttns[a].gameObject.GetComponent<Animator>().SetTrigger("In");
        a++;
        yield return new WaitForSeconds(1);
    }
    for (int i=0; i < answerBttns.Length;i++) answerBttns[i].interactable = true;
    yield break;
} 
    IEnumerator TrueOrFalse(bool check)
    {
        for (int i=0; i < answerBttns.Length;i++) answerBttns[i].interactable = false;
        yield return new WaitForSeconds(1);

        if (!TextQuestion.GetComponent<Animator>().enabled) TextQuestion.GetComponent<Animator>().enabled=true;
        else TextQuestion.GetComponent<Animator>().SetTrigger("Out");

        yield return new WaitForSeconds(0.5f);

         if (!TFicon.gameObject.activeSelf) TFicon.gameObject.SetActive(true);
             else TFicon.gameObject.GetComponent<Animator>().SetTrigger("In");

        if (check)
        {
            TFicon.sprite = TFicons[0];
            TFtext.text= "Правильный ответ";
        }
        else 
        {
            TFicon.sprite = TFicons[1];
            TFtext.text= "Неправильный ответ";
        }
    }
public void AnswerButtons(int index)
    {
       if (AnswerQuestion[index].text.ToString() == currentQuestion.answers[0]) StartCoroutine(TrueOrFalse(true));
       else StartCoroutine(TrueOrFalse(false));
    //    qList.RemoveAt(RandQuestion);
    //    questionGenerate();
    }
}

[System.Serializable]
public class QuestiongList
{
    public string question;
    public string[] answers = new string[4];
}
