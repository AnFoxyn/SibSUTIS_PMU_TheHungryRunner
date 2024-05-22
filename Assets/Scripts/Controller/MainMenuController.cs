using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(Application.isMobilePlatform);
    }

    public void OnPlayClick()
    {
        SceneManager.LoadScene("Scenes/Levels/Level_01");
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
