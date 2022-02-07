using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptySpawner : _Spawner {
    public override SpawnParams[] Spawn(float current_time, Vector3 spawn_origin) {

        SpawnParams[] new_spawns = { };

        return new_spawns;
    }
}
