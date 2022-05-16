using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class OptionsManager : MonoBehaviour
{
    #region VAR_DECLORATION

    [SerializeField] private bool fullscreen;
    [SerializeField] private bool vsync;
    [SerializeField] private bool fpscounter;
    [SerializeField] private int fpsLimit;
    [SerializeField] private int display;

    [SerializeField] private Toggle fullscreenButton;
    [SerializeField] private Toggle vsyncButton;
    [SerializeField] private Toggle fpsCountButton;

    [SerializeField] private Dropdown fpsLimitDropdown;
    [SerializeField] private Dropdown displaysDropdown;

    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Text speedMultiText;

	[SerializeField] private GameObject[] menus;

    private List<DisplayInfo> m_Displays = new List<DisplayInfo>();

    private int[] framerates = { 30, 60, 120, 144, 240, 360, -1 };

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Reads settings and sets the variables in the options menu
        fullscreen = Screen.fullScreen;
        vsync = QualitySettings.vSyncCount != 0;
        fpscounter = Player.current.playerData.fpsCounter;

        fpsLimit = Player.current.playerData.fps;

        Screen.GetDisplayLayout(m_Displays);

        display = Player.current.playerData.display;

        fullscreenButton.isOn = fullscreen;
        vsyncButton.isOn = vsync;
        fpsCountButton.isOn = fpscounter;

        sfxSlider.value = Player.current.playerData.sfxVolume;
        musicSlider.value = Player.current.playerData.musicVolume;

        if (fpsLimit == -1)
        {
            fpsLimitDropdown.value = 6;
            Application.targetFrameRate = -1;
        }
        else
        {
            for (int i = 0; i < fpsLimitDropdown.options.Count; i++)
            {
                if (fpsLimitDropdown.options[i].text.Equals(fpsLimit.ToString()))
                {
                    fpsLimitDropdown.value = i;
                }
            }
        }

        for (int i = 1; i < m_Displays.Count; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = (i + 1).ToString();
            displaysDropdown.options.Add(option);
        }

        displaysDropdown.value = display;

        displaysDropdown.enabled = true;

        speedMultiText.text = String.Format("{0:0.00}", Player.current.playerData.speed);

    }

    #region EVENTS

    public void FullScreenToggle(bool isOn)
    {
        Screen.fullScreen = isOn;
    }

    public void VSyncToggle(bool isOn)
    {
        QualitySettings.vSyncCount = isOn ? 1 : 0;
    }

    public void FpsCounterToggle(bool isOn)
    {
        Player.current.playerData.fpsCounter = isOn;
    }

    public void FPSLimit(int limit)
    {
        Application.targetFrameRate = framerates[limit];
        Player.current.playerData.fps = framerates[limit];
    }

    public void SpeedMulti(int change)
    {
        Player.current.playerData.speed += (float)change / 100;

        if (Player.current.playerData.speed < 1f) Player.current.playerData.speed = 1f;
        else if (Player.current.playerData.speed > 10f) Player.current.playerData.speed = 2f;
        speedMultiText.text = String.Format("{0:0.00}", Player.current.playerData.speed);
    }

    public void ChangeMUSICvolume(float volume)
    {
        audioMixer.SetFloat("MUSICvolume", volume);
        Player.current.playerData.musicVolume = (int)volume;
    }

    public void ChangeSFXvolume(float volume)
    {
        audioMixer.SetFloat("SFXvolume", volume);
        Player.current.playerData.sfxVolume = (int)volume;
    }

    public void ChangeDisplay(int displayNum)
    {
        // Sets the display to the given index
        Screen.GetDisplayLayout(m_Displays);

        if (displayNum > m_Displays.Count) return;

        var display = m_Displays[displayNum];

        Vector2Int targetCoordinates = new Vector2Int(0, 0);

        Debug.Log("lol");

        if (Screen.fullScreenMode != FullScreenMode.Windowed)
        {
            targetCoordinates.x += display.width / 2;
            targetCoordinates.y += display.height / 2;
        }

        Screen.MoveMainWindowTo(m_Displays[displayNum], targetCoordinates);
        Player.current.playerData.display = displayNum;
    }

	public void changeMenu(int num)
	{
		foreach(GameObject menu in menus)
		{
			if (menu != menus[num])
			{
				menu.SetActive(false);
			}
			 else menu.SetActive(true);
		}
	}

    #endregion

    public void PlayClick()
    {
        AudioManager.instance.PlaySFX("Click");
    }

    public void ExitSettings()
    {
        SerializationManager.Save("PlayerData", Player.current, false);
        SceneManager.LoadScene(1);
    }
}
