using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class GameInfo : UIShowableHidable
    {
        [SerializeField] private Button back;

        private void OnEnable()
        {
            back.onClick.AddListener(() => { HideUI(); });
        }

        private void OnDisable()
        {
            back.onClick.RemoveAllListeners();
        }
    }
}

