using UnityEngine;

public enum PowerUpType { JumpBoost, SpeedBoost }

public class PowerUp : MonoBehaviour
{
    public PowerUpType powerType;
    public float boostAmount = 1.5f;
    public float duration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();

            if (player != null)
            {
                switch (powerType)
                {
                    case PowerUpType.JumpBoost:
                        player.ApplyJumpBoost(boostAmount, duration);
                        break;
                    case PowerUpType.SpeedBoost:
                        player.ApplySpeedBoost(boostAmount, duration);
                        break;
                }
            }
        }
    }
}
