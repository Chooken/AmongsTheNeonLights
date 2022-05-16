using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region VAR_DECLORATION

    private Rigidbody2D playerRB;

    [SerializeField] private GameScript game;
    [SerializeField] private float playerSpeed = 2f;
    [SerializeField] private CutSceneManager cutSceneManager;

    private Vector2 desiredDirecetion;
    private bool isSpeedUp = false;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    #region EVENTS

    private void OnDisable()
    {
        playerRB.velocity = Vector2.zero;
    }

    public void OnMove(InputValue value)
    {
        desiredDirecetion = value.Get<Vector2>().normalized;
    }

    public void OnExit(InputValue value)
    {
        AudioManager.instance.InstaStopMusic();
        game.GoToScene(1);
    }

    public void OnNextSentence(InputValue value)
    {
        cutSceneManager.NextSentence();
    }

    public void OnSpeedUp(InputValue value)
    {
        isSpeedUp = value.isPressed;
    }

    #endregion

    private void Update()
    {
        // Updates player velocity and timescale
        playerRB.velocity = desiredDirecetion * playerSpeed / Time.timeScale;
        Time.timeScale = (isSpeedUp) ? Player.current.playerData.speed : 1f;
        AudioManager.instance.SetSongSpeed(Time.timeScale);
    }

#if UNITY_EDITOR

	public void OnLogTime(InputValue value)
	{
		Debug.Log(AudioManager.instance.songTime());
	}

#endif
}
