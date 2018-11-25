using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;


public class ConsoleController : MonoBehaviour
{
    public Text text;
    LevelController levelController;
    private Text instructionText, instructionTextOld, instructionHeaderText, statusText;
    public Text code;
    private Text scoreText;
    private int instructionCount;

    [SerializeField]
    private GameObject textCodePlayer, textCodeSolution;

    [SerializeField]
    private Text instructionHeaderText2, instructionBodyText2;

    private ArrayList allCode;
    private string[] positiveAffirmations = new string[] { "Great work!", "Excellent!", "Order Up!", "Smooth Server!", "Sweetener and Cream!" };
    public GameObject PanelInfo;

    public bool condensedCode;

    [SerializeField] 
    private GameObject PanelPopUp, HeaderPopUp, BodyPopUp, TotalStars, TotalGold;

    [SerializeField]
    private GameObject Star1Fill, Star2Fill, Star3Fill;

    private string codeString;

    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();

        statusText = GameObject.Find("/canvasHUD2/PanelHUD/PanelCode/Text").GetComponent<Text>();
        allCode = new ArrayList();
      //  code = GameObject.Find("/canvasHUD2/PanelHUD/PanelCode/Text").GetComponent<Text>();
        scoreText = GameObject.Find("/canvasHUD2/PanelHUD/ImageScore/Score").GetComponent<Text>();
        text.text = "";
        statusText.text = "";
      //  code.text = "";
        codeString = "";
        instructionCount = 0;


        condensedCode = false;

        levelController = GameObject.Find("/TheLevel").GetComponent<LevelController>();
    }

    void updateScoreText()
    {
        scoreText.text = instructionCount.ToString() + " / " + levelController.Current.par.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelController.Current != null)
        {
            text.text = levelController.Current.collected;
        }
    }

    public void LevelStart()
    {

        if ((levelController.Current.world == 0) && (levelController.Current.level == 0))
        {
            PanelInfo.SetActive(false);
        }
        else
        {
            PanelInfo.SetActive(true);
        }

        text.text = "";
        statusText.text = "";
       // code.text = "";
        codeString = "";
        instructionCount = 0;
        instructionHeaderText2.text = "World " + (levelController.Current.world + 1) + ", Level " + (levelController.Current.level + 1);

        instructionBodyText2.text = System.Text.RegularExpressions.Regex.Unescape(levelController.Current.instructions);
        allCode.Clear();
        updateScoreText();
    }

    public void LevelReload() {
        textCodePlayer.GetComponent<Text>().text = "";
        textCodeSolution.GetComponent<Text>().text = "";
    }

    public void LevelEnd()
    {
         StartCoroutine(DelayedLevelEnd());
    }

    IEnumerator DelayedLevelEnd() {
        yield return new WaitForSeconds(1.5f);

        int currentWorld = PlayerPrefs.GetInt("currentWorld");
        int currentLevel = PlayerPrefs.GetInt("currentLevel");

        PlayerPrefs.SetInt("achievedWorld", currentWorld);
        PlayerPrefs.SetInt("achievedLevel", currentLevel);

        PanelPopUp.SetActive(true);
        Star1Fill.SetActive(false);
        Star2Fill.SetActive(false);
        Star3Fill.SetActive(false);

        float tempPercent = levelController.Current.par / (instructionCount * 1.0f);
        int numStars;

        if (tempPercent >= 1) { numStars = 3; }
        else if (tempPercent >= .4) { numStars = 2; }
        else { numStars = 1; }

        StartCoroutine(showStars(numStars));

        // Save the numStars to PlayerPrefs
        int[] starsArray = new int[4];

        starsArray = PlayerPrefsX.GetIntArray("NumStars-World-" + currentWorld);
        starsArray[currentLevel] = numStars;
        PlayerPrefsX.SetIntArray("NumStars-World-" + currentWorld, starsArray);

        HeaderPopUp.GetComponent<Text>().text = positiveAffirmations[Random.Range(0, positiveAffirmations.Length)];
        instructionBodyText2.text = "";

    }

    IEnumerator ShowCode(bool single, bool checkEquality, ArrayList codeToShow, Text textField) {
        float pause = 0.5f;
        string allLinesSolution = "";
        int x = 0;
        foreach (string line in codeToShow)
        {
            if (levelController.Current.solutionSwap.Count > 0)
            {
                string newLine = line;
                foreach (string swapLine in levelController.Current.solutionSwap)
                {
                    string[] patterns = swapLine.Split(':');
                    newLine = newLine.Replace(patterns[0], patterns[1]);
                }
                allLinesSolution += newLine + "\n";
            }
            else { allLinesSolution += line + "\n"; }
            if (!single)
            { yield return new WaitForSeconds(pause); }
            textField.text = allLinesSolution;

            // if the lines match, give gold.
            if (x < allCode.Count && line == allCode[x].ToString())
            {
                TotalGold.GetComponent<Text>().text = (int.Parse(TotalGold.GetComponent<Text>().text) + 25).ToString();
            }
            x++;
        }
    }

    IEnumerator showStars(int numStars)
    {
        float pause = 0.85f;
        yield return new WaitForSeconds(pause);


        // get current # of stars earned and show it here
        int totalStars = PlayerPrefs.GetInt("totalStars");

        TotalStars.GetComponent<Text>().text = "" + totalStars;
        float tempGold = 0;
        float starGold = 100;

        switch (numStars)
        {
            case 1:
                Star1Fill.SetActive(true);
                tempGold = float.Parse(TotalGold.GetComponent<Text>().text) + starGold * .25f;
                PlayerPrefs.SetInt("totalGold", (int)tempGold);
                TotalGold.GetComponent<Text>().text = tempGold.ToString("N0");
                break;

            case 2:
                Star1Fill.SetActive(true);
                tempGold = float.Parse(TotalGold.GetComponent<Text>().text) + starGold * .25f;
                PlayerPrefs.SetInt("totalGold", (int)tempGold);
                TotalGold.GetComponent<Text>().text = tempGold.ToString("N0");

                yield return new WaitForSeconds(pause);
                Star2Fill.SetActive(true);
                tempGold = float.Parse(TotalGold.GetComponent<Text>().text) + starGold * .6f;
                PlayerPrefs.SetInt("totalGold", (int)tempGold);
                TotalGold.GetComponent<Text>().text = tempGold.ToString("N0");
                break;

            case 3:
                Star1Fill.SetActive(true);
                tempGold = float.Parse(TotalGold.GetComponent<Text>().text) + starGold * .25f;
                PlayerPrefs.SetInt("totalGold", (int)tempGold);
                TotalGold.GetComponent<Text>().text = tempGold.ToString("N0");
                yield return new WaitForSeconds(pause);

                Star2Fill.SetActive(true);
                tempGold = float.Parse(TotalGold.GetComponent<Text>().text) + starGold * .6f;
                PlayerPrefs.SetInt("totalGold", (int)tempGold);
                TotalGold.GetComponent<Text>().text = tempGold.ToString("N0");
                yield return new WaitForSeconds(pause);

                Star3Fill.SetActive(true);
                Star1Fill.SetActive(true);
                tempGold = float.Parse(TotalGold.GetComponent<Text>().text) + starGold;
                PlayerPrefs.SetInt("totalGold", (int)tempGold);
                TotalGold.GetComponent<Text>().text = tempGold.ToString("N0");
                break;

            default:
                break;
        }

        Text textPlayer = textCodePlayer.GetComponent<Text>();
        StartCoroutine(ShowCode(false, false, allCode, textPlayer));

        yield return new WaitForSeconds(pause*allCode.Count);

        Text textCode = textCodeSolution.GetComponent<Text>();
        StartCoroutine(ShowCode(false, true, levelController.Current.solutionCode, textCode));

    }

    public void updateAllCodePanel()
    {
      //  code.text = "";
        codeString = "";
        if (condensedCode)
        {
            // condense
            int count = allCode.Count;
            for (int i = 0; i < count; i++)
            {
                string s = (string)allCode[i];
                if (i + 1 < count)
                {
                    string next = (string)allCode[i + 1];
                    if (s.IndexOf('=') > 0 && next.IndexOf('=') > 0)
                    {
                        string[] sParts = s.Split('=');
                        string[] nextParts = next.Split('=');
                        string sTrimmed = sParts[0].Trim();
                        string nextTrimmed = nextParts[1].Trim().TrimEnd(';');
                        if (sTrimmed == nextTrimmed)
                        {
                            s = nextParts[0] + "=" + sParts[1];
                            i++;
                        }
                    }
                }
            //    code.text += s + "\n";
                codeString += s + "\n";
             }
        }
        else
        {
            foreach (string s in allCode)
            {
            //   code.text += s + "\n";
                codeString += s + "\n";
            }
        }
    }

    public void Status(string text)
    {
        ArrayList sArray = new ArrayList();
        sArray.Add(text);
        // Debug.Log("status text: " + text);
        StartCoroutine(ShowCode(true, false, sArray, statusText));

        allCode.Add(text);
        instructionCount++;
        updateScoreText();

        // not good for performance but for now this should be ok
        updateAllCodePanel();
    }

    public void SetCondensedCode(bool value)
    {
        condensedCode = value;
        updateAllCodePanel();
    }
}