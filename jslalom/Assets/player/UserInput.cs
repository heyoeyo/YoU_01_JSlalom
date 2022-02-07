using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour {
    // Controls
    [SerializeField] private PlayerCollision player_life;
    [SerializeField, Range(1f, 15)] private float momentum_accumulation_rate = 6f;
    [SerializeField, Range(1f, 15f)] private float momentum_decay_rate = 4f;

    // Constants
    private static float _max_mom_pct = 100f;
    private static float _min_mom_pct = 0.01f;

    // User input variables
    private float in_x;
    private bool user_is_strafing;
    private float _strafe_mom_pct = 0f;
    private bool player_is_dead;

    // Outward facing properties
    public float strafe_momentum;

    private void Start() {
        player_is_dead = false;
        player_life.OnPlayerDeath += HandlePlayerDeath;
    }

    void Update() {
        // Listen for user input
        in_x = player_is_dead ? 0 : Input.GetAxisRaw("Horizontal");
        user_is_strafing = Mathf.Abs(in_x) > 0.1f;
    }

    void FixedUpdate() {

        // Handle momentum updates based on user input
        if (user_is_strafing) {
            _strafe_mom_pct += in_x * momentum_accumulation_rate;
        } else {
            _strafe_mom_pct *= (1f - momentum_decay_rate / 100f);
        }

        // Limit range of momentum values and normalize for use in other calculations
        _strafe_mom_pct = Mathf.Clamp(_strafe_mom_pct, -_max_mom_pct, _max_mom_pct);
        if (Mathf.Abs(_strafe_mom_pct) < _min_mom_pct)
            _strafe_mom_pct = 0f;

        // Update outward facing property
        strafe_momentum = _strafe_mom_pct / _max_mom_pct;

    }

    void HandlePlayerDeath() {
        player_is_dead = true;
    }
}