using UnityEngine;

namespace ProjectProvines.Unity.Scriptables
{
    public abstract class CellData : ScriptableObject
    {
        public string Name;
        public string Description;
        public Sprite Icon;
        public GameObject Prefab;
        public CellType CellType;
    }
}