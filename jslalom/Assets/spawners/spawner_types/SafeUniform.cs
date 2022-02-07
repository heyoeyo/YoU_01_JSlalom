using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeUniform : _Spawner

    // Spawns as uniform random distribution, but with
    // an empty corridor straight in front of the player
{
    [SerializeField] private float corridor_separation = 10f;

    private float spawn_width;

    protected override void Init() {
        spawn_width = (max_spawn_x - corridor_separation);
    }

    public override SpawnParams[] Spawn(float current_time, Vector3 spawn_origin) {

        // Generate spawn parameters as uniform random variables
        float spawn_sign = (Random.Range(0, 2) == 1) ? 1f : -1f;
        float spawn_x = spawn_origin.x + (corridor_separation + Random.Range(0f, 1f) * spawn_width) * spawn_sign;

        // Completely random color/angle
        float spawn_angle = RandomAngle();
        float spawn_hue = RandomHue();

        // Generate a single obstacle per spawn event
        SpawnParams spawn_1 = new SpawnParams(spawn_x, spawn_angle, spawn_hue);
        SpawnParams[] new_spawns = { spawn_1 };

        return new_spawns;
    }
}
