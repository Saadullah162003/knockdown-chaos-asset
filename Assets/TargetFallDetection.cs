using UnityEngine;

public class TargetFallDetection : MonoBehaviour
{
    public ScoreManager scoreManager; // Reference to the ScoreManager
    private bool hasFallen = false;
    private bool hasCollidedWithBall = false;

    void Start()
    {
        // Find the ScoreManager in the scene
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the target has collided with the ball
        if (collision.gameObject.CompareTag("Player"))
        {
            // Update score for the initial target collision
            if (!hasCollidedWithBall)
            {
                hasCollidedWithBall = true;
                CheckAndHandleFall();
                Debug.Log("Target collided with the ball and is checked for fall");
            }
        }
    }

    void Update()
    {
        // Check if the target has fallen after colliding with the ball
        if (!hasFallen && transform.position.y < 0.5f) // Adjust the y position threshold as needed
        {
            CheckAndHandleFall();
        }
    }

    private void CheckAndHandleFall()
    {
        if (!hasFallen && (hasCollidedWithBall || IsNearbyFallenTarget()))
        {
            hasFallen = true;
            scoreManager.AddScore(10); // Add points as needed
            Debug.Log("Target has fallen");
        }
    }

    private bool IsNearbyFallenTarget()
    {
        // Check if nearby targets have fallen
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.0f); // Adjust radius as needed
        foreach (Collider collider in colliders)
        {
            TargetFallDetection nearbyTarget = collider.GetComponent<TargetFallDetection>();
            if (nearbyTarget != null && nearbyTarget.hasFallen)
            {
                return true;
            }
        }
        return false;
    }
}
