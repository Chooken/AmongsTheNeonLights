using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aarthificial.Reanimation;

public class BeamerScript : MonoBehaviour
{

	#region VAR_DECLORATION

	[System.Serializable]
	class BeamEvents
	{
		public bool isLeft = true;
		public float startHeight = 4f;
		public float endHeight = -4f;
	}

	[Header("Core")]
	[SerializeField] Reanimator beamAni;

	[SerializeField] GameObject BeamLeft;
	[SerializeField] GameObject BeamRight;

	[Header("Events")]
	[SerializeField] List<BeamEvents> beamEvents;

	private bool isBeaming = false;

	#endregion

	#region EVENTS

	public void OnBeamRight()
	{
		BeamRight.SetActive(true);
	}

	public void OnBeamLeft()
	{
		BeamLeft.SetActive(true);
	}

	#endregion

	private void OnEnable()
	{
		beamAni.AddListener("beamLeft", OnBeamLeft);
		beamAni.AddListener("beamRight", OnBeamRight);
	}

	private void OnDisable()
	{
		beamAni.RemoveListener("beamLeft", OnBeamLeft);
		beamAni.RemoveListener("beamRight", OnBeamRight);
	}

	void FixedUpdate()
    {
		if (beamEvents.Count == 0) return;

		if (beamEvents[0].endHeight > gameObject.transform.position.y)
		{
			if (beamEvents[0].isLeft)
			{
				BeamLeft.SetActive(false);
			}
			else
			{
				BeamRight.SetActive(false);
			}
			isBeaming = false;
			beamEvents.RemoveAt(0);

			return;
		}

		if (isBeaming) return;

        if (beamEvents[0].startHeight > gameObject.transform.position.y)
		{
			if (beamEvents[0].isLeft)
			{
				beamAni.Set("beam", 1);
			}
			else
			{
				beamAni.Set("beam", 2);
			}
			isBeaming = true;
		}
    }
}
