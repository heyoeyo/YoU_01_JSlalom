using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameStage {

	// Controls
	[Min(0)] public float stage_duration = 10;
	[SerializeField] public StageParams stage_params;

	// Timing variables - need to be set after startup sequence is known
	private float transition_duration;
	private float stage_start_time;
	private float trans_start_time;
	private float stage_end_time;

	// Used to handle transitions
	private StageParams next_stage_params;

	public void SetTimes(float start_time, float transition_duration) {
		this.stage_start_time = start_time;
		this.trans_start_time = start_time + this.stage_duration;

		this.transition_duration = transition_duration;
		this.stage_end_time = this.trans_start_time + this.transition_duration;
    }

	public void SetNextStageParams(StageParams next_stage_params) {
		this.next_stage_params = next_stage_params;
    }

	public StageParams GetCurrentParams(float current_time) {

		if (current_time < trans_start_time) {
			return this.stage_params;
		}

		float transistion_progress = (current_time - trans_start_time) / transition_duration;
		return StageParams.Lerp(this.stage_params, this.next_stage_params, transistion_progress);
	}

	public bool IsActive(float current_time) {
		bool after_start_time = (current_time >= stage_start_time);
		bool before_end_time = (current_time < stage_end_time);
		return (after_start_time && before_end_time);
	}
}

