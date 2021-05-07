using UnityEngine;


public class UIController : MonoBehaviour
{

    [SerializeField] Canvas settingCanvas;
    
    public void PlayAgain()
    {
        SceneController.Instance.RestartScene();
    }

    public void OpenSetting()
    {
        settingCanvas.gameObject.SetActive(true);
        GameplayController.Instance.PauseGame();
    }

    public void CloseSetting()
    {
        settingCanvas.gameObject.SetActive(false);
        GameplayController.Instance.ResumeGame();

    }

    public void OpenShopping()
    {
        /*TODO:
         * ADD SHOPPING CANVAS!
         */
    }

    public void CloseShopping()
    {
        /*TODO:
         * ADD SHOPPING CANVAS IN GAME.
         */
    }
}
