using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    public static GamePauseUI Instance { get; private set; }

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        Instance = this;

        resumeButton.onClick.AddListener(() => {
            GameManager.Instance.PauseGame();
        });
        optionsButton.onClick.AddListener(() => {
            OptionsUI.Instance.Show();
            Hide();
        });
        mainMenuButton.onClick.AddListener(() => {
            GameManager.Instance.PauseGame();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }

    private void Start() {
      
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
        Hide();
    }

    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }

    public void Show() {
        gameObject.SetActive(true);

        resumeButton.Select();
    }

    private void Hide() { gameObject.SetActive(false); }
}
