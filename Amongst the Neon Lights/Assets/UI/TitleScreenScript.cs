using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class TitleScreenScript : MonoBehaviour
{
    #region VAR_DECLORATION
    [SerializeField] private AudioMixer audioMixer;

    #endregion

    private void Awake()
    {
        Player.current = (Player)SerializationManager.Load(Application.persistentDataPath + "/saves/PlayerData.save");
    }

    private void Start()
    {
        audioMixer.SetFloat("MUSICvolume", Player.current.playerData.musicVolume);
        audioMixer.SetFloat("SFXvolume", Player.current.playerData.sfxVolume);
        if (!AudioManager.instance.isPlaying()) AudioManager.instance.Play("MainTheme1");
        Player.current.playerData.lastlevel = -1;
    }

    #region EVENTS

    public void GoToScene(int sceneInt)
    {
        SceneManager.LoadScene(sceneInt);
    }

    public void PlayClick()
    {
        AudioManager.instance.PlaySFX("Click");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    #endregion
}
