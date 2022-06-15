using System;
using System.Collections;
using System.Collections.Generic;
using MainMenu;
using UnityEngine;

public class WinState : UIState
{
    protected override IUIShowableHidable ShowableHidable { get; set; }
    
    private WinMenuView _view;
    private Action GoToRoom;

    public WinState(WinMenuView view)
    {
        _view = view;
        ShowableHidable = _view;
        InitActions();
    }

    private void InitActions()
    {
        GoToRoom = () =>
        {
            AppStartPoint.menuManager.GoToScreenOfType<RoomMenuState>();
        };
    }

    protected override void Enter(params object[] parameters)
    {
        _view.GoToRoomMenu += GoToRoom;
    }

    protected override void Exit()
    {
        _view.GoToRoomMenu -= GoToRoom;
    }
}
