using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningSpawner : MonoBehaviour
{
    #region VAR_DECLORATION

    [SerializeField] private GameObject bullet;
    [SerializeField] private Vector3 spin;
    [SerializeField] private float freqency;

    private float time;

    #endregion

    // Update is called once per frame
    void FixedUpdate()
    {
        // Rotates spinner and shots a projectile

        transform.Rotate(spin * Time.deltaTime);

        time += Time.deltaTime;

        if (time < freqency) return;

        time -= freqency;

        Instantiate(bullet, this.transform.position, this.transform.rotation);
    }
}
