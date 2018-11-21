using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour
{

    #region Variables

    // Audio
    [SerializeField]
    private AudioClip worldMusic;
    [SerializeField]
    private AudioClip chatter;
    [SerializeField]
    private AudioClip worldVictoryMusic;
    [SerializeField]
    private AudioClip worldVictorySFX;
    [SerializeField]
    private AudioClip levelVictory;
    [SerializeField]
    private AudioClip simpleButtonSFX;
    [SerializeField]

    private Animator anim;

    private int currentLevel;
    private int currentWorld;
    private int achievedLevel;
    private int achievedWorld;
    private int currentPosition;

    [SerializeField]
    private Text levelCompleteText;
    public GameObject cutScene, PanelCutScene1, PanelCutScene2;

    [SerializeField]
    private GameObject loadingPanel, LevelPopUpPanel, PanelPause, PanelInfo, PanelToggle, ButtonToggleCode, PanelSettings, backwardButton, forwardButton;

    [SerializeField]
    private int currentCutScenePanel = 1;

    private string[] levelCompleteFeedback = { "Excellent!", "Good job!", "You did it!", "Fantastic!", "Very good!", "Superb!", "Splendid!" };

    LevelController levelController;


    [SerializeField]
    private GameObject PanelTutorial, buttonTutorial, PanelTutorialText;
    //add another value for panel instruct text 


    //private LevelManager levelManager;
    #endregion

    #region Unity Event Functions
    void Awake()
    {
        Time.timeScale = 1;
        achievedWorld = PlayerPrefs.GetInt("achievedWorld");
        achievedLevel = PlayerPrefs.GetInt("achievedLevel");
        levelController = GameObject.Find("/TheLevel").GetComponent<LevelController>();

    }


    void Start()
    {

        anim = PanelToggle.GetComponent<Animator>();
        //disable it on start to stop it from playing the default animation
        anim.enabled = false;

    }

    #endregion

    #region Public Functions
    //Pauses the game and timescale

    public void pauseGame()
    {
        Time.timeScale = 0.0001F;
        PanelPause.SetActive(true);
    }

    //resumes the game from the paused state
    public void resumeGame()
    {
        Time.timeScale = 1;
        PanelPause.SetActive(false);
    }

    //Pauses the game and timescale
    /*public void showPanelInfo()     
        {
            Time.timeScale = 0.0001F; 
            PanelInfo.SetActive(true);
        }*/

    //resumes the game from the paused state
    public void closePanelInfo()
    {
        Time.timeScale = 1;
        PanelInfo.SetActive(false);
        Debug.Log(string.Format("done"));
        SoundManager.instance.PlayBGMusic(worldMusic);
        SoundManager.instance.PlayBGChatter(chatter);
        showTutorialPanels();
    }

    // Pop-up tutorial panels show texts
    IEnumerator showTextInPanel(List<string> showText, double[,] textPosition)
    {
        PanelTutorial.SetActive(true);
        int index = 0;
        foreach (string line in showText)
        {
            //instruction form the showtext 
            //read in the showtext value 

            PanelTutorialText.GetComponent<Text>().text = line;
            PanelTutorial.GetComponent<GameObject>();

            PanelTutorial.transform.localPosition = new Vector3((float)textPosition[index, 0], (float)textPosition[index, 1], 0.0f);
            print(PanelTutorial.transform.position);

            //show up one of the value in txt
            index++;
            yield return new WaitForSeconds(4.0f);
        }
        PanelTutorial.SetActive(false);
    }

    // pop-up tutorial panels show up 
    public void showTutorialPanels()
    {

        currentWorld = PlayerPrefs.GetInt("currentLevel");
        currentLevel = PlayerPrefs.GetInt("currentWorld");

        // Debug.Log(string.Format("print"));
        Debug.Log("Inside show tutorial panel**********");
        TextAsset textFile = Resources.Load("tutorialText") as TextAsset;
        string[] text = textFile.text.Split("\n"[0]);

        Debug.Log("$$$$" + currentWorld + "-" + currentLevel);

        List<string> showText = new List<string>();


        double[,] textPosition = new double[5, 2];
        int index = 0;

        foreach (string line in text)
        {

            char[] seperators = { ';' };
            string[] myTutorialText = line.Split(seperators);

            if (myTutorialText.Length > 0)
            {
                Debug.Log("text found");
                if ((int.Parse(myTutorialText[0]) == currentWorld) && (int.Parse(myTutorialText[1]) == currentLevel))
                {

                    Debug.Log("Current world and current level has tutorial text.");

                    textPosition[index, 0] = double.Parse(myTutorialText[2]);
                    textPosition[index, 1] = double.Parse(myTutorialText[3]);
                    index++;
                    showText.Add(myTutorialText[4]);

                }
            }
        }
        if (showText.Count > 0)
        {
            Debug.Log(currentWorld + "-" + currentLevel + " " + showText.Count);
            StartCoroutine(showTextInPanel(showText, textPosition));
        }
    }

    //close button on tutorial panel
    public void closeTutorialPanel()
    {
        PanelTutorial.SetActive(false);
    }

    // next button on cut scene
    public void forwardCutScenePanel()
    {
        currentCutScenePanel += 1;
        showCutScenePanel();
    }

    // previous button on cut scene
    public void backwardCutScenePanel()
    {
        currentCutScenePanel -= 1;
        showCutScenePanel();
    }

    private void showCutScenePanel()
    {
        // Shows game story panels 1 through 2 depending on what is selected
        Debug.Log(currentCutScenePanel);
        switch (currentCutScenePanel)
        {
            case 1:
                PanelCutScene1.SetActive(true);
                PanelCutScene2.SetActive(false);
                backwardButton.SetActive(false);
                break;
            case 2:
                PanelCutScene1.SetActive(false);
                PanelCutScene2.SetActive(true);
                backwardButton.SetActive(true);
                break;
            case 3:
                cutScene.SetActive(false);
                PanelInfo.SetActive(true);
                break;
            default:
                PanelCutScene1.SetActive(true);
                PanelCutScene2.SetActive(false);
                backwardButton.SetActive(false);
                break;
        }
    }

    // close button on cut scene
    public void closeCutScenePanel()
    {
        cutScene.SetActive(false);
        PanelInfo.SetActive(true);

    }

    //Pauses the game and timescale
    public void showPanelSettings()
    {
        Time.timeScale = 0.0001F;
        PanelSettings.SetActive(true);
    }

    //resumes the game from the paused state
    public void closePanelSettings()
    {
        Time.timeScale = 1;
        PanelSettings.SetActive(false);
        SoundManager.instance.PlayBGMusic(worldMusic);
        SoundManager.instance.PlayBGChatter(chatter);
    }


    //goes to main menu
    public void goToMainMenu()
    {
        // loadingPanel.SetActive (true);
        SceneManager.LoadScene("main");
    }


    //restarts the level
    public void restartLevel()
    {
        GameObject.Find("TheLevel").GetComponent<LevelLoader>().ResetLevel();
    }


    //advances scene to the specified level
    public void goToMainScene(string panelName) 
    {
        PlayerPrefs.SetString("mainPanel", panelName);
        SceneManager.LoadScene("main");
    }

    public void callNextLevel()
    {
        levelCompleteText.text = levelCompleteFeedback[Random.Range(0, levelCompleteFeedback.Length)];
        //AudioManager.instance.PlaySingle(false, levelVictory);
    }

    public void closePanel()
    {
        //	goToGeneric("Main");
    }

    #endregion

    #region Private Functions

    public void playButtonSound()
    {
        AudioManager.instance.PlaySingle(/*false, */simpleButtonSFX);
    }

    public void goToEULA()
    {
    }

    public void goToPrivacy()
    {
    }


    public void toggleCodePanel()
    {

        if (!PanelToggle.activeSelf)
        {
            // slide it out
            //enable the animator component
            anim.enabled = true;
            //play the Slidein animation
            anim.Play("PauseMenuSlideIn");

            PanelToggle.SetActive(true);

            //PanelToggle.transform.Translate.Ler
            // set rotation
            //ButtonToggleCode.transform.Rotate(180,0,0);
            //position = false;
        }
        else
        {
            //ButtonToggleCode.transform.Rotate(180,0,0);
            //position = true;
            PanelToggle.SetActive(false);
        }
    }

    #endregion
}