using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public bool isTesting;
    public int buildIndex;

    private void Start()
    {
        if (isTesting)
        {
            SceneManager.LoadScene(buildIndex);
        }
        else
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("Level") >= SceneManager.sceneCountInBuildSettings
                ? PlayerPrefs.GetInt("ThisLevel")
                : PlayerPrefs.GetInt("Level", 1));
        }
    }
    
}