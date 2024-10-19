using UnityEngine;

public class BlinkAbility : MonoBehaviour
{
    public float blinkRange = 10f;            // Max blink range
    public float cooldownTime = 5f;           // Cooldown in seconds
    public ParticleSystem blinkEffectPrefab;  // Prefab of the particle effect

    private float cooldownTimer = 0f;         // Timer for cooldown tracking

    void Update()
    {
        // Reduce cooldown timer
        if (cooldownTimer > 0)
            cooldownTimer -= Time.deltaTime;

        // Trigger blink on right-click if cooldown is ready
        if (Input.GetMouseButtonDown(1) && cooldownTimer <= 0)
        {
            PerformBlink();
        }
    }

    void PerformBlink()
    {
        // Get mouse position in the world
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Calculate target position within blink range
            Vector3 targetPosition = hit.point;
            float distance = Vector3.Distance(transform.position, targetPosition);

            if (distance > blinkRange)
            {
                Vector3 direction = (hit.point - transform.position).normalized;
                targetPosition = transform.position + direction * blinkRange;
            }

            // Play particle effect at current position
            if (blinkEffectPrefab != null)
                PlayBlinkEffect(transform.position);

            // Move player to target position
            transform.position = targetPosition;

            // Play particle effect at new position
            if (blinkEffectPrefab != null)
                PlayBlinkEffect(targetPosition);

            // Start cooldown
            cooldownTimer = cooldownTime;
        }
    }

    // Function to play the blink effect at a specific position
    void PlayBlinkEffect(Vector3 position)
    {
        ParticleSystem effect = Instantiate(blinkEffectPrefab, position, Quaternion.identity);
        effect.Play();

        // Destroy the effect after it finishes to avoid clutter
        Destroy(effect.gameObject, effect.main.duration);
    }
}
