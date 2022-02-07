using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSequence : MonoBehaviour {

    // Controls/inputs
    [SerializeField] private PlayerCollision player_life;
    [SerializeField] private float transition_duration = 5f;
    [SerializeField] private List<GameStage> stage_sequence;
    [SerializeField] private StageParams last_stage_params;

    // Main outputs
    [HideInInspector] public StageParams curr_params;
    [HideInInspector] public bool stage_changed;

    // Convenience
    private const int last_stage_idx = 10000;

    // Bookkeeping
    private float last_stage_start_time;
    private int curr_idx;
    private bool game_is_over;

    private void Awake() {

        // Initialize state
        last_stage_start_time = 1;
        curr_idx = -1;
        game_is_over = false;

        // Stages
        SetupAllStages();
        UpdateCurrentParams();
    }

    private void Start() {
        player_life.OnPlayerDeath += HandlePlayerDeath;
    }

    private void FixedUpdate() {
        if (!game_is_over)
            UpdateCurrentParams();
    }

    private void UpdateCurrentParams() {

        float curr_time = Time.timeSinceLevelLoad;
        int prev_curr_idx = curr_idx;

        // Get current stage parameters (which behaves differently for final stage vs others)
        bool on_last_stage = (curr_time >= last_stage_start_time);
        if (on_last_stage) {
            curr_idx = last_stage_idx;
            curr_params = last_stage_params;

        } else {

            // Figure out which stage we should currently be using
            bool set_active_stage = false;
            for (int i = 0; i < stage_sequence.Count; i++) {
                set_active_stage = stage_sequence[i].IsActive(curr_time);
                if (set_active_stage) {
                    curr_idx = i;
                    break;
                }
            }

            // If we didn't set a stage, force it to be the last stage
            if (!set_active_stage) {
                curr_idx = last_stage_idx;
                curr_params = last_stage_params;
            } else {
                curr_params = stage_sequence[curr_idx].GetCurrentParams(curr_time);
            }
        }

        // Signal to spawners whether the stage changed... hacky feeling
        stage_changed = (curr_idx != prev_curr_idx);
    }

    private void SetupAllStages() {

        /*
        Function used to update the start/end/transition times of each stage
        -> Note the stages cannot do this by themselves, since they are not aware of their sequencing
        */

        // Set up stage starting/transition timing
        float stage_start_time = 0f;
        foreach (GameStage stage in stage_sequence) {
            stage.SetTimes(stage_start_time, this.transition_duration);
            stage_start_time += stage.stage_duration + this.transition_duration;
        }

        // Store final stage timing, so we know when to switch!
        last_stage_start_time = stage_start_time;

        // Set up stage transition parameters
        for (int i = 0; i < stage_sequence.Count - 1; i++) {
            GameStage curr_stage = stage_sequence[i];
            GameStage next_stage = stage_sequence[i + 1];
            curr_stage.SetNextStageParams(next_stage.stage_params);
        }

        // Special case to link last given stage to end (infinite) stage
        GameStage pre_last_stage = stage_sequence[stage_sequence.Count - 1];
        pre_last_stage.SetNextStageParams(last_stage_params);
    }

    private void HandlePlayerDeath() {
        game_is_over = true;
    }
}
