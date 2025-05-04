using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls knight charging movement during a joust round.
/// Called by JoustGameManager to start and reset each round.
/// </summary>
public class KnightMovement : MonoBehaviour
{
    [Header("Movement")]
    public float chargeSpeed = 5f;      // Knight charging speed
    public Transform startPoint;        // Reference to an externally assigned starting position
    private Vector3 targetPosition;     // Where the knight charges toward
    private bool isCharging = false;

    [Header("Sprites")]
    public Sprite idleSide1;
    public Sprite chargeSide1;
    public Sprite idleSide2;
    public Sprite chargeSide2;

    private SpriteRenderer spriteRenderer;
    private bool isSide1 = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!isCharging) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, chargeSpeed * Time.deltaTime);
        // We no longer reset the sprite when reaching the destination
    }

    ///<summary>
    ///Called by GameManager to set the knight's side (for flipping sprite variation).
    ///</summary>
    public void SetSide(bool side1)
    {
        isSide1 = side1;
        UpdateSprite(false);
    }

    ///<summary>
    /// Starts the knight charging toward aa target.
    ///</summary>
    public void StartCharge(Vector3 centerPoint)
    {
        targetPosition = centerPoint;
        isCharging = true;
    }

    ///<summary>
    /// Resets the knight to the original position and stops charging.
    ///</summary>
    public void ResetKnight()
    {
        transform.position = startPoint.position;
        isCharging = false;
        UpdateSprite(false);
    }

    ///<summary>
    ///Applies the correct sprite based on charge state and side.
    ///</summary>
    private void UpdateSprite(bool charging)
    {
        if (charging)
            spriteRenderer.sprite = isSide1 ? chargeSide1 : chargeSide2;
        else
            spriteRenderer.sprite = isSide1 ? idleSide1 : idleSide2;
    }

    /// <summary>
    /// Allows external control of sprite change to charging state (used by GameManager).
    /// </summary>
    public void SetCharging(bool charging)
    {
        isCharging = charging;
        UpdateSprite(charging);
    }

    ///<summary>
    ///Applies a new sprite set (called when selected knight changes).
    ///</summary>
    public void AssignSpriteSet(KnightSpriteSet newSet)
    {
        idleSide1 = newSet.idleSide1;
        chargeSide1 = newSet.chargeSide1;
        idleSide2 = newSet.idleSide2;
        chargeSide2 = newSet.chargeSide2;

        UpdateSprite(false);    // Update to idle immediately
    }
}
