using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectController : MonoBehaviour
{
    [Tooltip("Reference to the World Object in the Hierarchy")]
    [SerializeField]
    private GameObject worldWindow;
    [Tooltip("Reference to the Level Object in the Hierarchy")]
    [SerializeField]
    private GameObject levelWindow;
    [SerializeField]
    private GameObject scrollContent;
    //[Tooltip("The StarCount Text for each world")]
    //[SerializeField]
    //private TextMeshProUGUI[] worldText;
    [Tooltip("The number of worlds the player has access to")]
    [SerializeField]
    private int numberOfWorlds;
    [SerializeField]
    private int worldsAvaliable;
    [Tooltip("The last world visted by the player")]
    [SerializeField]
    private int currentWorld;
    [SerializeField]
    private GameObject worldButtonPrefab;
    [SerializeField]
    private GameObject levelButtonPrefab;

    [SerializeField] TextMeshProUGUI _title;

    [SerializeField]
    private int[] levelsInWorld;

    [SerializeField] private Button _leftArrow;
    [SerializeField] private Button _rightArrow;

    private GameObject[] worlds;
    //private GameObject[] worldLevels;

    private GameObject[] levels;
    private int _gridSize = 5;
    private bool _leftToRight = true;

    public static string[] WorldNames = new string[5] { "Asteroid Belt", "Mines", "Fleet Ship", "Jump Gate", "The Other Side" };

    private bool isInAnimation = false;

    [SerializeField] private Image lockLogo;
    public static int worldCount = 1;

    // Use this for initialization
    void Start()
    {
        // Removed as World Select is done via Level Select Arrows
        /*
        scrollContent.GetComponent<RectTransform>().sizeDelta = new Vector2(100 * numberOfWorlds, 0);
        scrollContent.GetComponent<RectTransform>().position = new Vector2((numberOfWorlds - currentWorld) * this.GetComponent<RectTransform>().position.x, this.GetComponent<RectTransform>().position.y);

        worlds = new GameObject[numberOfWorlds];
        //worldLevels = new GameObject[numberOfWorlds];
        for (int i = 0; i < numberOfWorlds; i++)
        {
            int closureIndex = i;
            worlds[i] = Instantiate(worldButtonPrefab, GameObject.Find("WorldButtons").transform);
            worlds[i].name = "World" + (i+1);
            worlds[closureIndex].GetComponent<Button>().onClick.AddListener(() => WorldSelect(closureIndex + 1));
            Transform name = worlds[i].transform.Find("WorldName");
            Transform stars = worlds[i].transform.Find("StarCount");
            name.GetComponent<TextMeshProUGUI>().text = "World " + (i + 1);
            stars.GetComponent<TextMeshProUGUI>().text = CalculateWorldStars(i+1) + "/" + (levelsInWorld[i] * 3);
            /*worlds[i] = GameObject.Find("World" + (i + 1));
            worldLevels[i] = GameObject.Find("World" + ((i + 1)) + "Levels");
            worldLevels[i].SetActive(false);*/
        //UpdateWorldText(i);
        /*}
        
        levelWindow.SetActive(false);
        WorldsUnlocked();*/

        WorldSelect(worldCount);

    }
    // Update is called once per frame
    void Update ()
    {
   
	}

    private int CalculateWorldStars(int world)
    {
        int total = 0;
        for(int i=0; i<levelsInWorld[world-1]; i++)
        {
            if(SaveManager.Instance.LoadStars(world, i+1) != 0)
            {
                total += SaveManager.Instance.LoadStars(world, i + 1);
            }
        }
        return total;
    }

    public void BackToWorldSelect()
    {
        worldCount = 1;
        SceneManager.LoadScene("HomeMenu");

        // Removed as World Select is done via Level Select Arrows
        /*
        worldWindow.SetActive(true);
        Transform levels = GameObject.Find("WorldLevels").transform;
        foreach (Transform level in levels)
        {
            level.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
            Destroy(level.gameObject);
        }
        levelWindow.SetActive(false);
        scrollContent.GetComponent<RectTransform>().position = new Vector2((numberOfWorlds - currentWorld) * this.GetComponent<RectTransform>().position.x, this.GetComponent<RectTransform>().position.y);
        */
    }

    public void PrevWorld()
    {
        Debug.Log("This feature is not currently available in this demo version");
        return;
        if (isInAnimation)
        {
            return;
        }

        currentWorld--;
        WorldSelect(currentWorld);
        //_leftArrow.interactable = false;
    }

    public void NextWorld()
    {
        Debug.Log("This feature is not currently available in this demo version");
        return;
        if (isInAnimation)
        {
            return;
        }

        currentWorld++;
        WorldSelect(currentWorld);
        //_rightArrow.interactable = false;
    }

    private void ClearLevels()
    {
        if (levels == null) return;

        for(int i = 0; i < levels.Length; i++)
        {
            Destroy(levels[i]);
        }
    }

    public void WorldSelect(int worldNumber)
    {
        if (levelsInWorld[worldNumber-1] == 0)
        { 
            lockLogo.gameObject.SetActive(true);
        }
        else
        {
            lockLogo.gameObject.SetActive(false);
        }

        isInAnimation = true;
        _leftArrow.interactable = worldNumber != 1;
        _rightArrow.interactable = worldNumber != levelsInWorld.Length;

        if ((worldNumber - 1) < WorldNames.Length && worldNumber >= 0)
        {
            _title.text = WorldNames[worldNumber-1];
        }
        else
        {
            _title.text = "World " + worldNumber;
        }

        currentWorld = worldNumber;

        ClearLevels();
        levels = new GameObject[levelsInWorld[currentWorld-1]];
        levelWindow.SetActive(true);

        bool prevUnplayed = false; // Previous Level Unplayed Check
        for (int i=0; i<levels.Length; i++)
        {
            int closureIndex = i;
            levels[i] = Instantiate(levelButtonPrefab, GameObject.Find("WorldLevels").transform);
            levels[i].name = "Level" + (i + 1);

            prevUnplayed = levels[i].GetComponentInChildren<LevelButton>().SetInfo(currentWorld, (i + 1), () => StartLevel(closureIndex + 1), prevUnplayed);
        }

       // worldLevels[worldNumber - 1].SetActive(true);
        worldWindow.SetActive(false);

        StartCoroutine(CascadeLevelButtons());
    }

    private IEnumerator CascadeLevelButtons()
    {
        int i = 0;
        while (i < levels.Length)
        {
            Animator animator = levels[i].GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("Show", true);
            }
            yield return new WaitForSeconds(0.06f);

            if (_leftToRight)
            {
                i++;
                if (i % _gridSize == 0)
                {
                    _leftToRight = false;
                    i = Mathf.Min(i + _gridSize - 1, levels.Length);
                }
            }
            else
            {
                i--;
                if ((i + 1) % _gridSize == 0)
                {
                    _leftToRight = true;
                    i = Mathf.Min(i + _gridSize + 1, levels.Length);
                }
            }
        }
        isInAnimation = false;
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    public void UpdateWorldText(int world)
    {
        /*int totalStars = 0;
        Button[] levels = worldLevels[world].GetComponentsInChildren<Button>();
        foreach (Button level in levels)
        {
            totalStars += level.GetComponent<LevelButton>().StarCount();
        }
        worldText[world].GetComponent<TextMeshProUGUI>().text = totalStars.ToString() + "/63";*/

        //Insert code to access total number of stars per world from playerprefs
    }

    private void WorldsUnlocked()
    {
        for(int i=0; i<worldsAvaliable; i++)
        {
            worlds[i].GetComponent<Button>().interactable = true;
        }
    }

    public void StartLevel(int levelNum)
    {
        if(currentWorld == 1 && levelNum == 1)
        {
            if (PlayerPrefs.HasKey("ComicSeen"))
            {
                SceneManager.LoadScene("World" + currentWorld + "Level" + levelNum);
            }
            else
            {
                SceneManager.LoadScene("Prologue");
            }
        }
        else
        {
            SceneManager.LoadScene("World" + currentWorld + "Level" + levelNum);
        }
    }
}   