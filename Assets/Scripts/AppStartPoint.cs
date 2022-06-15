using MainMenu;
using UnityEngine;


public class AppStartPoint : MonoBehaviour
{
    public static MenuManager menuManager;
    
    [SerializeField] private StartMenuView startMenu;
    [SerializeField] private RoomMenuView roomMenu;
    [SerializeField] private LevelMenuView levelMenu;
    [SerializeField] private WinMenuView winMenu;
    
    private void Awake()
    {
        menuManager = new MenuManager(
            new StartMenuState(startMenu),
            new RoomMenuState(roomMenu),
            new LevelMenuState(levelMenu),
            new WinState(winMenu));
    }

    private void OnEnable()
    {
        menuManager.GoToScreenOfType<StartMenuState>();
    }
}
