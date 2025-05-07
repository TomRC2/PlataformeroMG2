using UnityEngine;

public class LaunchPlatform : MonoBehaviour
{
    public Vector3 launchDirection = Vector3.up;
    public float launchForce = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController controller = other.GetComponent<CharacterController>();
            PlayerMovement movement = other.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                movement.ExternalLaunch(launchDirection.normalized * launchForce);
            }
        }
    }
}