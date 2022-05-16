using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aarthificial.Reanimation;

public class BeamScript : MonoBehaviour
{
    #region VAR_DECLORATION
    public event Action ShotLeftBeam;
    public event Action ShotRightBeam;

    [SerializeField] Reanimator leftBeam;
    [SerializeField] Reanimator rightBeam;
    [SerializeField] GameObject leftBeamGO;
    [SerializeField] GameObject rightBeamGO;

    #endregion

    #region EVENTS

    private void OnEnable()
    {
        leftBeam.AddListener("Finish", DeactivateBeams);
        rightBeam.AddListener("Finish", DeactivateBeams);
        leftBeam.AddListener("BeamHit", BeamAttackLeft);
        rightBeam.AddListener("BeamHit", BeamAttackRight);
    }

    private void OnDisable()
    {
        leftBeam.RemoveListener("Finish", DeactivateBeams);
        rightBeam.RemoveListener("Finish", DeactivateBeams);
        leftBeam.RemoveListener("BeamHit", BeamAttackLeft);
        rightBeam.RemoveListener("BeamHit", BeamAttackRight);
    }

    public void ShootBeam(bool isLeft)
    {
        if (isLeft)
        {
            leftBeamGO.SetActive(true);
            leftBeam.Set("beam", 0);
            return;
        }

        rightBeamGO.SetActive(true);
        rightBeam.Set("beam", 0);
    }

    public void DeactivateBeams()
    {
        leftBeamGO.SetActive(false);
        rightBeamGO.SetActive(false);
    }

    private void BeamAttackRight()
    {
        ShotRightBeam?.Invoke();
    }

    private void BeamAttackLeft()
    {
        ShotLeftBeam?.Invoke();
    }

    #endregion
}
