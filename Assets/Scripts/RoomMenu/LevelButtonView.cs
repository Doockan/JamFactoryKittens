using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class LevelButtonView : MonoBehaviour
    {
        [SerializeField] private int levelId;
        [SerializeField] private Button button;

        public int LevelId => levelId;
        public Button Button => button;
    }
}