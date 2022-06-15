using Core_Logic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : UIShowableHidable
{
    [SerializeField] private Button back;

    private MonoBehaviourSingleton mainSingleton;
    
    protected override void Awake()
    {
        base.Awake();
        mainSingleton = GameObject.Find("MainSingleton").GetComponent<MonoBehaviourSingleton>();
    }

    private void OnEnable()
    {
        back.onClick.AddListener(() =>
        {
            HideUI();
            mainSingleton.PlayMenuClick();
        });
    }

    private void OnDisable()
    {
        back.onClick.RemoveAllListeners();
    }
}
