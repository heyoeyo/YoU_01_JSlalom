using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterSpawner : _Spawner {


    [SerializeField, Range(0, 10)] private int min_cluster_count = 1;
    [SerializeField, Range(0, 10)] private int max_cluster_count = 3;
    [SerializeField] private float cluster_spread = 1f;


    public override SpawnParams[] Spawn(float current_time, Vector3 spawn_origin) {

        int num_spawns = Random.Range(min_cluster_count, max_cluster_count);
        float center_x = spawn_origin.x + Random.Range(-1f, 1f) * max_spawn_x;

        float spawn_hue = RandomHue();
        float spawn_angle = RandomAngle();
        SpawnParams[] new_spawns = new SpawnParams[num_spawns];
        for (int i = 0; i < num_spawns; i++) {
            float spawn_x = center_x + Random.Range(-1f, 1f) * cluster_spread;
            spawn_hue = (spawn_hue + (Random.Range(-1f, 1f)) * 0.1f) % 1f;
            spawn_angle += Random.Range(-1f, 1f) * 10;
            new_spawns[i] = new SpawnParams(spawn_x, spawn_angle, spawn_hue);

        }

        return new_spawns;
    }

}

