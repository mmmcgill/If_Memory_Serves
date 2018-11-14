using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ConsoleController : MonoBehaviour {
    public Text text;
    LevelController levelController;
	private Text instructionText, instructionTextOld, instructionHeaderText, statusText;
    public Text code;
    private Text scoreText;
    private int instructionCount;
    private GameObject panelAllCode;
	[SerializeField]
	public Text instructionHeaderText2, instructionBodyText2;

  	private ArrayList allCode;
	private string[] positiveAffirmations = new string[] {"Great work!", "Excellent!", "Order Up!", "Smooth Server!", "Sweetener and Cream!"};
	public GameObject  PanelInfo;

  	public bool condensedCode;
	public GameObject PanelPopUp, HeaderPopUp, BodyPopUp;
    public GameObject star1, star2, star3;

  // Use this for initialization
  void Start() {
    text = GetComponent<Text>();
 
    statusText = GameObject.Find("/canvasHUD2/PanelHUD/PanelCode/Text").GetComponent<Text>();
    panelAllCode = GameObject.Find("/canvasHUD2/PanelHUD/PanelAllCode");
    allCode = new ArrayList();
    code = GameObject.Find("/canvasHUD2/PanelHUD/PanelAllCode/Text").GetComponent<Text>();
    scoreText = GameObject.Find("/canvasHUD2/PanelHUD/Score").GetComponent<Text>();
    text.text = "";
    statusText.text = "";
    code.text = "";
    instructionCount = 0;


    condensedCode = false;

    levelController = GameObject.Find("/TheLevel").GetComponent<LevelController>();
  }

  void updateScoreText() {
    scoreText.text = instructionCount.ToString() + " / " + levelController.Current.par.ToString();
  }

  // Update is called once per frame
  void Update() {
    if (levelController.Current != null) {
      text.text = levelController.Current.collected;
    }
  }

  public void LevelStart() {
		
	if ((levelController.Current.world == 0) && (levelController.Current.level == 0)) {
		PanelInfo.SetActive (false);
	} else {
		PanelInfo.SetActive (true);
	}

    text.text = "";
    statusText.text = "";
    code.text = "";
    instructionCount = 0;
	instructionHeaderText2.text = "World " + (levelController.Current.world+1) + ", Level " + (levelController.Current.level+1);

		instructionBodyText2.text = System.Text.RegularExpressions.Regex.Unescape(levelController.Current.instructions);
	//	instructionTextOld.text = instructionText.text;
    panelAllCode.SetActive(false);
    allCode.Clear();
    updateScoreText();
  }

    IEnumerator showStars(int numStars) {
        float pause = 1.0f;
        yield return new WaitForSeconds(pause);
        switch (numStars)
        {
            case 1:
                star1.SetActive(true);
                break;
            case 2:
                star1.SetActive(true);
                yield return new WaitForSeconds(pause);
                star2.SetActive(true);
                break;
            case 3:
                star1.SetActive(true);
                yield return new WaitForSeconds(pause);
                star2.SetActive(true);
                yield return new WaitForSeconds(pause);
                star3.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void LevelEnd() {
		PanelPopUp.SetActive(true);
        star1.SetActive(false);
        star2.SetActive(false);
        star3.SetActive(false);

        // Set stars, then save them to player prefs for this level
        Debug.Log("score text is " + scoreText.text);
        Debug.Log("par is " + levelController.Current.par);

        float tempPercent = levelController.Current.par / (instructionCount * 1.0f);
        int numStars;

        if (tempPercent >= 1) { numStars = 3; }
        else if (tempPercent >= .4) { numStars = 2; }
        else { numStars = 1; }

        StartCoroutine(showStars(numStars));

        // Save the numStars to PlayerPrefs
        int[] starsArray = new int[4];
        int currentWorld = PlayerPrefs.GetInt("currentWorld");
        int currentLevel = PlayerPrefs.GetInt("currentLevel");

        starsArray = PlayerPrefsX.GetIntArray("NumStars-World-" + currentWorld);
        for (int j = 0; j < 4; j++)
        {
            if (j==currentLevel) {
                starsArray[j] = numStars;
            }
            Debug.Log(j +": " +starsArray[j]);
        }
        PlayerPrefsX.SetIntArray("NumStars-World-" + currentWorld, starsArray);

        HeaderPopUp.GetComponent<Text>().text = positiveAffirmations[Random.Range(0,positiveAffirmations.Length)];
		instructionBodyText2.text = ""; //levelController.Current.world.ToString() + "-"
   			 //+ levelController.Current.level.ToString() + ": Press = for next level!";

    
    panelAllCode.SetActive(true);
  }

  public void updateAllCodePanel() {
    code.text = "";

    if (condensedCode) {
      // condense
      int count = allCode.Count;
      for (int i = 0; i < count; i++) {
        string s = (string)allCode[i];
        if (i + 1 < count) {
          string next = (string)allCode[i + 1];
          if (s.IndexOf('=') > 0 && next.IndexOf('=') > 0) {
            string[] sParts = s.Split('=');
            string[] nextParts = next.Split('=');
            string sTrimmed = sParts[0].Trim();
            string nextTrimmed = nextParts[1].Trim().TrimEnd(';');
            if (sTrimmed == nextTrimmed) {
              s = nextParts[0] + "=" + sParts[1];
              i++;
            }
          }
        }
        code.text += s + "\n";
      }
    }
    else {
      foreach (string s in allCode) {
        code.text += s + "\n";
      }
    }
  }

  public void Status(string text) {
    statusText.text = text;
    allCode.Add(text);
    instructionCount++;
    updateScoreText();

    // not good for performance but for now this should be ok
    updateAllCodePanel();
  }

  public void SetCondensedCode(bool value) {
    condensedCode = value;
    updateAllCodePanel();
  }
}