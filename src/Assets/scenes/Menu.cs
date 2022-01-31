using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartGame();

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Debug.Log("QUIT GAME!");
            Application.Quit();
        }
    }

    void StartGame() {
        int next_scene_idx = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(next_scene_idx);
    }
}
