using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameoverScreen : MonoBehaviour
{
    [SerializeField] private GameObject gameover_ui;
    [SerializeField] private Text seconds_survived_text;
    [SerializeField] private PlayerCollision player_life;



    private bool player_is_dead;
    private int player_score;

    void Start()
    {
        gameover_ui.SetActive(false);
        player_is_dead = false;

        player_life.OnPlayerDeath += HandlePlayerDeath;
    }

    // Update is called once per frame
    void Update()
    {
        if (player_is_dead) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                int curr_scene_idx = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(curr_scene_idx);
            }
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Debug.Log("QUIT GAME!");
                Application.Quit();
            }
        }
    }

    private void HandlePlayerDeath() {

        player_is_dead = true;
        player_score = Mathf.RoundToInt(Time.timeSinceLevelLoad);

        gameover_ui.SetActive(true);
        seconds_survived_text.text = player_score.ToString();

    }
}
