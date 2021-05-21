using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    public int Score=0;
    public Text ScoreText;
    public Image ImageQuestion;

    public Text[] AnswerQuestion;
    public QuestiongList[] questions;
    QuestiongList currentQuestion;
    public Text EndGame;
    public Button[] answerBttns = new Button[4];

    public Button PlayGame;
    public Button Back;

    public Sprite[] TFicons = new Sprite[2];
    public Image TFicon;
    public Text TFtext;

    public AudioSource myFx;
    public AudioClip TrueAudio;
    public AudioClip FalseAudio;

    List <object> qList;
    
    int RandQuestion;


public void MouseUpAsButton()
{
    StartCoroutine(BackMenu());
}
IEnumerator BackMenu()
{
    ImageQuestion.GetComponent<Animator>().SetTrigger("Out");
    Back.interactable=false;
    //Back.GetComponent<Animator>().SetTrigger("Out");
    for (int i=0; i<answerBttns.Length;i++) answerBttns[i].gameObject.GetComponent<Animator>().SetTrigger("Out");
    ScoreText.GetComponent<Animator>().SetTrigger("Out");
    yield return new WaitForSeconds(1.5f);
    PlayGame.interactable = true;
     PlayGame.GetComponent<Animator>().SetTrigger("Out");
    yield break;
}
    public void OnClickPlay()
    {
           Score=0;   
           ScoreText.text = Score.ToString();
        if (!PlayGame.GetComponent<Animator>().enabled) 
        {
            PlayGame.GetComponent<Animator>().enabled=true;
            PlayGame.interactable = false;
        }
         else PlayGame.GetComponent<Animator>().SetTrigger("In");

         if (!Back.GetComponent<Animator>().enabled) 
            Back.GetComponent<Animator>().enabled=true;
             else Back.GetComponent<Animator>().SetTrigger("In");

        qList = new List<object>(questions);
        questionGenerate();
        if (!ImageQuestion.GetComponent<Animator>().enabled) ImageQuestion.GetComponent<Animator>().enabled=true;
        else ImageQuestion.GetComponent<Animator>().SetTrigger("In");
         if (!ScoreText.GetComponent<Animator>().enabled) ScoreText.GetComponent<Animator>().enabled=true;
        else ScoreText.GetComponent<Animator>().SetTrigger("In");
    }

    void questionGenerate()
    {
        Back.GetComponent<Button>().interactable = false;
        if (qList.Count > 0)
        {
        RandQuestion = Random.Range(0,qList.Count);
        currentQuestion = qList[RandQuestion] as QuestiongList;
        ImageQuestion.sprite = currentQuestion.question; 
        ImageQuestion.GetComponent<Animator>().SetTrigger("In");
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
            StartCoroutine(RestartGame());
        }
    }

IEnumerator RestartGame()
{
    ScoreText.gameObject.GetComponent<Animator>().SetTrigger("Out");
    yield return new WaitForSeconds(0.7f);
    if (!EndGame.GetComponent<Animator>().enabled) EndGame.GetComponent<Animator>().enabled=true;
    else EndGame.GetComponent<Animator>().SetTrigger("In");
    yield return new WaitForSeconds(1.5f);
    EndGame.GetComponent<Animator>().SetTrigger("Out");
    yield return new WaitForSeconds(1);
    PlayGame.interactable = true;
    PlayGame.GetComponent<Animator>().SetTrigger("Out");
    // Score =0;
    // ScoreText.text = Score.ToString();
    yield break;
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
    Back.interactable = true;
    yield break;
} 
    IEnumerator TrueOrFalse(bool check)
    {
        Back.interactable=false;
        for (int i=0; i < answerBttns.Length;i++) answerBttns[i].interactable = false;
        yield return new WaitForSeconds(0.5f);

        if (!ImageQuestion.GetComponent<Animator>().enabled) ImageQuestion.GetComponent<Animator>().enabled=true;
        else ImageQuestion.GetComponent<Animator>().SetTrigger("Out");
        for (int i=0; i<answerBttns.Length;i++) answerBttns[i].gameObject.GetComponent<Animator>().SetTrigger("Out");
        yield return new WaitForSeconds(1.5f);

         if (!TFicon.gameObject.activeSelf) TFicon.gameObject.SetActive(true);
             else TFicon.gameObject.GetComponent<Animator>().SetTrigger("In");


        if (check)
        {
            PlayTrueSound();
            TFicon.sprite = TFicons[0];
            TFtext.text= "Правильный ответ";
            yield return new WaitForSeconds(1);
            Score+=10;
            ScoreText.text=Score.ToString();
            TFicon.gameObject.GetComponent<Animator>().SetTrigger("Out");
            qList.RemoveAt(RandQuestion);
            questionGenerate();
            yield break;
        }
        else 
        {

            PlayFalseSound();
            TFicon.sprite = TFicons[1];
            TFtext.text= "Неправильный ответ";
            ScoreText.GetComponent<Animator>().SetTrigger("Out");
            yield return new WaitForSeconds(1);
            TFicon.gameObject.GetComponent<Animator>().SetTrigger("Out");
            Back.GetComponent<Animator>().SetTrigger("Out");
            PlayGame.interactable = true;
            PlayGame.GetComponent<Animator>().SetTrigger("Out");
        }
    }
public void AnswerButtons(int index)
    {
       if (AnswerQuestion[index].text.ToString() == currentQuestion.answers[0]) StartCoroutine(TrueOrFalse(true));
       else StartCoroutine(TrueOrFalse(false));
    }

public void PlayTrueSound()
{
    myFx.PlayOneShot(TrueAudio);
}
public void PlayFalseSound()
{
    myFx.PlayOneShot(FalseAudio);
}

}


[System.Serializable]
public class QuestiongList
{
    public Sprite question;
    public string[] answers = new string[4];
}
