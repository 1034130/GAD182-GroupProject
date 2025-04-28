using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Controls the horizontal timing bar mechanic used in the jousting game.
/// Moves a marker across a fixed bar and evaluates player input based on predefined zones.
/// Now works with one static bar image (including green zone and sweet spot) and a separate marker.
/// </summary>
public class TimingBarController : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform marker;        // The moving marker (e.g., a small arrow or box)
    public RectTransform barArea;       // The full width of the timing bar container

    [Header("Timing Settings")]
    public float markerSpeed = 1.0f;    // How fast the marker moves (in normalized units per second
    public bool isMoving = false;       // Whether the marker is currently moving
        
    private float barWidth;             // Cached width of the barArea for calculations
    private float markerProgress;       // 0 to 1 progress across the bar (0 = left, 1 = right

    [Header("Zone Settings")]
    [Range(0f, 1f)]
    public float greenZoneStart = 0.45f;    // Normalised start position of green zone
    [Range(0f, 1f)]
    public float greenZoneEnd = 0.55f;      // Normalised end position of green zone
    [Range(0f, 1f)]
    public float sweetSpotCenter = 0.5f;    // Normalised center of sweet spot
    public float sweetSpotRange = 0.02f;    // Range around center of sweet spot


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

        float markerNormalizedPosition = markerProgress; // Already 0-1

        HitResult result;

        if (markerNormalizedPosition >= (sweetSpotCenter - sweetSpotRange) &&
              markerNormalizedPosition <= (sweetSpotCenter + sweetSpotRange))
        {
            result = HitResult.SweetSpot;
        }
        else if (markerNormalizedPosition >= greenZoneStart && markerNormalizedPosition <= greenZoneEnd)
        {
            result = HitResult.GreenZone;
        }
        else
        {
            result = HitResult.Miss;
        }

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
