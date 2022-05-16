using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aarthificial.Reanimation;

public class VelocityScript : MonoBehaviour
{
    #region VAR_DECLORATION

    [SerializeField] private Vector2 velocity;
    [SerializeField] private bool isForward;
    [SerializeField] private float boost = 1f;
    [SerializeField] private bool isFlipped;

    private Rigidbody2D _rigidbody;

    #endregion

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _rigidbody.velocity = (isForward) ? -transform.up * boost : (Vector3)velocity * boost;
        if (GetComponent<Reanimator>() != null) GetComponent<Reanimator>().Flip = isFlipped;
    }

    private void OnDisable()
    {
        _rigidbody.velocity = Vector2.zero;
    }
}
