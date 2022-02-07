using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SpawnParams {
    public float x;
    public float angle;
    public float hue;

    public SpawnParams(float spawn_x, float spawn_angle, float spawn_hue) {
        this.x = spawn_x;
        this.angle = spawn_angle;
        this.hue = spawn_hue;
    }
}
