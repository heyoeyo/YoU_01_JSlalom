using UnityEngine;

public class SmoothCorridorSpawner : _Spawner {

    [SerializeField] private float envelope_duration = 3;
    [SerializeField] private float corridor_separation = 10f;
    [SerializeField, Range(0.1f, 5)] private float min_shift_duration = 1f;
    [SerializeField, Range(1f, 10)] private float max_shift_duration = 3f;
    [SerializeField, Range(0f, 1f)] private float min_shift_difficulty = 0.25f;
    [SerializeField, Range(0f, 1f)] private float max_shift_difficulty = 1f;

    // Start envelope properties (used to initially funnel player into corridor)
    float envelope_height;
    float env_a;
    float env_b;
    float env_c;

    // Shift-targeting variables (used to move corridor around over time)
    StartEndFloat target_time;
    StartEndFloat target_x;

    // Hard-code slope correction factor for cosine interpolation
    //  Corridor is regularly shifted by some amount X over random durations t
    //  The shifting distance must be limited, so that the player is able to
    //  navigate these shifts (according to the player's max strafing speed)
    //  However, the corridor shifting is not 'shaped' linearly, instead it uses
    //  cosine interpolation to smoothly transistion between the start/end x positions.
    //  A cosine curve will change x values more quickly over time compared to a linear shift
    //  The steepest part of the cosine curve is a factor of: (PI / 2) times
    //  steeper than a linear interpolation over the same X & t, which this term accounts for!
    const float cos_slope_correction_Factor = 2 / Mathf.PI;

    // Avoid re-initializing these every update
    float spawn_angle;
    float spawn_hue;
    
    protected override void Init() {

        // Pre-calculate envelope size so that it covers the entire player screen
        envelope_height = max_spawn_x / corridor_separation;

        // Pre-calculate the quadratic terms used in the envelope (env = ax^2 + bx + c)
        // Target behavior is to have following properties:
        //   env(x = 0) = height
        //   env(x = duration) = 1
        //   slope of env(x = duration) = 0
        env_a = (envelope_height - 1) / (envelope_duration * envelope_duration);
        env_b = -2 * (envelope_height - 1) / envelope_duration;
        env_c = envelope_height;

        // Set initial targeting point
        target_time = new StartEndFloat(0, spawner_init_time);
        target_x = new StartEndFloat(0, spawner_init_origin.x);
        UpdateTargetingParameters();
    }

    
    void UpdateTargetingParameters() {

        // Calculate largest shift amount, based on max rate & shift duration
        //target_time.delta = Random.Range(min_shift_duration, max_shift_duration);
        float time_delta = Random.Range(min_shift_duration, max_shift_duration);
        target_time.StepUpdate(time_delta);

        // Figure out x-shifting limits (so that player speed is able to keep up with shifting)
        float linear_max_x_shift = player_speed.x * time_delta;
        float max_allowable_shift = linear_max_x_shift * cos_slope_correction_Factor;

        // Update target x values for spawn-x calculation
        float random_sign = Random.Range(0, 2) > 0 ? -1f : 1f;
        float max_shift_x = max_shift_difficulty * max_allowable_shift;
        float min_shift_x = min_shift_difficulty * max_shift_x;
        float x_delta = Random.Range(min_shift_x, max_shift_x) * random_sign;
        target_x.StepUpdate(x_delta);
    }


    private float CalculateSpawnX(float current_time) {

        // Function which uses cosine interpolation between start/end target x values (over target time duration)

        float normalized_time = (current_time - target_time.start) / target_time.delta;
        float cos_zero_to_one = (1f - Mathf.Cos(Mathf.PI * normalized_time)) / 2f;
        float cos_interpolation = target_x.start + cos_zero_to_one * target_x.delta;

        return cos_interpolation;
    }

    public override SpawnParams[] Spawn(float current_time, Vector3 spawn_origin) {

        float spawn_x = CalculateSpawnX(current_time);
        if (current_time > target_time.end)
            UpdateTargetingParameters();

        return SpawnPair(current_time, spawn_x);
    }

    private SpawnParams[] SpawnPair(float current_time, float center_x) {

        // Calculate enveloping, to deal with initial funnelling into corridor
        float stage_time = TimeSinceStart(current_time);
        float curr_envelope = GetEnvelope(stage_time);

        // Generate offset x co-ords for pair
        float curr_separation = (corridor_separation * curr_envelope);
        float x_left = center_x - curr_separation;
        float x_right = center_x + curr_separation;

        // Generate (shared) hue and angle
        spawn_angle = (0.01f * stage_time) % 180f;
        spawn_hue = (0.2f * stage_time) % 1f;

        // Generate a pair of spawns
        SpawnParams spawn_left = new SpawnParams(x_left, spawn_angle, spawn_hue);
        SpawnParams spawn_right = new SpawnParams(x_right, spawn_angle, spawn_hue);
        return new SpawnParams[] { spawn_left, spawn_right };
    }

    private float GetEnvelope(float stage_time) {

        // End the envelope after it's target duration
        if (stage_time > envelope_duration) {
            return 1f;
        }

        // Degree 2 polynomial envelope
        return (env_a * stage_time * stage_time) + (env_b * stage_time) + env_c;
    }
}

// STOPPED HERE
// - add collisions + scoring/ui + sound?
// - then play with actual game sequence a bit more
// - THEN DONE!

public struct StartEndFloat {
    public float start;
    public float delta;
    public float end;

    public StartEndFloat(float start, float end) {
        this.start = start;
        this.end = end;
        this.delta = end - start;
    }

    public void StepUpdate(float new_delta) {
        this.start = this.end;
        this.end = this.start + new_delta;
        this.delta = new_delta;
    }
}