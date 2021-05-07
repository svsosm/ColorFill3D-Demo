using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : Singleton<SceneController>
{
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //get current level for leveltext.
    public int GetCurrentLevel()
    {
        return SceneManager.GetActiveScene().buildIndex + 1;
    }
}
