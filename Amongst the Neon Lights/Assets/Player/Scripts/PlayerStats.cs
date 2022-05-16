using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aarthificial.Reanimation;

public class PlayerStats : MonoBehaviour
{
    #region VAR_DECLORATION

    [SerializeField] private LayerMask blockerMaskLeft;
    [SerializeField] private LayerMask blockerMaskRight;

    [SerializeField] BeamScript beam;
    [SerializeField] GameScript game;

    [SerializeField] bool isInvincible;

    private Reanimator reanimator;

    #endregion

    private void Awake()
    {
        beam.ShotLeftBeam += checkBeamHitLeft;
        beam.ShotRightBeam += checkBeamHitRight;

        reanimator = GetComponent<Reanimator>();
    }

	#region EVENTS

	private void OnEnable()
    {
        reanimator.AddListener("footstep", PlayFootStep);
    }

    private void OnDisable()
    {
        reanimator.RemoveListener("footstep", PlayFootStep);
    }

    // Hit Detection
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile") Dead();
    }

	#endregion

	public void PlayFootStep()
    {
        AudioManager.instance.PlaySFX("FootStep");
    }

    // Checks for beam hit
    #region BEAM_CHECKS
    private void checkBeamHitLeft()
    {
        if (Physics2D.Raycast(transform.position, Vector2.left, 200, blockerMaskLeft)) return;

        Dead();
    }

    private void checkBeamHitRight()
    {
        if (Physics2D.Raycast(transform.position, Vector2.right, 200, blockerMaskRight)) return;

        Dead();
    }

    #endregion

    private void Dead()
    {
        if (isInvincible) return;

        StartCoroutine(game.PlayerDeath());
    }
}
