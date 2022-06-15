using UnityEngine;

[System.Serializable]
public class GameLevel
{
    public GameLevelTest[] GameLevelTests;
    public GameObject[] PrefabGame;
    public GameObject WinPanel;
    private int CurrentIndexLevel;
    public void PlayLevel(int indexLevel)
    {
        CurrentIndexLevel = indexLevel -1;
        PrefabGame[CurrentIndexLevel].SetActive(true);
        var TriggerWin = GameObject.FindObjectOfType<TriggerWin>();
        TriggerWin.WunAction += NewTest;
        TriggerWin.WunAction += OnDisable;
    }
    public void OnEnable(int indexLevel)
    {
        PlayLevel(indexLevel);
    }
    public void OnDisable() 
    {
        PrefabGame[CurrentIndexLevel] = GameLevelTests[CurrentIndexLevel].PafabLevel;
        PrefabGame[CurrentIndexLevel].SetActive(false);

        CurrentIndexLevel = 0;
    }
    private void NewTest()
    {
        WinPanel.SetActive(true);
    }
}

