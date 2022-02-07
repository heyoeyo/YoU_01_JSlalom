using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageParams {

	public _Spawner spawner;
	public Color sky_color;
	public float spawns_per_meter;

	public StageParams(_Spawner spawner, Color sky_color, float spawns_per_meter) {
		this.spawner = spawner;
		this.sky_color = sky_color;
		this.spawns_per_meter = spawns_per_meter;
	}

	public static StageParams Lerp(StageParams s1, StageParams s2, float t) {

		_Spawner lerp_spawner = s1.spawner;
		Color lerp_sky_color = Color.Lerp(s1.sky_color, s2.sky_color, t);
		float lerp_spawn_rate = Mathf.Lerp(s1.spawns_per_meter, s2.spawns_per_meter, t);

		return new StageParams(lerp_spawner, lerp_sky_color, lerp_spawn_rate);
	}
}
