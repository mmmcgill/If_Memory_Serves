using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

	#region Variables

	// Audio

	[SerializeField]
	public GameObject PanelLevelSelect, loadingPanel, PanelStory, backwardButton, PanelInfo;
    public GameObject PanelStory1, PanelStory2, PanelStory3;

    [SerializeField]
	public GameObject UICanvas;

	private int currentLevel;
	private int currentWorld;
	private int achievedLevel;
	private int achievedWorld;

    public int currentStoryPanel = 1;

    public AudioClip simpleButtonSFX;

	//private LevelManager levelManager;
	#endregion

	#region Unity Event Functions
	void Awake() {
    Time.timeScale = 1;

    // Get saved world and level or assign initial world and level
    if (!(PlayerPrefs.HasKey("achievedWorld"))) {
      PlayerPrefs.SetInt("achievedWorld", 1);
      PlayerPrefs.SetInt("achievedLevel", 1);
    }
    achievedWorld = PlayerPrefs.GetInt("achievedWorld");
    achievedLevel = PlayerPrefs.GetInt("achievedLevel");
    currentWorld = 1;
    currentLevel = 1;

    string sceneName = SceneManager.GetActiveScene().name;     // "level 0-1" for example

    if (PanelLevelSelect != null) {
      // Set all levels to disabled
      int levelCount = 1;
      for (int i = 0; i < PanelLevelSelect.transform.childCount; i++) {
        //Debug.Log("currentLevel is " + currentLevel);

        if (PanelLevelSelect.transform.GetChild(i).name.Contains("Level")) {
          if (currentLevel >= levelCount) {
            PanelLevelSelect.transform.GetChild(i).GetComponent<Button>().interactable = true;
            levelCount++;
          }
          else {
            PanelLevelSelect.transform.GetChild(i).GetComponent<Button>().interactable = false;
          }
        }		
      }
    }

	}

	void Start()
	{
	}

    #endregion

    #region Public Functions
    //restarts the level

    public void forwardStoryPanel()
    {
        currentStoryPanel += 1;
        showStoryPanel();
    }

    public void backwardStoryPanel()
    {
        currentStoryPanel -= 1;
        showStoryPanel();
    }

    private void showStoryPanel()
    {
        // Shows game story panels 1 through 3 depending on what is selected
        Debug.Log(currentStoryPanel);
        switch (currentStoryPanel)
        {
            case 1:
                PanelStory1.SetActive(true);
                PanelStory2.SetActive(false);
                PanelStory3.SetActive(false);
                backwardButton.SetActive(true);
                break;
            case 2:
                PanelStory1.SetActive(false);
                PanelStory2.SetActive(true);
                PanelStory3.SetActive(false);
                break;
            case 3:
                PanelStory1.SetActive(false);
                PanelStory2.SetActive(false);
                PanelStory3.SetActive(true);
                break;
            case 4:
                // Show the PanelLevelSelect
                PanelLevelSelect.SetActive(true);
                PanelStory.SetActive(false);
               break;
            default:
                PanelStory1.SetActive(true);
                PanelStory2.SetActive(false);
                PanelStory3.SetActive(false);
                break;
        }
    }
    public void closeStoryPanel(){
        PanelStory.SetActive(false);
    }

    public void gotoStory(){
        PanelStory.SetActive(true);
    }

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

	//goes to the world select screen
	public void goToGeneric(string panelName)
	{
		SoundManager.instance.PlaySingle(simpleButtonSFX);

		for (int i = 0; i < UICanvas.transform.childCount; i++)
		{
			if (UICanvas.transform.GetChild(i).name.Contains(panelName))
			{ UICanvas.transform.GetChild(i).gameObject.SetActive(true); }
			else { UICanvas.transform.GetChild(i).gameObject.SetActive(false); };
		}
	}

	//advances scene to the specified level
	public void goToLevel(Button buttonSelected)
	{
		SoundManager.instance.PlaySingle(simpleButtonSFX);

		string buttonName = buttonSelected.name;
		buttonName = buttonName.Remove(0,5);					// Left with "0-1"
		string[] sceneLocale = buttonName.Split ('-'); 

		currentWorld = int.Parse (sceneLocale[0]);
		currentLevel = int.Parse (sceneLocale[1]);

		//Debug.Log(currentWorld + " " + currentLevel);
		PlayerPrefs.SetInt("currentLevel", currentLevel);
		PlayerPrefs.SetInt("currentWorld", currentWorld);
		SceneManager.LoadScene("FromLevelLoadFile");
	}
		
	public void closePanel()
	{
		SoundManager.instance.PlaySingle(simpleButtonSFX);
        //goToGeneric("Main");
        PanelLevelSelect.SetActive(false);
	}

	#endregion

	#region Private Functions

	public void playButtonSound(){
		SoundManager.instance.PlaySingle(simpleButtonSFX);
	}
		
	public void goToEULA()
	{
	}

	public void goToPrivacy()
	{
	}

	#endregion
}