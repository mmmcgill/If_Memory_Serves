using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;

[RequireComponent(typeof(TextMesh))]
public class MainMenuController : MonoBehaviour
{

    #region Variables

    public static List<string> textEULA;
    private TextMesh textComp;

    [SerializeField]
    public int[] rowsToReadFrom;
    public string fileName;
    private TextAsset textAsset;

    [SerializeField]
    private GameObject PanelMain, PanelLevelSelect, PanelSettings, PanelAbout;

    [SerializeField]
    private GameObject PanelEula, PanelPrivacy, PanelEulaText, PanelPrivacyText; 

    [SerializeField]
    private GameObject UICanvas;
    private GameObject[] pause;

    private int currentLevel;
    private int currentWorld;
    private int achievedLevel;
    private int achievedWorld;

    // Audio
    [SerializeField]
    private AudioClip simpleButtonSFX;

    #endregion

    #region Unity Event Functions
    void Awake()
    {
        Time.timeScale = 1;
        // Get saved world and level or assign initial world and level
        if (!(PlayerPrefs.HasKey("achievedWorld")))
        {
            resetGame();
        }


        achievedWorld = PlayerPrefs.GetInt("achievedWorld");
        achievedLevel = PlayerPrefs.GetInt("achievedLevel");
        currentWorld = 1;
        currentLevel = 1;

      // if (PanelLevelSelect != null)
      //  {
            // Set all levels to disabled
            int levelCount = 1;
            for (int i = 0; i < PanelLevelSelect.transform.childCount; i++)
            {
                if (PanelLevelSelect.transform.GetChild(i).name.Contains("Level"))
                {
                    if (currentLevel >= levelCount)
                    {
                        PanelLevelSelect.transform.GetChild(i).GetComponent<Button>().interactable = true;
                        levelCount++;
                    }
                    else
                    {
                        PanelLevelSelect.transform.GetChild(i).GetComponent<Button>().interactable = false;
                    }
                }
            }
      //  }

        // Not the cleanest way to do this, but it works.
        if (PlayerPrefs.HasKey("mainPanel")) {
            if (PlayerPrefs.GetString("mainPanel")=="PanelLevelSelect") {
                // show the panel specified
                PanelLevelSelect.SetActive(true);
                PanelMain.SetActive(false);
                PlayerPrefs.SetString("mainPanel", "");
            }
            else if (PlayerPrefs.GetString("mainPanel") == "PanelSettings") {
                // show the panel specified
                PanelSettings.SetActive(true);
                PanelMain.SetActive(false);
                PlayerPrefs.SetString("mainPanel", "");
            }
        }
    }

    public void resetGame() {
        Debug.Log("Resetting game");
        PlayerPrefs.SetInt("achievedWorld", -1);
        PlayerPrefs.SetInt("achievedLevel", -1);
        PlayerPrefs.SetInt("totalStars", 0);
        PlayerPrefs.SetInt("totalGold", 0);

        int[] starsArray = new int[4];

        for (int i = 0; i < 6; i++)
        {
            for (int j = 1; j < 5; j++)
            {
                starsArray[j - 1] = 0;
            }
            PlayerPrefsX.SetIntArray("NumStars-World-" + i, starsArray);
        }
        // Set all levels to disabled
        int levelCount = 1;
        for (int i = 0; i < PanelLevelSelect.transform.childCount; i++)
        {
            if (PanelLevelSelect.transform.GetChild(i).name.Contains("Level"))
            {
                if (currentLevel >= levelCount)
                {
                    PanelLevelSelect.transform.GetChild(i).GetComponent<Button>().interactable = true;
                    levelCount++;
                }
                else
                {
                    PanelLevelSelect.transform.GetChild(i).GetComponent<Button>().interactable = false;
                }
            }
        }

    }

    void Start()
    {
        textAsset = Resources.Load("EULAtext") as TextAsset;
        textComp = GetComponent<TextMesh>();
        showEULAText();
    }

    #endregion

    #region Public Functions

    public void goToLevelSelect()
    {
        // Create the buttons on the screen
        SoundManager.instance.PlaySingle(simpleButtonSFX);
        goToGeneric("Level");
    }

    public void goToAbout()
    {
        SoundManager.instance.PlaySingle(simpleButtonSFX);
        goToGeneric("About");
    }

    public void goToSettings()
    {
        SoundManager.instance.PlaySingle(simpleButtonSFX);
        PanelSettings.SetActive(true);
    }


    //goes to the panelName screen
    public void goToGeneric(string panelName)
    {
        SoundManager.instance.PlaySingle(simpleButtonSFX);

        for (int i = 0; i < UICanvas.transform.childCount; i++)
        {
            if (UICanvas.transform.GetChild(i).name.Contains(panelName))
            { UICanvas.transform.GetChild(i).gameObject.SetActive(true); }
        }
    }

    //advances scene to the specified level
    public void goToLevel(Button buttonSelected)
    {
        SoundManager.instance.PlaySingle(simpleButtonSFX);

        string buttonName = buttonSelected.name;
        buttonName = buttonName.Remove(0, 5);                   // Left with "0-1"
        string[] sceneLocale = buttonName.Split('-');

        currentWorld = int.Parse(sceneLocale[0]);
        currentLevel = int.Parse(sceneLocale[1]);

        PlayerPrefs.SetInt("currentLevel", currentLevel);
        PlayerPrefs.SetInt("currentWorld", currentWorld);

        SceneManager.LoadScene("FromLevelLoadFile"); //, LoadSceneMode.Additive);
    }

    public void goToEULA()
    {
        SoundManager.instance.PlaySingle(simpleButtonSFX);
        goToGeneric("EULA");
        PanelAbout.SetActive(false);
    }

    public void goToPrivacy()
    {
        SoundManager.instance.PlaySingle(simpleButtonSFX);
        goToGeneric("Privacy");
        PanelAbout.SetActive(false);
    }

    //[MenuItem("Tools/Read file")]
    public void showEULAText()
    {
        textEULA = textAsset.text.Split('\n').ToList();
        for (int i = 0; i < rowsToReadFrom.Length; i++){
            if (rowsToReadFrom[0]<0|| rowsToReadFrom.Length==0){
                textComp.text = textAsset.text;
            }else{
                textComp.text += textEULA[rowsToReadFrom[i]] + "\n";
            }
        }
    }
    public void closePanel(string panelName)
    {
        SoundManager.instance.PlaySingle(simpleButtonSFX);

        for (int i = 0; i < UICanvas.transform.childCount; i++)
        {
            if (UICanvas.transform.GetChild(i).name.Contains(panelName))
            { UICanvas.transform.GetChild(i).gameObject.SetActive(false); }
            //else { UICanvas.transform.GetChild(i).gameObject.SetActive(true); };
        }
    }
    public void closeAbout(){
        SoundManager.instance.PlaySingle(simpleButtonSFX);
        closePanel("PanelAbout");
    }

    public void closeSetting(){
        SoundManager.instance.PlaySingle(simpleButtonSFX);
        //closePanel("PanelSetting");
        PanelSettings.SetActive(false);

    }

    public void closeLevelSelect(){
        SoundManager.instance.PlaySingle(simpleButtonSFX);
        closePanel("PanelLevelSelect");
    }

    public void closeEULA(){
        SoundManager.instance.PlaySingle(simpleButtonSFX);
        closePanel("PanelEULA");
    }

    public void closePrivacy(){
        SoundManager.instance.PlaySingle(simpleButtonSFX);
        closePanel("PanelPrivacy");
    }

    #endregion

    #region Private Functions

    public void playButtonSound()
    {
        SoundManager.instance.PlaySingle(simpleButtonSFX);
    }
    public void Mute(){
        AudioListener.pause = !AudioListener.pause;
    }

    #endregion
}