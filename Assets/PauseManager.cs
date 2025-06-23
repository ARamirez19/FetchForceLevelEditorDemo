using System.Collections;
using System.Collections.Generic;
using GameState;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class PauseManager : MonoBehaviour, IGameState
{
    private e_GAMESTATE state;
    private GameStateManager gsManager;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject pauseButton;

    void Start()
    {
        gsManager = GameStateManager.GetInstance();
        gsManager.GameStateSubscribe(this.gameObject);
        state = gsManager.GetGameState();
        pauseMenu.SetActive(false);
        pauseButton.SetActive(false);
    }

    public void TogglePause()
    {
        if(state == e_GAMESTATE.PLAYING)
        {
            gsManager.SetGameState(e_GAMESTATE.PAUSED);
        }
        else if(state == e_GAMESTATE.PAUSED)
        {
            gsManager.SetGameState(e_GAMESTATE.PLAYING);
        }
    }

    private void PauseLevel()
    {
        pauseMenu.SetActive(true);
    }

    private void ContinueLevel()
    {
        pauseMenu.SetActive(false);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitLevel()
    {
        char[] worldNumber = SceneManager.GetActiveScene().name.ToCharArray();
        int world = (int)char.GetNumericValue(worldNumber[5]);
        LevelSelectController.worldCount = world;
        LevelManager.GetInstance().ReturnToLevelSelect();
    }

    public void ChangeState(e_GAMESTATE m_state)
    {
        if(state == e_GAMESTATE.PLAYING && m_state == e_GAMESTATE.PAUSED)
        {
            PauseLevel();
        }
        else if(state == e_GAMESTATE.PAUSED && m_state == e_GAMESTATE.PLAYING)
        {
            ContinueLevel();
        }
        else if(state == e_GAMESTATE.MENU && m_state == e_GAMESTATE.PLAYING)
        {
            pauseButton.SetActive(true);
        }
        else if(state == e_GAMESTATE.PLAYING && m_state == e_GAMESTATE.LEVELCOMPLETE)
        {
            pauseButton.SetActive(false);
        }

        state = m_state;
    }
}
