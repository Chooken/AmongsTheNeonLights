using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZaggerScript : MonoBehaviour
{
	#region VAR_DECLORATIONS

	[SerializeField] private float period;
	[SerializeField] private int sections;
	[SerializeField] private GameObject projectile;
	[SerializeField] private float length;

	private GameScript game;
	private float firstScore;
	private float lastScore;
	private int spawnPos;
	private int dir = 1;

	#endregion

	private void Start()
	{
		game = GameObject.Find("Game").GetComponent<GameScript>();
		firstScore = game.score;
		lastScore = game.score;
	}

	private void FixedUpdate()
	{
		if (game.score - lastScore < period) return;

		lastScore = game.score;

		SpecSpawn(projectile);

		dir = (spawnPos + dir > sections || spawnPos + dir < 1) ? -dir : dir;

		spawnPos += dir;

		if (game.score - firstScore >= length) Destroy(gameObject);
	}

	private void SpecSpawn(GameObject gameObject)
	{
		float prevSection = -3.6f;

		float sectionSize = 7.2f / (sections * 2);

		Instantiate(gameObject, new Vector2(prevSection + sectionSize + (sectionSize * 2 * (spawnPos - 1)), 4.12f + gameObject.transform.position.y), Quaternion.identity, transform.parent);
		return;
	}
}
