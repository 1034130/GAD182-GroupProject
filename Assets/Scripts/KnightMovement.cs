using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls knight charging movement during a joust round.
/// Called by JoustGameManager to start and reset each round.
/// </summary>
public class KnightMovement : MonoBehaviour
{
    public float chargeSpeed = 5f;      // Knight charging speed
    private Vector3 targetPosition;        // Where the knight charges toward
    private bool isCharging = false;

    // Reference to an externally assigned starting position
    public Transform startPoint;

    void Update()
    {
        if (!isCharging) return;

        // Move toward target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, chargeSpeed * Time.deltaTime);
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
    }
}
