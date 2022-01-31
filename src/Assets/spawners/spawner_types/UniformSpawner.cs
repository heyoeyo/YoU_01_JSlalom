using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniformSpawner : _Spawner {

    public override SpawnParams[] Spawn(float current_time, Vector3 spawn_origin) {

        // Generate spawn parameters as uniform random variables
        float spawn_x = spawn_origin.x + Random.Range(-1f, 1f) * max_spawn_x;
        float spawn_angle = RandomAngle();
        float spawn_hue = RandomHue();

        // Generate a single obstacle per spawn event
        SpawnParams spawn_1 = new SpawnParams(spawn_x, spawn_angle, spawn_hue);
        SpawnParams[] new_spawns = { spawn_1 };

        return new_spawns;
    }

}

