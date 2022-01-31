using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInUI : MonoBehaviour
{

    [SerializeField] private CanvasGroup ui_group;
    [SerializeField] private float duration;

    private bool fade_enable;
    private float fade_end_time;

    void Awake()
    {
        fade_enable = false;
        fade_end_time = 0;
    }

    void Update()
    {
        if (fade_enable) {
            float curr_time = Time.timeSinceLevelLoad;
            float fade_amount = 1 - ((fade_end_time - curr_time) / duration);
            ui_group.alpha = Mathf.Clamp01(fade_amount * fade_amount * fade_amount);
            fade_enable = (curr_time < fade_end_time);
        }
    }

    private void OnEnable() {
        fade_enable = true;
        fade_end_time = Time.timeSinceLevelLoad + duration;
    }
}
