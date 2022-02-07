using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _Spawner : MonoBehaviour {

    protected float spawner_init_time;
    protected Vector3 spawner_init_origin;
    protected float max_spawn_x;
    protected Vector3 player_speed;

    public void StartSpawner(float spawner_init_time, Vector3 spawner_init_origin, float max_spawn_x, Vector3 player_speed) {
        this.spawner_init_time = spawner_init_time;
        this.spawner_init_origin = spawner_init_origin;
        this.max_spawn_x = max_spawn_x;
        this.player_speed = player_speed;

        Init();
    }
    public float TimeSinceStart(float current_time) {
        return current_time - this.spawner_init_time;
    }

    public Vector3 TravelInStage(Vector3 spawn_origin) {
        return spawn_origin - this.spawner_init_origin;
    }

    protected float RandomAngle() {
        return Random.Range(-180f, 180f);
    }

    protected float RandomHue() {
        return Random.Range(0f, 1f);
    }

    public abstract SpawnParams[] Spawn(float current_time, Vector3 spawn_origin);

    protected virtual void Init() { }
}