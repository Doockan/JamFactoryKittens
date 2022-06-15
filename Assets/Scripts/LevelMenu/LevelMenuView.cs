using System;
using Core_Logic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenuView : UIShowableHidable
{
    public event Action GoToRoomMenu;

    [SerializeField] private Button _back;
    [SerializeField] private Button _gameInfo;
    [SerializeField] private Button _howToFind;
    [SerializeField] private Button _panel;
    [SerializeField] private Button _rating;
    [SerializeField] private Button _mite;
    [SerializeField] private Button _cutMite;

    [SerializeField] private lvlGameInfo _lvlGameInfoPopUp;
    [SerializeField] private lvlHowToFindCat _lvlHowToFindPopUp;
    [SerializeField] private lvlPanel _lvlPanelPopUp;
    [SerializeField] private lvlRating _lvlRatingPopUp;
    [SerializeField] private lvlMite _lvlMitePopUp;
    [SerializeField] private lvlCutMite _lvlcutMitePopUp;

    private MonoBehaviourSingleton mainSingleton;

    protected override void Awake()
    {
        base.Awake();
        mainSingleton = GameObject.Find("MainSingleton").GetComponent<MonoBehaviourSingleton>();
    }

    private void OnEnable()
    {
        _back.onClick.AddListener(() =>
        {
            GoToRoomMenu();
            mainSingleton.GoHome();
            mainSingleton.PlayToRoom();
        });

        _gameInfo.onClick.AddListener(OpenInfo);
        _howToFind.onClick.AddListener(OpenHowToFind);
        _panel.onClick.AddListener(OpenPanel);
        _rating.onClick.AddListener(OpenRating);
        _mite.onClick.AddListener(OpenMite);
        _cutMite.onClick.AddListener(OpenCutMite);
    }

    private void OnDisable()
    {
        _back.onClick.RemoveAllListeners();

        _gameInfo.onClick.RemoveAllListeners();
        _howToFind.onClick.RemoveAllListeners();
        _panel.onClick.RemoveAllListeners();
        _rating.onClick.RemoveAllListeners();
        _mite.onClick.RemoveAllListeners();
        _cutMite.onClick.RemoveAllListeners();
    }

    private void OpenInfo()
    {
        mainSingleton.DefaultState = MonoBehaviourSingleton.PlayerState.Menu;
        mainSingleton.State = MonoBehaviourSingleton.PlayerState.Idling;
        mainSingleton.IsCountingHigh = false;
        _lvlGameInfoPopUp.ShowUI();
    }

    private void OpenHowToFind()
    {
        _lvlHowToFindPopUp.ShowUI();
    }

    private void OpenPanel()
    {
        _lvlPanelPopUp.ShowUI();
    }

    private void OpenRating()
    {
        _lvlRatingPopUp.ShowUI();
    }
    private void OpenMite()
    {
        _lvlMitePopUp.ShowUI();
    }
    private void OpenCutMite()
    {
        _lvlcutMitePopUp.ShowUI();
    }
}
