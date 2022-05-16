using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsScript : MonoBehaviour
{
	#region VAR_DECLORATION

	[SerializeField] private GameObject[] menus;

	#endregion

	#region EVENTS
	public void changeMenu(int num)
	{
		foreach (GameObject menu in menus)
		{
			if (menu != menus[num])
			{
				menu.SetActive(false);
			}
			else menu.SetActive(true);
		}
	}

	#endregion
}
