using UnityEngine;
using GameState;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class EnemyController : BaseController
{
    public bool deathAnimation = false;
    private Image healthPotion;
    private TextMeshProUGUI livesCount;
    private int display;
 
	protected void OnCollisionEnter2D(Collision2D other)
	{
		if (other.collider.tag == "Player" && (state == GameState.e_GAMESTATE.PLAYING || state == GameState.e_GAMESTATE.PAUSED) && deathAnimation == false)
		{
            levelManager.RestartLevel();
		}
	}

    public IEnumerator DeathTimer(float time)
    {
        Haptics.HapticFailure();
        gsManager.SetGameState(e_GAMESTATE.DEAD);
        //healthPotion = GameObject.FindGameObjectWithTag("HealthPotion").GetComponent<Image>();
        //livesCount = GameObject.FindGameObjectWithTag("LivesCount").GetComponent<TextMeshProUGUI>();
        //healthPotion.GetComponent<LivesAnimTrigger>().PlayAnimation();
        //livesCount.enabled = true;
        //display = PlayerPrefs.GetInt("Lives");
        //display++;
        //livesCount.SetText(display.ToString());
        yield return new WaitForSeconds(time / 2);
        //livesCount.SetText((PlayerPrefs.GetInt("Lives") - 1).ToString());
        yield return new WaitForSeconds(time / 2);
        levelManager.DeathRestart();
    }
}