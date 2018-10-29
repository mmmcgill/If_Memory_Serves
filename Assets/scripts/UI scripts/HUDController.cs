using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour {

	#region Variables

	// Audio
	[SerializeField]
	private AudioClip   worldMusic;
	[SerializeField]
	private AudioClip   chatter;
	[SerializeField]
	private AudioClip   worldVictoryMusic;
	[SerializeField]
	private AudioClip   worldVictorySFX;
	[SerializeField]
	private AudioClip   levelVictory;
	[SerializeField]
	private AudioClip   simpleButtonSFX;
	[SerializeField]

	private Animator anim;

	private int currentLevel;
	private int currentWorld;
	private int achievedLevel;
	private int achievedWorld;
    private int currentPosition;

	[SerializeField]
	private Text levelCompleteText;

	[SerializeField]
	public GameObject loadingPanel, LevelPopUpPanel, PanelPause, PanelInfo, PanelToggle, ButtonToggleCode, PanelSettings, PanelStory, backwardButton;
	[SerializeField]
	public GameObject PanelStory1, PanelStory2, PanelStory3;
	public int currentStoryPanel = 1;

	private string[] levelCompleteFeedback = { "Excellent!", "Good job!", "You did it!", "Fantastic!", "Very good!", "Superb!", "Splendid!"};

	LevelController levelController;


    [SerializeField]
    public GameObject PanelTutorial, buttonTutorial, PanelTutorialText;
    //add another value for panel instruct text 


	//private LevelManager levelManager;
	#endregion

	#region Unity Event Functions
	void Awake()
	{
		Time.timeScale = 1;

		// Get saved world and level or assign initial world and level
		if (!(PlayerPrefs.HasKey("achievedWorld"))) 
		{
			PlayerPrefs.SetInt("achievedWorld", 1);
			PlayerPrefs.SetInt("achievedLevel", 1);
		}
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
	public void showPanelInfo()     
	{
		Time.timeScale = 0.0001F; 
		PanelInfo.SetActive(true);
	}

	//resumes the game from the paused state
	public void closePanelInfo()    
	{
		Time.timeScale = 1;  
		PanelInfo.SetActive(false);
        Debug.Log(string.Format("done"));
		SoundManager.instance.PlayBGMusic(worldMusic);
		SoundManager.instance.PlayBGChatter (chatter);
        showTutorialPanels();
	}

    public void showPanelInstruct1( List <string> textToShow )
    {
        //Time.timeScale = 0.0001F; 

      // PanelTutorial.SetActive(true);

        //loop to show panel instruct 

        //show text for 5 seconds

       // PanelTutorial.SetActive(false);
    }

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

            PanelTutorial.transform.localPosition = new Vector3((float)textPosition[index,0], (float)textPosition[index,1], 0.0f);
            print(PanelTutorial.transform.position);

            //show up one of the value in txt
            index++;
            yield return new WaitForSeconds(8.0f);
        }
        PanelTutorial.SetActive(false);
    }

	public void showTutorialPanels(){

        currentWorld = PlayerPrefs.GetInt("currentLevel");
        currentLevel = PlayerPrefs.GetInt("currentWorld");

        // Debug.Log(string.Format("print"));
        Debug.Log("Inside show tutorial panel**********");
        TextAsset textFile = Resources.Load("tutorialText") as TextAsset;
        string[] text = textFile.text.Split("\n"[0]);

        Debug.Log("$$$$" + currentWorld + "-"+ currentLevel);

		List<string> showText = new List<string> ();


        double[,] textPosition = new double[5, 2];
        int index = 0;

		foreach(string line in text) {
            
            char[] seperators = { ';' };
            string[] myTutorialText = line.Split(seperators);

            //float x = float.Parse(myTutorialText[2]);
            //float y = float.Parse(myTutorialText[3]);

			if (myTutorialText.Length >0)
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
        if (showText.Count >0) {
            Debug.Log(currentWorld + "-" + currentLevel + " " + showText.Count);
            StartCoroutine(showTextInPanel(showText, textPosition));
        }
	}
    public void closeTutorialPanel(){
        PanelTutorial.SetActive(false);
    }
		
   // public void showPanelInstruct2()
    //{
        //Time.timeScale = 0.0001F; 
     //   PanelInstruct.SetActive(true);
      //  PanelInstruct1.SetActive(false);
       // PanelInstruct2.SetActive(true);
    //}

   // public void closeInstruct()
    //{
      //  Time.timeScale = 1;  
        //PanelInstruct.SetActive(false); 
    //}
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
        SoundManager.instance.PlayBGChatter (chatter);
    }


	//goes to main menu
	public void goToMainMenu()
	{
		// loadingPanel.SetActive (true);
		SceneManager.LoadScene("mainMenu");
	}

	//reset to world 0
	public void resetWorld()
	{
		SceneManager.LoadScene ("World0");
	}
		
	//restarts the level
	public void restartLevel()
	{
		GameObject.Find("TheLevel").GetComponent<LevelLoader>().ResetLevel();
	}

	public void forwardStoryPanel() {
		currentStoryPanel += 1;
		showStoryPanel ();
	}

	public void backwardStoryPanel() {
		currentStoryPanel -= 1;
		showStoryPanel ();
	}

	private void showStoryPanel() {
		// Shows game story panels 1 through 3 depending on what is selected
		Debug.Log(currentStoryPanel);
		switch (currentStoryPanel) {
		case 1:
			PanelStory1.SetActive (true);
			PanelStory2.SetActive (false);
			PanelStory3.SetActive (false);
			backwardButton.SetActive (true);
			break;
		case 2:
			PanelStory1.SetActive(false);
			PanelStory2.SetActive(true);
			PanelStory3.SetActive(false);
			break;
		case 3:
			PanelStory1.SetActive (false);
			PanelStory2.SetActive (false);
			PanelStory3.SetActive (true);
			break;
		case 4:
			// Show the PanelInfo
			PanelInfo.SetActive (true);
			PanelStory.SetActive (false);
			break;
		default:
			PanelStory1.SetActive (true);
			PanelStory2.SetActive(false);
			PanelStory3.SetActive(false);
			break;
		}
	}

	//advances scene to the specified level
    public void goToLevel(Button buttonSelected)
	{
		string buttonName = buttonSelected.name;
		buttonName = buttonName.Remove(0,5);					// Left with "0-1"
		string[] sceneLocale = buttonName.Split ('-'); 

		currentWorld = int.Parse (sceneLocale[0]);
		currentLevel = int.Parse (sceneLocale[1]);

		//Debug.Log(currentWorld + " " + currentLevel);
		PlayerPrefs.SetInt("currentLevel", currentLevel);
		PlayerPrefs.SetInt("currentWorld", currentWorld);
		SceneManager.LoadScene("World0");
		//AudioManager.instance.PlayNewMusic(worldMusic);
	}
		
	public void callNextLevel()
	{
		levelCompleteText.text = levelCompleteFeedback[Random.Range(0, levelCompleteFeedback.Length)];
		//StartCoroutine(displayFeedback());
		//AudioManager.instance.PlaySingle(false, levelVictory);
	}

	public void closePanel()
	{
	//	goToGeneric("Main");
	}

	#endregion

	#region Private Functions

	public void playButtonSound(){
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