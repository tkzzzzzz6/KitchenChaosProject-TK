using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private Player player;
    private float stepSoundRate = 0.2f;
    private float stepSoundTimer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        stepSoundTimer += Time.deltaTime;
        if (stepSoundTimer >= stepSoundRate)
        {
            stepSoundTimer = 0f;
            if (!player.IsWalking) return;
            float volume = 0.5f;
            SoundManager.Instance.PlayStepSound(volume);
        }
    }
}
