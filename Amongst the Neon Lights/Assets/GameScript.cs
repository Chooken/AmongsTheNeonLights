using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour
{
    #region VAR_DECLORATION

    [Serializable]
    public class GameEvents
    {
        public string name;
        public float eventTime;
        public int spawnedObj;
        public int numOfSpawns = 1;
		public bool isTargeted = false;
        public bool isBeam = false;
        public bool isLeft = false;
        public bool specPos = false;
        public int posSeg = 4;
        public int posVal;
        public bool isRandAngle = false;
    }

    [Serializable]
    public class EventObjects
    {
        public GameObject obj;
        public float timeOffset;
    }

    [SerializeField] BeamScript beam;
    [SerializeField] List<GameEvents> gameEvents;
    [SerializeField] EventObjects[] eventObjects;
    [SerializeField] Transform spawnLocation;
    [SerializeField] GameObject fpsCounter;

    [SerializeField] Text timeUI;
    [SerializeField] string levelMusic;
    public float score;
    [SerializeField] int finishTime = 190;

    [SerializeField] CutSceneManager cutSceneManager;

    [SerializeField] int levelNum;

	[SerializeField] GameObject entrance;

	[SerializeField] GameObject playerObj;

    [Header("Debug")]
    [SerializeField] private bool setTime;
    [SerializeField] private int startTime;

    private bool isFinish = false;
    private bool inCutscene = true;

    #endregion

    private void Awake()
    {
		Player.current = (Player)SerializationManager.Load(Application.persistentDataPath + "/saves/PlayerData.save");
		fpsCounter.SetActive(Player.current.playerData.fpsCounter);
        Player.current.playerData.attempts++;
        SerializationManager.Save("PlayerData", Player.current, false);
    }

    private void Start()
    {
		if (Player.current.playerData.lastlevel != -1)
        {
            cutSceneManager.SkipToStart();
            StartGame();
            return;
        }

		AudioManager.instance.StopCurrentMusic();
        Player.current.playerData.lastlevel = levelNum;
        cutSceneManager.StartCutscene(true);
        timeUI.text = $"time: {String.Format("{0:0.0}", score)}";
    }

    public void StartGame()
    {
        inCutscene = false;

        AudioManager.instance.InstaStopMusic();

        AudioManager.instance.Play(levelMusic);

		if (!setTime) 
		{ 
			entrance.transform.position = new Vector3(0, finishTime / 2, 0);
			return;
		}

		entrance.transform.position = new Vector3(0, (finishTime - startTime) / 2, 0);

        List<GameEvents> refGameEvents = new List<GameEvents>(gameEvents);

        foreach (GameEvents gameEvent in refGameEvents)
        {
            if (gameEvent.eventTime < startTime) gameEvents.Remove(gameEvent);
        }

        AudioManager.instance.setSongTime(startTime);
    }

    #region UPDATES

    void Update()
    {
        if (isFinish || inCutscene) return;

        score = AudioManager.instance.songTime();
        timeUI.text = $"time: {String.Format("{0:0.0}", score)}";
    }

    private void FixedUpdate()
    {
        if (score >= finishTime * Time.timeScale && !isFinish) FinishLevel();

        if (gameEvents.Count == 0) return;

        if (gameEvents[0].isBeam)
        {
            if (gameEvents[0].eventTime - 1.5f < score)
            {
                beam.ShootBeam(gameEvents[0].isLeft);

                gameEvents.RemoveAt(0);
            }

            return;
        }

        if (gameEvents[0].eventTime - eventObjects[gameEvents[0].spawnedObj].timeOffset < score)
        {
            if (gameEvents[0].specPos)
            {
                SpecSpawn(eventObjects[gameEvents[0].spawnedObj].obj, gameEvents[0].posVal, gameEvents[0].posSeg);
            }
            else Spawn(eventObjects[gameEvents[0].spawnedObj].obj, gameEvents[0].numOfSpawns, gameEvents[0].spawnedObj <= 1);

            gameEvents.RemoveAt(0);
        }
    }

    #endregion

    #region SPAWNING

    private void SpecSpawn(GameObject gameObject, int posVal, int posSeg)
    {
        float prevSection = -3.3f;

        float sectionSize = 6.6f / (posSeg * 2);

        if (!gameEvents[0].isRandAngle)
        {
            Instantiate(gameObject, new Vector2(prevSection + sectionSize + (sectionSize * 2 * (posVal - 1)), spawnLocation.position.y + gameObject.transform.position.y), Quaternion.identity, transform.parent);
            return;
        }

        Vector3 randAngle = new Vector3(0, 0, UnityEngine.Random.Range(-33, 33));

        Instantiate(gameObject, new Vector2(prevSection + sectionSize + (sectionSize * 2 * (posVal - 1)), spawnLocation.position.y + gameObject.transform.position.y), Quaternion.Euler(randAngle), transform.parent);
    }

    private void Spawn(GameObject gameObject, int numOfSpawns, bool isBlocker)
    {
        float prevSection = isBlocker ? -2.2f : -3.3f;

        float sectionSize = isBlocker ? 4.4f : 6.6f;
        sectionSize /= numOfSpawns;

        if (!gameEvents[0].isRandAngle)
        {
			if (gameEvents[0].isTargeted)
			{
				for (int i = 1; i <= numOfSpawns; i++)
				{
					Instantiate(gameObject, new Vector2(playerObj.transform.position.x, spawnLocation.position.y + gameObject.transform.position.y), Quaternion.identity, transform.parent);
					prevSection += sectionSize;
				}
				return;
			}

            for (int i = 1; i <= numOfSpawns; i++)
            {
                float randomXPos = UnityEngine.Random.Range(prevSection, prevSection + sectionSize);
                Instantiate(gameObject, new Vector2(randomXPos, spawnLocation.position.y), Quaternion.identity, transform.parent);
                prevSection += sectionSize;
            }
            return;
        }

        Vector3 randAngle;

        for (int i = 1; i <= numOfSpawns; i++)
        {
            randAngle = Vector3.forward * UnityEngine.Random.Range(-33, 33);
            float randomXPos = UnityEngine.Random.Range(prevSection, prevSection + sectionSize);
            Instantiate(gameObject, new Vector2(randomXPos, spawnLocation.position.y), Quaternion.Euler(randAngle), transform.parent);
            prevSection += sectionSize;
        }
    }

    #endregion

    public void GoToScene(int sceneInt)
    {
        SceneManager.LoadScene(sceneInt);
    }

    public IEnumerator PlayerDeath()
    {
        AudioManager.instance.StopCurrentMusic();
        Player.current.playerData.currentScore = score;

        yield return new WaitForSeconds(0.05f);
        GoToScene(5);
    }

    private void FinishLevel()
    {
        score = finishTime;
        timeUI.text = "time: " + String.Format("{0:0.0}", score);
		timeUI.gameObject.SetActive(false);
        isFinish = true;

        Player.current.playerData.level = levelNum + 1;
        Player.current.playerData.highscores[levelNum].finished = true;
        Player.current.playerData.highscores[levelNum].highscore = score;
        SerializationManager.Save("PlayerData", Player.current, false);
		cutSceneManager.StartCutscene(false);
    }
}
