using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCanvas : MonoBehaviour
{
    public static GameCanvas game;
    public GameObject win, fail;
    [SerializeField] private Transform warningTransform;
    [SerializeField] private TextMeshProUGUI tapToPlayText;
    [SerializeField] private float tapToPlayDelay = 0f;

    private void OnEnable()
    {
        GameEvents.TapToPlay += OnTapToPlay;
        GameEvents.GameWin += OnGameWin;
        GameEvents.GameLose += OnGameLose;
    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= OnTapToPlay;
        GameEvents.GameWin -= OnGameWin;
        GameEvents.GameLose -= OnGameLose;
    }

    private void Awake()
    {
        if (!game) game = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // if (GAScript.Instance)
        //     GAScript.Instance.LevelStart(PlayerPrefs.GetInt("Level", 1).ToString());
        // Debug.Log("Level Start" + PlayerPrefs.GetInt("Level", 1));
    }


    private void OnGameEnd()
    {
        DOTween.KillAll();
        DOTween.Kill(warningTransform, true);
    }

    private void OnTapToPlay()
    {
        //HideInstructions();
        tapToPlayText.transform.DOScale(tapToPlayText.transform.localScale * 1.15f, 0.25f).SetLoops(2, LoopType.Yoyo);

        if (tapToPlayDelay > 0f)
            tapToPlayText.DOColor(Color.clear, tapToPlayDelay * 0.6f)
                .SetDelay(tapToPlayDelay * 0.4f)
                .SetEase(Ease.InExpo)
                .OnComplete(() => tapToPlayText.gameObject.SetActive(false));
        else
            tapToPlayText.gameObject.SetActive(false);
    }

    public void restart()
    {
        // if (GAScript.Instance)
        //     GAScript.Instance.LevelFail(PlayerPrefs.GetInt("Level", 1).ToString());
        // if (ISManager.instance)
        //     ISManager.instance.ShowInterstitialAds();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void next()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Before" + PlayerPrefs.GetInt("Level", 1));
        // if (GAScript.Instance)
        //     GAScript.Instance.LevelCompleted(PlayerPrefs.GetInt("Level", 1).ToString());
        // if (ISManager.instance)
        //     ISManager.instance.ShowInterstitialAds();
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 1) + 1);
        Debug.Log("After" + PlayerPrefs.GetInt("Level", 1));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnGameWin()
    {
        win.SetActive(true);
        OnGameEnd();
    }

    private void OnGameLose()
    {
        fail.SetActive(true);
        OnGameEnd();
    }
}