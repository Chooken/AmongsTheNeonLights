using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBurstScript : MonoBehaviour
{
    // Checks for when all bullets have been destroyed
    void FixedUpdate()
    {
        if (transform.childCount == 0) Destroy(gameObject);
    }
}
