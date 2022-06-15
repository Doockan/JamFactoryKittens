using System;
using Core_Logic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class RoomMenuView : UIShowableHidable
    {
        public event Action GoToStartMenu;
        public event Action GoToLevel;

        private MonoBehaviourSingleton mainSingleton;
    
        [SerializeField] private Button back;
        [SerializeField] private Button Scroll;
        [SerializeField] private Button _gameInfo;
        [SerializeField] private Button _howToPlay;
        [SerializeField] private Button _howToFind;
        [SerializeField] private Button _panel;
        [SerializeField] private Button _rating;

        [SerializeField] private GameInfo _gameInfoPopUp;
        [SerializeField] private HowToPlay _howToPlayPopUp;
        [SerializeField] private HowToFind _howToFindPopUp;
        [SerializeField] private Panel _panelPopUp;
        [SerializeField] private Rating _ratingPopUp;

        [SerializeField] private LevelButtonView[] views;
        [SerializeField] private RectTransform ScrollBar;

        [SerializeField] private RectTransform TargetDown;
        [SerializeField] private RectTransform TargetUp;

        

        private bool _scrollBarUp = true;

        protected override void Awake()
        {
            base.Awake();
            mainSingleton = GameObject.Find("MainSingleton").GetComponent<MonoBehaviourSingleton>();
        }

        private void OnEnable()
        {
            back.onClick.AddListener(() =>
            {
                GoToStartMenu();
                mainSingleton.PlayMenuClick();
            });

            _gameInfo.onClick.AddListener(OpenInfo);
            _howToPlay.onClick.AddListener(OpenHowToPlay);
            _howToFind.onClick.AddListener(OpenHowToFind);
            _panel.onClick.AddListener(OpenPanel);
            _rating.onClick.AddListener(OpenRating);

            foreach (LevelButtonView buttonView in views)
            {
                buttonView.Button.onClick.AddListener(() =>
                {
                    GoToLevel?.Invoke();
                    mainSingleton.SetLevel(buttonView.LevelId);
                });
            }
        }

        private void OnDisable()
        {
            back.onClick.RemoveAllListeners();

            _gameInfo.onClick.RemoveAllListeners();
            _howToPlay.onClick.RemoveAllListeners();
            _howToFind.onClick.RemoveAllListeners();
            _panel.onClick.RemoveAllListeners();
            _rating.onClick.RemoveAllListeners();


            foreach (LevelButtonView buttonView in views)
            {
                buttonView.Button.onClick.RemoveAllListeners();
            }
        }

        public void MoveScrollBar()
        {
            if (_scrollBarUp)
            {
                ScrollBar.DOAnchorPos(TargetDown.anchoredPosition, 1f);
                _scrollBarUp = !_scrollBarUp;
                mainSingleton.PlayMenuClick();
            }
            else
            {
                ScrollBar.DOAnchorPos(TargetUp.anchoredPosition, 1f);
                _scrollBarUp = !_scrollBarUp;
                mainSingleton.PlayMenuClick();
            }
        }

        private void OpenInfo()
        {
            _gameInfoPopUp.ShowUI();
        }

        private void OpenHowToPlay()
        {
            _howToPlayPopUp.ShowUI();
        }

        private void OpenHowToFind()
        {
            _howToFindPopUp.ShowUI();
        }

        private void OpenPanel()
        {
            _panelPopUp.ShowUI();
        }

        private void OpenRating()
        {
            _ratingPopUp.ShowUI();
        }
    }
}
