using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelButtons : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    #region VAR_DECLORATION

    [SerializeField] private int levelNum;
    [SerializeField] private string levelName;
    [SerializeField] private GameObject pointer;
    [SerializeField] private Text levelText;
    [SerializeField] private Text completeText;

    #endregion

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        // Shows a pointer
        pointer.SetActive(true);

        // Displays the Level name and best finish time on selection.
        levelText.text = "Level " + levelNum.ToString() + ": " + levelName;
        completeText.text = (Player.current.playerData.highscores[levelNum].finished)
            ? $"{Player.current.playerData.highscores[levelNum].highscore} sec" : "";
    }

    public void OnDeselect(BaseEventData eventData)
    {
        // Disables the pointer on the button on deselect
        pointer.SetActive(false);
    }
}
