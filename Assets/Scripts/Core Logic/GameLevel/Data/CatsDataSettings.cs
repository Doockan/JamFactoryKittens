using UnityEngine;

namespace Core_Logic.Data
{
    [CreateAssetMenu(menuName = "Settings/CatsData", fileName = "CatsData")]
    public class CatsDataSettings : ScriptableObject
    {
        [SerializeField] private int[] catsPool;

        public int[] CatsPool => catsPool;
    }
}
