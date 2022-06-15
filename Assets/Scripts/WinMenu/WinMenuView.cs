using System;
using Core_Logic;
using UnityEngine;
using UnityEngine.UI;

public class WinMenuView: UIShowableHidable
{
    public event Action GoToRoomMenu;

    [SerializeField] private Button _back;

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
            mainSingleton.PlayMenuClick();
        });
    }

    private void OnDisable()
    {
        _back.onClick.RemoveAllListeners();
    }
}
