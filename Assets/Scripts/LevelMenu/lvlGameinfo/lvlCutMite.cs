using UnityEngine;
using UnityEngine.UI;

public class lvlCutMite : UIShowableHidable
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
