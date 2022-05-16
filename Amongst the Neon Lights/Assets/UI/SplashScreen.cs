using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aarthificial.Reanimation;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    #region VAR_DECLORATION
    private Reanimator reanimator;
    #endregion

    private void Awake()
    {
        reanimator = GetComponent<Reanimator>();
    }

    #region EVENTS

    private void OnEnable()
    {
        reanimator.AddListener("SplashFinish", CompleteSplashScreen);
    }

    private void OnDisable()
    {
        reanimator.RemoveListener("SplashFinish", CompleteSplashScreen);
    }

    private void CompleteSplashScreen()
    {
        SceneManager.LoadScene(1);
    }

    #endregion
}
