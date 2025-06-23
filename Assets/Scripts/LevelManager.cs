using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GameState;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour, IGameState
{
    private static LevelManager levelManager;
    [SerializeField] private float timeToBeat = 0.0f;
    private float currentTimer = 0.0f;
	private static int deaths = 0;

    private int freezeLimit = 0;
    private int timesFrozen = 0;
    private bool canPlayerFreeze = true;

    private GameStateManager gsManager;
    private e_GAMESTATE state;

    private GameObject GUIObj;
    private GameObject LevelCompleteGUIObj;
    private GameObject StartMenuGUIObj;
	GameObject adWindow;

    private GameObject finalGoal;
    private SpriteRenderer[] goalSprites;
    private SpriteMask[] goalMasks;
    private ParticleSystem dataParticle;

    private GameObject[] carrots;
    private GameObject extraCollectible;
    private SpriteRenderer[] extraCarrotSprites;
    private Animator carrotAnim;
    private ParticleSystem carrotPS;
    [HideInInspector]
    public bool extraCollectableEarned = false;
    public int CollectableAmount { get; private set; }
    public int CurrentCollectableCount { get; set; }
    public bool LevelStarted { get; private set; }

    public static LevelManager GetInstance()
    {
        if (levelManager == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("LevelManager");

            if (go == null)
            {
                go = new GameObject();
                go.name = "LevelManager";
                go.tag = "LevelManager";
                levelManager = go.AddComponent<LevelManager>();
            }
            else if (go.GetComponent<LevelManager>() == null)
                levelManager = go.AddComponent<LevelManager>();
            else
                levelManager = go.GetComponent<LevelManager>();
        }
        return levelManager;
    }

    void Start()
    {
        carrots = GameObject.FindGameObjectsWithTag("Carrot");
        levelManager = LevelManager.GetInstance();
        gsManager = GameStateManager.GetInstance();
        state = gsManager.GetGameState();
        gsManager.GameStateSubscribe(this.gameObject);
        Screen.sleepTimeout = SleepTimeout.SystemSetting;

        finalGoal = GameObject.FindGameObjectWithTag("Goal");
        finalGoal.GetComponent<Animator>().enabled = false;
        finalGoal.GetComponent<CircleCollider2D>().enabled = false;
        goalSprites = finalGoal.gameObject.GetComponentsInChildren<SpriteRenderer>();
        goalMasks = finalGoal.gameObject.GetComponentsInChildren<SpriteMask>();
        dataParticle = finalGoal.gameObject.GetComponentInChildren<ParticleSystem>();
        dataParticle.Stop();
        foreach (SpriteRenderer spriteRenderer in goalSprites)
        {
            spriteRenderer.enabled = false;
        }
        foreach (SpriteMask spriteMask in goalMasks)
        {
            spriteMask.enabled = false;
        }

        GUIObj = GameObject.FindGameObjectWithTag("GUI");

        LevelCompleteGUIObj = GameObject.FindGameObjectWithTag("LevelCompleteGUI");
        LevelCompleteGUIObj.SetActive(false);
        StartMenuGUIObj = GameObject.FindGameObjectWithTag("StartMenuGUI");

        //LevelCompleteGUIObj.SetActive(false);

        CurrentCollectableCount = 0;
        CollectableAmount = carrots.Length;
        if (PersistentGUI.Instance != null)
        {
            PersistentGUI.Instance.gameObject.SetActive(true);
        }
        extraCollectible = GameObject.FindGameObjectWithTag("ExtraCollectable");
        extraCarrotSprites = extraCollectible.GetComponentsInChildren<SpriteRenderer>();
        carrotAnim = extraCollectible.GetComponentInChildren<Animator>();
        carrotPS = extraCollectible.GetComponentInChildren<ParticleSystem>();
        extraCollectible.SetActive(false);
    }

    void Update()
    {
        if (state == e_GAMESTATE.PLAYING || state == e_GAMESTATE.PAUSED)
        {
            currentTimer += Time.unscaledDeltaTime;
        }   
    }

    public void ChangeState(e_GAMESTATE e_state)
    {
        if (state == e_GAMESTATE.PLAYING && e_state == e_GAMESTATE.LEVELCOMPLETE)
        {
            ////AkSoundEngine.PostEvent("Stop_WormHole_Loop", gameObject);
            // //AkSoundEngine.PostEvent("Play_WormHole_Exit", gameObject);
            LevelCompleteController controller = (LevelCompleteController)FindObjectOfType(typeof(LevelCompleteController), true);

            controller.gameObject.SetActive(true);
            Screen.sleepTimeout = SleepTimeout.SystemSetting;

            double timeTaken = System.Math.Round(currentTimer, 2);
            double recommendedTime = System.Math.Round(timeToBeat, 2);
            LevelCompleteGUIObj.GetComponent<LevelCompleteController>().CompleteLevel(SceneManager.GetActiveScene().buildIndex, timeTaken, recommendedTime, deaths);
			deaths = 0;
		}

        if (state == e_GAMESTATE.MENU && e_state == e_GAMESTATE.PLAYING)
        {
            StartMenuGUIObj.SetActive(false);
            if (PersistentGUI.Instance != null)
            {
                PersistentGUI.Instance.gameObject.SetActive(false);
            }
        }
        state = e_state;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void DeathRestart()
    {
		deaths++;
        if (SaveManager.Instance.LoadLives() > 0)
        {
            SaveManager.Instance.SaveLives(SaveManager.Instance.LoadLives() - 1);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
		deaths = 0;
    }

    public bool GetPlayerFreezeStatus()
    {
        return canPlayerFreeze;
    }

    public void ToggleLevelFreeze()
    {
        if (state == e_GAMESTATE.PLAYING)
        {
            if (timesFrozen < freezeLimit || freezeLimit < 0)
            {
                gsManager.SetGameState(e_GAMESTATE.PAUSED);
                timesFrozen++;
            }
        }
        else if (state == e_GAMESTATE.PAUSED)
            gsManager.SetGameState(e_GAMESTATE.PLAYING);
    }

    public void StartLevel()
    {
        if (state == e_GAMESTATE.MENU)
        {
            if (SaveManager.Instance.LoadLives() > 0)
            {
                gsManager.SetGameState(e_GAMESTATE.PLAYING);
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
                LevelStarted = true;
            }
            else
            {
                gsManager.SetGameState(e_GAMESTATE.PLAYING);
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
                LevelStarted = true;
                //adWindow.SetActive(true);
            }
        }
    }

    public void SetGoalActive()
    {
        finalGoal.GetComponent<CircleCollider2D>().enabled = true;
        finalGoal.GetComponent<Animator>().enabled = true;
        foreach (SpriteRenderer spriteRenderer in goalSprites)
        {
            spriteRenderer.enabled = true;
        }
        foreach (SpriteMask spriteMask in goalMasks)
        {
            spriteMask.enabled = true;
        }
        dataParticle.Play();
    }

    public void SpawnExtraCollectible()
    {
        extraCollectible.SetActive(true);
    }
}