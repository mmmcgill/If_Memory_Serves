using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WorldLoader : MonoBehaviour
{
    public ArrayList levels; // string
    public Dictionary<string, TextAsset> levelAssets;
    public Dictionary<int, int> worlds;
    private int world;

    [SerializeField]
    private GameObject levelButton, worldPanel, worldText;

    [SerializeField]
    private Sprite lockImage;

    [SerializeField]
    public GameObject ScrollViewContent;

    void Awake()
    {
        levelAssets = new Dictionary<string, TextAsset>();
        worlds = new Dictionary<int, int>();
        LoadWorlds();
    }

    void Start()
    {

        if (ScrollViewContent != null)
        {
             LoadLevelSelect();
        }
    }

    // translates from 2-3 to 13 if there are 5 sublevels per world starting at 0
    public int GetIndex(int world, int sublevel)
    {
        int level = sublevel;
        for (int i = 0; i < world; i++)
        {
            level += worlds[i];
        }
        return level;
    }

    void LoadWorlds()
    {
        levels = new ArrayList();
        world = 0;
        int level = 0;
        bool hasMoreWorlds = true;
        while (hasMoreWorlds)
        {
            bool hasMoreLevels = true;
            level = 0;
            int levelsThisWorld = 0;
            while (hasMoreLevels)
            {
                string name = "level" + world + "-" + level;
                TextAsset ta = Resources.Load<TextAsset>("levels/" + name);
                if (ta == null)
                {
                    hasMoreLevels = false;
                }
                else
                {
                    levels.Add(name);
                    levelAssets[name] = ta;
                    levelsThisWorld++;
                    level++;
                }
            }
            if (level == 0)
            {
                hasMoreWorlds = false;
            }
            else
            {
                worlds[world] = levelsThisWorld;
                world++;
            }
        }
    }

    // This loads (dynamically) the grid on the level select screen.
    public void LoadLevelSelect()
    {

        // Remove the current worlds, then reload
        foreach (Transform child in ScrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        string[] worldTitle = new string[] { "Coffee Break", "Just Desserts", "Rise and Shine", "High Noon", "Tapas", "Main Grub" };
        int worldY = 0;
        int achievedWorld = PlayerPrefs.GetInt("achievedWorld");
        int achievedLevel = PlayerPrefs.GetInt("achievedLevel");
        //Debug.Log("Level Load achievedWorld" + achievedWorld);
        //Debug.Log("Level Load achievedLevel" + achievedLevel);

        Color textColor = new Color();
        ColorUtility.TryParseHtmlString("#7F4E0A", out textColor);
        GameObject[] worldPanels = new GameObject[worlds.Count];
        int totalStars = 0;

        for (int i = 0; i < worlds.Count; i++)
        {
            // Get the stars saved in previous play sessions for the world.
            int[] starsArray = new int[4];

            // NumStars-World-0[levelnum]
            // NumStars-World-1[levelnum]
            // etc.
            starsArray = PlayerPrefsX.GetIntArray("NumStars-World-" + i);

            // Create a panel
            worldPanels[i] = Instantiate(worldPanel) as GameObject;
            worldPanels[i].transform.SetParent(ScrollViewContent.transform, false);

            GameObject newText = Instantiate(worldText) as GameObject;
            newText.transform.SetParent(worldPanels[i].transform, false);
            worldY = 50 - (100 * i);
            newText.name = "worldText" + i;
            newText.GetComponent<Text>().color = textColor;
            newText.GetComponent<Text>().text = worldTitle[i];
            Vector2 newTextPos = new Vector2(-345, 0);
            newText.transform.localPosition = newTextPos;

            for (int j = 0; j < worlds[i]; j++)
            {
                GameObject newButton = Instantiate(levelButton) as GameObject;
                newButton.transform.SetParent(worldPanels[i].transform, false);
                newButton.name = "level" + i + "-" + j;
                GameObject star1 = newButton.gameObject.transform.Find("Star1").gameObject;
                GameObject star2 = newButton.gameObject.transform.Find("Star2").gameObject;
                GameObject star3 = newButton.gameObject.transform.Find("Star3").gameObject;
                GameObject star1Outline = newButton.gameObject.transform.Find("Star1Outline").gameObject;
                GameObject star2Outline = newButton.gameObject.transform.Find("Star2Outline").gameObject;
                GameObject star3Outline = newButton.gameObject.transform.Find("Star3Outline").gameObject;

                if (achievedLevel == -1 && i == 0 && j == 0)
                {
                    // Show the first level in the first world only
                    newButton.GetComponentInChildren<Text>().text = (j + 1) + "";
                    // set up the stars
                    star1Outline.SetActive(true);
                    star2Outline.SetActive(true);
                    star3Outline.SetActive(true);
                    star1.SetActive(false);
                    star2.SetActive(false);
                    star3.SetActive(false);
                }
                else if (starsArray[j] > 0 || (j > 0 && starsArray[j - 1] > 0) || (starsArray[j] == 0 && (i >= 1) && (j == 0)))
                {
                    // In same world
                    newButton.GetComponentInChildren<Text>().text = (j + 1) + "";

                    // set up the stars
                    star1Outline.SetActive(true);
                    star2Outline.SetActive(true);
                    star3Outline.SetActive(true);

                    if (starsArray[j] >= 1) { star1.SetActive(true); star1Outline.SetActive(false); }
                    if (starsArray[j] >= 2) { star2.SetActive(true); star2Outline.SetActive(false); }
                    if (starsArray[j] >= 3) { star3.SetActive(true); star3Outline.SetActive(false); }
                    totalStars += starsArray[j];

                }
                else
                {
                    // Show the lock 
                    newButton.GetComponent<Image>().sprite = lockImage;
                    newButton.GetComponentInChildren<Text>().text = "";
                    star1.SetActive(false);
                    star2.SetActive(false);
                    star3.SetActive(false);
                    star1Outline.SetActive(false);
                    star2Outline.SetActive(false);
                    star3Outline.SetActive(false);
                    newButton.GetComponent<Button>().interactable = false;
                }
                Vector2 newButtonPos = new Vector2(175 * j - 110, 0);
                newButton.transform.localPosition = newButtonPos;
                PlayerPrefs.SetInt("totalStars", totalStars);
            }
        }
    }
}


