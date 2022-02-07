using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {

    [SerializeField] private bool enable_death = true;
    [SerializeField] private AudioSource explosion_sound_1;
    [SerializeField] private AudioSource explosion_sound_2;
    [SerializeField] private ParticleSystem explosion_particles;

    public event System.Action OnPlayerDeath;

    private bool player_alive;

    private void Awake() {
        player_alive = true;
        explosion_particles.Stop();
    }

    private void OnTriggerEnter(Collider other) {
        if (player_alive && enable_death) {
            TriggerExplosion();
            TriggerPlayerDeathEvent();
            Destroy(gameObject);
            player_alive = false;
        }
    }

    private void TriggerPlayerDeathEvent() {
        if (OnPlayerDeath != null)
            OnPlayerDeath();
    }

    private void TriggerExplosion() {

        // Trigger sound effect
        bool play_sound_1 = (Random.Range(0, 2) > 0);
        AudioSource selected_sound = play_sound_1 ? explosion_sound_1 : explosion_sound_2;
        selected_sound.Play();

        // Trigger particle effects
        explosion_particles.Play();
    }
}
