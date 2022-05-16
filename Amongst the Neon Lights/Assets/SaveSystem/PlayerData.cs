[System.Serializable]
public class PlayerData
{
	public int level = 0;

	public bool vsync = false;
	public int fps = -1;
	public bool fpsCounter = false;
	public int display = 0;

	public int attempts = 0;
	
	public int sfxVolume = 0;
	public int musicVolume = 0;

	public int lastlevel = -1;

	public float currentScore = 0f;

	public float speed = 1f;

	public HighScore[] highscores = { new HighScore(), new HighScore(), new HighScore(), new HighScore() };
}

[System.Serializable]
public class HighScore
{
	public float highscore = 0f;
	public bool finished = false;
}
