using UnityEngine;

namespace ProjectProvines.Core.Entities
{
    public class EntityCore
    {
        public int EntityState { get; private set; }

        public void ChangeEntityState(int stateHash) => EntityState = stateHash;

        public float CalculateDistance(Vector3 startPoint, Vector3 endPoint) => Vector3.Distance(startPoint, endPoint);
    }
}