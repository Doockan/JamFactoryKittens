using Core_Logic;
using UnityEngine;

namespace LevelMenu
{
    public class MintView : MonoBehaviour
    {
        private MonoBehaviourSingleton mainSingleton;

        public float Time;
    
        private void Start()
        {
            mainSingleton = GameObject.Find("MainSingleton").GetComponent<MonoBehaviourSingleton>();
        }
        private void OnMouseDown()
        {
            mainSingleton.RemoveMint(this);
        }
    }
}
