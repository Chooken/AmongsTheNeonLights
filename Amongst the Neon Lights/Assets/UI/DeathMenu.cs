using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    #region VAR_DECLORATION

    private int levelNum;

    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject highscore;

    #endregion

    private void Start()
    {
        levelNum = Player.current.playerData.lastlevel;

        UpdateScore();
    }

    private void UpdateScore()
    {
        float currentScore = Player.current.playerData.currentScore;

        if (currentScore > Player.current.playerData.highscores[levelNum].highscore)
        {
            Player.current.playerData.highscores[levelNum].highscore = currentScore;
            highscore.SetActive(true);
        }
        else highscore.SetActive(false);

        scoreText.text = $"time: {String.Format("{0:0.0}", currentScore)} sec";

        SerializationManager.Save("PlayerData", Player.current, false);
    }

    #region EVENTS

    public void GoToScene(int sceneInt)
    {
        SceneManager.LoadScene(sceneInt);
    }

    public void GoToLastLevel()
    {
		//     switch (levelNum)
		//     {
		//         case 0:
		//             SceneManager.LoadScene(6);
		//             break;
		//case 1:
		//	SceneManager.LoadScene(7);
		//	break;
		//default:
		//             return;
		//     }

		SceneManager.LoadScene(6 + levelNum);
	}

    public void PlayClick()
    {
        AudioManager.instance.PlaySFX("Click");
    }

    #endregion
}
