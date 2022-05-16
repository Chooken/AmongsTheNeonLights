using System.Collections.Generic;
[System.Serializable]
public class Player
{
	private static Player _current = null;
	public static Player current
	{
		get
		{
			if (_current == null)
			{
				_current = new Player();
			}
			return _current;
		}
		set
		{
			if (value != null)
			{
				_current = value;
			}
		}
	}

	public PlayerData playerData = new PlayerData();
}