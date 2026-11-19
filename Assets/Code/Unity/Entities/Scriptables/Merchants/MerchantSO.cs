using UnityEngine;

namespace ProjectProvines.Unity.Scriptables
{
    [CreateAssetMenu(fileName = "New Merchant", menuName = "Entities/Merchant")]
    public class MerchantData : EntityData
    {
        public int Level;

        public float MinProductsPrice;
        public float MaxProductsPrice;
    }
}