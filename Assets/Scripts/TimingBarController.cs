using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Controls the horizontal timning bar mechanic used in the jousting game.
/// A marker moves from left to right, and the player must press a key to stop it within a target zone.
/// The result is evaluated based on whether the marker lands in a green zone or sweet spot.
/// </summary>
public class TimingBarController : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform marker;        // The moving marker (e.g., a small arrow or box)
    public RectTransform barArea;       // The full width of the timing bar container
    public RectTransform greenZone;     // The general "safe" zone
    public RectTransform sweetSpot;     // The "perfect timing" sweet spot

    [Header("Timing Settings")]
    public float markerSpeed = 1.0f;    // How fast the marker moves (in normalized units per second
    public bool isMoving = false;       // Whether the marker is currently moving
        
    private float barWidth;             // Cached width of the barArea for calculations
    private float markerProgress;       // 0 to 1 progress across the bar (0 = left, 1 = right

    // Delegate for broadcasting result once marker stops
    public System.Action<HitResult> OnMarkerStopped;

    // Enum describing result of timing input
    public enum HitResult { Miss, GreenZone, SweetSpot }

    void Start()
    {   
        // Cache the width of the timing bar once on startup
        barWidth = barArea.rect.width;
    }

    void Update()
    {
        if (!isMoving) return;

        // Move the marker from left to right based on speed and time
        markerProgress += markerSpeed * Time.deltaTime;

        // Clamp to ensure it doesn't go past the end
        markerProgress = Mathf.Clamp01(markerProgress);

        // Update the marker's horizontal position on the bar using anchored position
        float xPos = markerProgress * barWidth;
        marker.anchoredPosition = new Vector2(xPos, marker.anchoredPosition.y);

        // If the marker reaches the end without the player sopping it, auto-evaluate
        if (markerProgress >= 1f)
        {
            StopAndEvaluate();
        }
    }

    /// <summary>
    /// Begins the timing bar movement from left to right.
    /// </summary>
    /// <param name="speed">How fast the marker moves (higher = more difficult)</param>
    public void StartBar(float speed)
    {
        markerSpeed = speed;
        markerProgress = 0f;
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

        // Get bounds of green zone
        float greenStart = greenZone.anchoredPosition.x - greenZone.rect.width / 2;
        float greenEnd = greenZone.anchoredPosition.x + greenZone.rect.width / 2;

        // Get bounds of sweet spot (inside green zone)
        float sweetStart = sweetSpot.anchoredPosition.x - sweetSpot.rect.width / 2;
        float sweetEnd = sweetSpot.anchoredPosition.x + sweetSpot.rect.width / 2;

        HitResult result;

        // Determine result based on where the marker landed
        if (markerX >= sweetStart && markerX <= sweetEnd)
            result = HitResult.SweetSpot;
        else if (markerX >= greenStart && markerX <= greenEnd)
            result = HitResult.GreenZone;
        else
            result = HitResult.Miss;

        // Broadcasst result to any listeners (likely the JoustGameManager)
        OnMarkerStopped?.Invoke(result);
    }

    /// <summary>
    /// Can be called externally to stop and evaluate early (e.g. after timeout).
    /// </summary>
    public void ForceStop()
    {
        StopAndEvaluate();
    }
}
