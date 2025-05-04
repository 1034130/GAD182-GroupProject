using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles click detection on the sword object and informs the BlacksmithGameMaanager.
/// </summary>
public class SwordClickHandler : MonoBehaviour
{
    public BlacksmithGameManager gameManager;

    private void OnMouseDown()
    {
        if (gameManager != null)
        {
            gameManager.AddClickScore();
        }
    }
}
