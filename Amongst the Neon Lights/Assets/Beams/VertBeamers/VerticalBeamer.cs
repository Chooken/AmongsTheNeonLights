using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aarthificial.Reanimation;

public class VerticalBeamer : MonoBehaviour
{

    #region VAR_DECLORATION

    [SerializeField] private Reanimator reanimator;

    [SerializeField] private BoxCollider2D beamCollider;

    [SerializeField] private int beamDuration = 5;

    private int beamCurrentDuration;

    private bool toBeDestroyed = false;

    #endregion

    #region EVENTS

    private void OnEnable()
    {
        reanimator.AddListener("hurtBeam", OnBeamDamageEnable);
        reanimator.AddListener("destroyBeam", DestroyBeam);
    }

    private void OnDisable()
    {
        reanimator.RemoveListener("hurtBeam", OnBeamDamageEnable);
        reanimator.RemoveListener("destroyBeam", DestroyBeam);
    }

    private void OnBeamDamageEnable()
    {
        if (beamCurrentDuration > beamDuration) DisableBeam();

        beamCurrentDuration++;

        if (beamCollider.enabled) return;

        beamCollider.enabled = true;
    }

    #endregion

    private void DisableBeam()
    {
        reanimator.Set("isBeaming", 1);
    }

    private void DestroyBeam()
    {
        //Destroy(transform.parent.gameObject);

        toBeDestroyed = true;
    }

    private void LateUpdate()
    {
        if (toBeDestroyed) Destroy(gameObject);
    }
}
