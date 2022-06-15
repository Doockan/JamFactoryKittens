using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class Rating : UIShowableHidable
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