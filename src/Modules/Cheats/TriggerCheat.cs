using UnityCheatTemplate.Interfaces;
using UnityEngine;

namespace UnityCheatTemplate.Modules.Cheats;

/// <summary>
/// Provides trigger bot functionality for automatically targeting and interacting with nearby game objects.
/// This class implements the singleton pattern to ensure only one instance exists throughout the application.
/// </summary>
internal sealed class TriggerCheat : ISingleton
{
    /// <summary>
    /// Retrieves all MonoBehaviour components that should be considered for trigger bot targeting.
    /// </summary>
    /// <returns>An array of MonoBehaviour components to evaluate for triggering.</returns>
    private static MonoBehaviour[] GetMonoBehaviours()
    {
        return [];
    }

    private static MonoBehaviour? triggerObject;

    /// <summary>
    /// Updates the trigger bot logic each frame. This method should be called from Unity's Update() method.
    /// Evaluates nearby objects and selects the best target based on screen position and angle.
    /// </summary>
    internal void Update()
    {
        triggerObject = null;
        var monos = GetMonoBehaviours();
        if (monos.Length <= 0) return;

        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        float defaultMaxAngle = 1.2f;
        float closestAngle = float.MaxValue;
        MonoBehaviour? closestMono = null;

        foreach (var mono in monos)
        {
            if (mono == null) continue;

            Vector3 pos = Vector3.zero;
            float currentMaxAngle = defaultMaxAngle;

            // Make logic to get Specified components position and angle
            {

            }

            if (currentMaxAngle < 0.5f)
            {
                currentMaxAngle = 0.5f;
            }

            Vector3 screenPoint = mainCamera.WorldToViewportPoint(pos);

            if (screenPoint.z > 0 &&
                screenPoint.x > 0 && screenPoint.x < 1 &&
                screenPoint.y > 0 && screenPoint.y < 1)
            {
                Vector2 viewportCenter = new(0.5f, 0.5f);
                Vector2 objectPos = new(screenPoint.x, screenPoint.y);
                float angle = Vector2.Angle(viewportCenter, objectPos);

                if (angle < currentMaxAngle && angle < closestAngle)
                {
                    closestAngle = angle;
                    closestMono = mono;
                }
            }
        }

        triggerObject = closestMono;
        Trigger();
    }

    /// <summary>
    /// Performs the trigger action when conditions are met. This method is called automatically by Update().
    /// Currently checks for middle mouse button press but requires additional logic for actual triggering.
    /// </summary>
    private static void Trigger()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse2)) return;


    }

    /// <summary>
    /// Adjusts the maximum target angle based on a Rigidbody's mass and velocity.
    /// Objects with higher mass and velocity get a larger target angle tolerance.
    /// </summary>
    /// <param name="rb">The Rigidbody component to evaluate for angle adjustment.</param>
    /// <param name="currentMaxAngle">Reference to the current maximum angle value to be modified.</param>
    /// <param name="defaultMaxAngle">The default maximum angle value to use as a baseline.</param>
    private void SetAngleFromRigidBody(Rigidbody rb, ref float currentMaxAngle, float defaultMaxAngle)
    {
        if (rb != null)
        {
            float massFactor = Mathf.Clamp(rb.mass, 0.1f, 10f) / 10f;
            float velocityFactor = Mathf.Clamp(rb.velocity.magnitude, 0f, 10f) / 10f;
            currentMaxAngle = defaultMaxAngle * (1f + massFactor + velocityFactor);
        }
    }
}