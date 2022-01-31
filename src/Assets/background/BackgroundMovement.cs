using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{

    [SerializeField] private Transform origin_reference;

    private Vector3 offset_position;

    private void Awake() {
        offset_position = transform.position - origin_reference.position;
    }

    void Update()
    {
        transform.position = origin_reference.position + offset_position;
    }
}
