using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameState
{
    GamePlay,
    Transition,
    Pause
}

public class GameplayController : Singleton<GameplayController>
{

    public GameState gamestate;
    [SerializeField] private AreaController areaController;
    [SerializeField] private PlayerController playerController;
    [SerializeField] GameObject obstacles;
    [SerializeField] Camera mainCamera;
    [SerializeField] Canvas gameOverCanvas;
    [SerializeField] TMP_Text currentLevelText;
    [SerializeField] TMP_Text nextLevelText;



    protected override void Awake()
    {
        base.Awake();
        ResumeGame();
        playerController.areaController = areaController;
    }

    private void Start()
    {
        currentLevelText.text = SceneController.Instance.GetCurrentLevel().ToString();
        nextLevelText.text = (SceneController.Instance.GetCurrentLevel() + 1).ToString();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        gamestate = GameState.Pause;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        gamestate = GameState.GamePlay;
    }

    public void WonLevel()
    {
        gamestate = GameState.Transition;
        obstacles.SetActive(false);
        mainCamera.GetComponent<Animator>().enabled = true;
    }

    public void LoseLevel()
    {
        PauseGame();
        gameOverCanvas.gameObject.SetActive(true);
    }

}
