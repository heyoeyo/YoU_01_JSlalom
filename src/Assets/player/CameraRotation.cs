using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{

    [SerializeField] private UserInput user_input;
    [SerializeField] private float max_rotation_angle_deg = 30;

    void Update() {

        // Rotate camera according to user input
        float strafe_angle = user_input.strafe_momentum * max_rotation_angle_deg;
        transform.rotation = Quaternion.Euler(Vector3.back * strafe_angle);
    }
}
