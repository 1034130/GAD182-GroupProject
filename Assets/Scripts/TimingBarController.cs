using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Moves a UI marker (RectTransform) back and forth between two positions on the timing bar.
/// Evaluates input based on distance to sweet spot in normalized space.
/// </summary>
public class TimingBarController : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform marker;        // The moving marker (e.g., a small arrow or box)
    public RectTransform startPoint;    // Left point of the bar
    public RectTransform endPoint;      // Right point of the bar
    public RectTransform greenZoneStart;// Left point of the green zone
    public RectTransform greenZoneEnd;  // Right point of the green zone
    public RectTransform sweetSpotStart;// Left point of the sweet spot
    public RectTransform sweetSpotEnd;  // Right point of the sweet spot

    [Header("Movement Settings")]

    public float markerSpeed = 1f;    // How fast the marker moves (in normalized units per second
    public bool isMoving = false;       // Whether the marker is actively moving

    private float timeCounter = 0f;     // Time counter for PingPong motion
    private float totalDistance;
   
  
    // Delegate for broadcasting result once marker stops
    public System.Action<HitResult> OnMarkerStopped;

    // Enum describing result of timing input
    public enum HitResult { Miss, GreenZone, SweetSpot }

    void Start()
    {
        // Cache the inital anchored positoon as the left edge
        totalDistance = Vector2.Distance(startPoint.anchoredPosition, endPoint.anchoredPosition);
    }

    void Update()
    {
        if (!isMoving) return;

        timeCounter += Time.deltaTime * markerSpeed;

        // Calculate back and forth motion between 0 and moveDistance
        float t = Mathf.PingPong(timeCounter, 1f);
        marker.anchoredPosition = Vector2.Lerp(startPoint.anchoredPosition, endPoint.anchoredPosition, t);
        
    }

    /// <summary>
    /// Begins the timing bar movement from left to right.
    /// </summary>
    /// <param name="speed">How fast the marker moves (higher = more difficult)</param>
    public void StartBar()
    {
        timeCounter = 0f;
        marker.anchoredPosition = startPoint.anchoredPosition; 
        isMoving = true;
    }

    /// <summary>
    /// Stops the marker and determines if the player landed in a valid zone.
    /// Triggers the OnMarkerStopped event with the result.
    /// </summary>
    public void StopAndEvaluate()
    {
        isMoving = false;

        float markerX = marker.anchoredPosition.x;

        float greenStart = greenZoneStart.anchoredPosition.x;
        float greenEnd = greenZoneEnd.anchoredPosition.x;
        float sweetStart = sweetSpotStart.anchoredPosition.x;
        float sweetEnd = sweetSpotEnd.anchoredPosition.x;

        HitResult result;

        if (markerX >= sweetStart && markerX <= sweetEnd)
            result = HitResult.SweetSpot;
        else if (markerX >= greenStart && markerX <= greenEnd)
            result = HitResult.GreenZone;
        else
            result = HitResult.Miss;

        OnMarkerStopped?.Invoke(result);
    }

    /// <summary>
    /// Can be called externally to stop and evaluate early (e.g. after timeout).
    /// </summary>
    public void ForceStop()
    {
        StopAndEvaluate();
    }

    public void ResetMarkerPosition()
    {
        timeCounter = 0f;
        marker.anchoredPosition = startPoint.anchoredPosition;
    }
}
