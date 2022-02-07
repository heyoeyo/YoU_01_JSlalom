using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramerateControl : MonoBehaviour
{
    [SerializeField] private bool enable_limit = true;
    [SerializeField, Min(30)] private int frames_per_second = 60;

    private void Awake() {
        Application.targetFrameRate = enable_limit ? frames_per_second : -1;
    }
}
