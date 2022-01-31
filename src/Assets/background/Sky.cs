using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{
    [SerializeField] private GameSequence game_seq;
    [SerializeField] private Camera camera_ref;

    // Update is called once per frame
    void Update()
    {
        camera_ref.backgroundColor = game_seq.curr_params.sky_color;
    }
}
