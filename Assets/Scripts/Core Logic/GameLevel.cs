using System;
using UnityEngine;

namespace Core_Logic
{
    [Serializable]
    public class GameLevel
    {
        public GameObject LevelObject;
        public SpriteRenderer BackgroundRenderer;
        public Transform CatNode;
        public Transform AllDraggablesParent;
        public Transform MintNode;
    }
}