using System;
using MainMenu;

public class LevelMenuState : UIState
{
    protected override IUIShowableHidable ShowableHidable { get; set; }
    private LevelMenuView _view;

    private Action GoToRoomMenu;

    public LevelMenuState(LevelMenuView view)
    {
        _view = view;
        ShowableHidable = _view;
        InitActions();
    }

    private void InitActions()
    {
        GoToRoomMenu = () =>
        {
            AppStartPoint.menuManager.GoToScreenOfType<RoomMenuState>();
        };
    }

    protected override void Enter(params object[] parameters)
    {
        _view.GoToRoomMenu += GoToRoomMenu;
    }

    protected override void Exit()
    {
        _view.GoToRoomMenu -= GoToRoomMenu;
    }
}
