using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttemptsScript : MonoBehaviour
{

    #region VAR_DECLORATION
    private Text text;
    #endregion

    // Grabs and displays number of attempts to the text on the obj
    void Start()
    {
        text = GetComponent<Text>();
        text.text = "Attempts: " + String.Format("{0:#,###0}", Player.current.playerData.attempts);
    }
}
