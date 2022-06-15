using System;
using Core_Logic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuView : UIShowableHidable
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _authorButton;
    [SerializeField] private SettingsMenu m_settingsPopUp;
    [SerializeField] private AuthorTable _authorPopUp;

    public event Action StartGame;

    private MonoBehaviourSingleton mainSingleton;

    protected override void Awake()
    {
        base.Awake();
        mainSingleton = GameObject.Find("MainSingleton").GetComponent<MonoBehaviourSingleton>();
    }

    private void OnEnable()
    {
        _startButton.onClick.AddListener(() =>
        {
            StartGame();
            mainSingleton.GoHome();
            mainSingleton.PlayMenuClick();
        });
        
        _settingButton.onClick.AddListener(OpenSettings);
        _authorButton.onClick.AddListener(OpenAuthor);
    }

    private void OpenSettings()
    {
        m_settingsPopUp.ShowUI();
        mainSingleton.PlayMenuClick();
    }

    private void OpenAuthor()
    {
        _authorPopUp.ShowUI();
        mainSingleton.PlayMenuClick();
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveAllListeners();
        _settingButton.onClick.RemoveAllListeners();
        _authorButton.onClick.RemoveAllListeners();
    }


}
