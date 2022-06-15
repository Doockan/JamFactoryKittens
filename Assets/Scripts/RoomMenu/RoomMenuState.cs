using System;

namespace MainMenu
{
    public class RoomMenuState : UIState
    {
        protected override IUIShowableHidable ShowableHidable { get; set; }
        private RoomMenuView _view;

        private Action _goToStartMenu;
        private Action _goToLevelMenu;

        public RoomMenuState(RoomMenuView view)
        {
            _view = view;
            ShowableHidable = _view;
            InitActions();
        }

        private void InitActions()
        {
            _goToStartMenu = () => 
            {
                AppStartPoint.menuManager.GoToScreenOfType<StartMenuState>();
            };
            _goToLevelMenu = () => 
            {
                AppStartPoint.menuManager.GoToScreenOfType<LevelMenuState>();
            };
        }

        protected override void Enter(params object[] parameters)
        {
            _view.GoToStartMenu += _goToStartMenu;
            _view.GoToLevel += _goToLevelMenu;
        
        }

        protected override void Exit()
        {
            _view.GoToStartMenu -= _goToStartMenu;
            _view.GoToLevel -= _goToLevelMenu;
        }

    }
}
