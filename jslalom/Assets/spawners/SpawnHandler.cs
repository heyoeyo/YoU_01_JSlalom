using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHandler : MonoBehaviour
{

    [SerializeField] private GameSequence game_seq;
    [SerializeField] private Transform horizon_marker;
    [SerializeField] private Camera camera_ref;
    [SerializeField] private Transform player;
    [SerializeField] private PlayerMovement move;
    [SerializeField] private GameObject obstacle_prefab;
    [SerializeField] private int spawn_pool_size = 500;

    // Variables for handling spawn events
    private SpawnPool pool;
    private _Spawner curr_spawner;
    private float next_spawn_time;
    private Vector3 prev_player_position;
    private float prev_update_time;

    // Variables for representing spawn boundaries
    private float max_spawn_x;
    private float spawn_y;
    private float spawn_z;

    private void Awake() {
        pool = new SpawnPool(obstacle_prefab, this.transform, spawn_pool_size);

        // Initialize state variables
        next_spawn_time = 0f;
        prev_player_position = Vector3.zero;
        prev_update_time = -1f;
    }

    void Start()
    {
        // Figure out camera boundaries
        spawn_y = horizon_marker.position.y;
        spawn_z = horizon_marker.position.z;
        max_spawn_x = GetMaxSpawnX(spawn_z);

        // Initialize positioning
        prev_player_position = player.position;

        // Initialize spawner
        UpdateSpawner(Time.timeSinceLevelLoad);
    }

    private void FixedUpdate() {

        float curr_update_time = Time.timeSinceLevelLoad;
        UpdateSpawner(curr_update_time);

        // Handle new spawns
        bool trigger_spawn_event = (curr_update_time > next_spawn_time);
        if (trigger_spawn_event) {

            // In case many spawns should have occurred in the update time, 
            // we need to 'interpolate' spawn timing/positions to make sure
            // all obstacles are spawned, even at very high spawn rates!
            Vector3 player_position_interp;
            float t_pos_lerp;
            float update_time_delta = (curr_update_time - prev_update_time);
            float spawn_delay = 1f / GetSpawnsPerSecond();
            while (curr_update_time > next_spawn_time) {

                // Estimate spawn position
                t_pos_lerp = (next_spawn_time - prev_update_time) / update_time_delta;
                player_position_interp = Vector3.Lerp(prev_player_position, player.position, t_pos_lerp);

                // Spawn & schedule next spawn time
                HandleOneSpawnEvent(next_spawn_time, player_position_interp);
                next_spawn_time += spawn_delay;
            }
        }

        // Store player position for future spawn point interpolation
        prev_player_position = player.position;
        prev_update_time = curr_update_time;

        // Clear obstacles that pass the player
        pool.Remove(player.position.z);
    }

    float GetSpawnsPerSecond() {
        return game_seq.curr_params.spawns_per_meter * move.meters_per_sec.z;
    }

    void HandleOneSpawnEvent(float current_time, Vector3 player_position) {

        Vector3 spawn_origin = new Vector3(player_position.x, spawn_y, player_position.z + spawn_z);
        SpawnParams[] new_spawns = curr_spawner.Spawn(current_time, spawn_origin);

        Transform spawn_parent = this.transform;
        foreach (SpawnParams spawn_args in new_spawns) {
            Vector3 spawn_pos = new Vector3(spawn_args.x, spawn_origin.y, spawn_origin.z);
            Quaternion spawn_ori = Quaternion.Euler(0, spawn_args.angle, 0);
            Color spawn_color = Color.HSVToRGB(spawn_args.hue, 1, 1);
            pool.Create(spawn_pos, spawn_ori, spawn_color);
        }
    }

    void UpdateSpawner(float current_time) {

        curr_spawner = game_seq.curr_params.spawner;
        if (game_seq.stage_changed)
            curr_spawner.StartSpawner(current_time, player.position, max_spawn_x, move.meters_per_sec);
    }

    float GetMaxSpawnX(float boundary_spawn_z) {

        // Function which determines the farthest x spawn point, based on the camera field-of-view
        // According to triangle: tan(fov / 2) = boundary_spawn_x / boundar_spawn_z

        float vertical_fov_deg = camera_ref.fieldOfView;
        float horizontal_fov_deg = vertical_fov_deg * camera_ref.aspect;
        float half_fov_rad = Mathf.Deg2Rad * horizontal_fov_deg / 2f;
        float boundary_spawn_x = boundary_spawn_z * Mathf.Tan(half_fov_rad);
        //Debug.Log("BOUNDARY MAX X: " + boundary_spawn_x);

        return boundary_spawn_x;
    }
}

