using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    StartScreen,
    Playing,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public GameState gameState;

    public string levelToLoad;

    public GameObject mainCanvas;
    public GameObject gameOverCanvas;
    public Player player;

    private void Start()
    {
        if (!gm)
        {
            gm = gameObject.GetComponent<GameManager>();
        }

        if (!player)
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }

        mainCanvas.SetActive(true);
        gameOverCanvas.SetActive(false);
    }

    private void Update()
    {
        if (player.isDead)
        {
            gameState = GameState.GameOver;
            mainCanvas.SetActive(false);
            gameOverCanvas.SetActive(true);
        }
        else
        {
            gameState = GameState.Playing;
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
