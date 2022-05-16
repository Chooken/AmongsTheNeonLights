using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectKiller : MonoBehaviour
{
    #region VAR_DECLORATION

    Vector3 spawnHeight = new Vector3(0f, 5.5f, 0f);

    #endregion

    #region EVENTS

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Foreground"))
        {
            collision.gameObject.transform.position = spawnHeight;
            return;
        }

        // Destroys anything that not the player enters the trigger
        if (!collision.gameObject.CompareTag("Player")) Destroy(collision.gameObject);
    }

    #endregion
}
