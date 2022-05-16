using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    #region VAR_DECLORATION

    [SerializeField] private bool isIdle = true;

    [Header("BodyParts")]
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject hands;

    private float _clock;
    private Vector3 handsMovement = new Vector3(0f, 1f / 16f, 0f);

    #endregion

    void Update()
    {
        AdvanceClock();
    }

    private void AdvanceClock()
    {
        // creates a 1 second timer to update boss logic
        _clock += Time.deltaTime;
        while (_clock >= 1)
        {
            _clock -= 1;
            UpdateBoss();
        }
    }

    private void UpdateBoss()
    {
        // Updates all of the boss logic
        if (!isIdle) return;

        Debug.Log(handsMovement);
        hands.transform.Translate(handsMovement);
        handsMovement = -handsMovement;
    }
}
