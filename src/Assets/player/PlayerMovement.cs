using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private UserInput user_input;
    [SerializeField] private float forward_speed_per_second = 80f;
    [SerializeField, Range(0f, 1f)] private float strafe_rate = 0.4f;

    // Shared
    [HideInInspector] public Vector3 meters_per_sec = Vector3.zero;

    private void Awake() {
        meters_per_sec.x = forward_speed_per_second * strafe_rate;
        meters_per_sec.y = 0;
        meters_per_sec.z = forward_speed_per_second;
    }

    void FixedUpdate()
    {
        Vector3 forward_move = Vector3.forward * meters_per_sec.z * Time.fixedDeltaTime;
        Vector3 strafe_move = Vector3.right * meters_per_sec.x * user_input.strafe_momentum * Time.fixedDeltaTime;
        transform.Translate(forward_move + strafe_move, Space.World);
    }
}
