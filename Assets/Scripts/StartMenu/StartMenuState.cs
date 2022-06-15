using System;
using MainMenu;

public class StartMenuState : UIState
{
    private StartMenuView _view;

    private Action StartGame;

    protected override IUIShowableHidable ShowableHidable { get; set; }

    public StartMenuState(StartMenuView startMenuView)
    {
        _view = startMenuView;
        ShowableHidable = _view;
        InitActions();
    }

    private void InitActions()
    {
        StartGame = () => 
        {
            AppStartPoint.menuManager.GoToScreenOfType<RoomMenuState>();
        };
    }

    protected override void Enter(params object[] parameters)
    {
        _view.StartGame += StartGame;
    }

    protected override void Exit()
    {
        _view.StartGame -= StartGame;
    }
}
