using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject squarePrefab;

    [SerializeField]
    private Transform squarePanel;

    [SerializeField]
    private Transform questionPanel;

    [SerializeField]
    private Text questionText;

    [SerializeField]
    private Sprite[] squareSprites;


    private GameObject[] squaresArray = new GameObject[25];

    List<int> levelvalueslist = new List<int>();
    int bolunenSayi, bolenSayi;
    int kacinciSoru;
    int buttonValue;
    int trueResult;

    bool isButtonClicked;

    int kalanHak;

    string questionDifficulty;


    KalanHaklarManager kalanHaklarManager;
    ScoreManager scoreManager;
    GameObject currentSquare;


    private void Awake()
    {
        kalanHak = 3;

        kalanHaklarManager = Object.FindObjectOfType<KalanHaklarManager>();
        scoreManager = Object.FindObjectOfType<ScoreManager>();

        kalanHaklarManager.HakKontrol(kalanHak);
    }


    void Start()
    {
        isButtonClicked = false;
        questionPanel.GetComponent<RectTransform>().localScale = Vector3.zero;
        createSquare();
    }


    public void createSquare()
    {
        for(int i=0; i < 25; i++)
        {
            GameObject square = Instantiate(squarePrefab, squarePanel);
            square.transform.GetChild(1).GetComponent<Image>().sprite = squareSprites[Random.Range(0, squareSprites.Length)];
            square.transform.GetComponent<Button>().onClick.AddListener(() => ButtonClicked());
            squaresArray[i] = square;
        }


        writeLevelValuesToText();
        StartCoroutine(DoFadeRoutine());
        Invoke("openQuestionPanel", 3f);
    }

    void ButtonClicked()
    {
        if(isButtonClicked)
        {
            buttonValue = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text);
            Debug.Log(buttonValue);
            currentSquare = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            ResultControl();
            
        }

    }

    void ResultControl()
    {
        if(buttonValue == trueResult)
        {
            currentSquare.transform.GetChild(1).GetComponent<Image>().enabled = true;
            currentSquare.transform.GetChild(0).GetComponent<Text>().text = "";
            currentSquare.transform.GetComponent<Button>().interactable = false;

            scoreManager.AddPoýnt(questionDifficulty);

            levelvalueslist.RemoveAt(kacinciSoru);

            if (levelvalueslist.Count > 0)
            {
                openQuestionPanel();
            }
            else
            {
                Debug.Log("Oyun bitti");
            }


        }
        else
        {
            kalanHak-=1;
            kalanHaklarManager.HakKontrol(kalanHak);
        }
        if(kalanHak<=0)
        {
            Debug.Log("Oyun bitti");
        }
    }

    IEnumerator DoFadeRoutine()
    {
        foreach(var square in squaresArray)
        {
            square.GetComponent<CanvasGroup>().DOFade(1, 0.2f);

            yield return new WaitForSeconds(0.07f);
        }
    }

    void writeLevelValuesToText()
    {
        foreach (var square in squaresArray)
        {
            int randomValue = Random.Range(1, 13);
            levelvalueslist.Add(randomValue);
            square.transform.GetChild(0).GetComponent<Text>().text = randomValue.ToString();
        }
    }

    void openQuestionPanel()
    {
        isButtonClicked = true;
        askQuestion();
        questionPanel.GetComponent<RectTransform>().DOScale(1, 0.3f).SetEase(Ease.OutBack);
    }

    void askQuestion()
    {
        bolenSayi = Random.Range(2, 11);
        kacinciSoru = Random.Range(0, levelvalueslist.Count);
        trueResult = levelvalueslist[kacinciSoru];
        bolunenSayi = bolenSayi * trueResult;

        if (bolunenSayi < 40)
        {
            questionDifficulty = "easy";
        }else if(bolunenSayi>40 && bolunenSayi <= 80)
        {
            questionDifficulty = "medium";
        }
        else
        {
            questionDifficulty = "hard";
        }

        questionText.text = bolunenSayi.ToString() + " : " + bolenSayi.ToString();

    }


}
