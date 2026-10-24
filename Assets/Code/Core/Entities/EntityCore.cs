using UnityEngine;

namespace ProjectProvines.Core.Entities
{
    /// <summary>
    /// Helper class for entities.
    /// </summary>
    public class EntityCore
    {
        public int EntityState { get; private set; }

        /// <summary>
        /// Changes the current state of the entity.
        /// </summary>
        /// <param name="stateHash">The new state of the entity (hash).</param>
        public void ChangeEntityState(int stateHash) => EntityState = stateHash;

        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        public float CalculateDistance(Vector3 startPoint, Vector3 endPoint) => Vector3.Distance(startPoint, endPoint);
    }
}