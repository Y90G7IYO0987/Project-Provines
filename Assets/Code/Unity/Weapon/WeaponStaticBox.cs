using ProjectProvines.Unity.Entities;
using ProjectProvines.Unity.Player;
using UnityEngine;

namespace ProjectProvines.Unity.Weapon
{
    /// <summary>
    /// Manages interaction with the static collider.
    /// </summary>
    [RequireComponent(typeof(PlayerPhysics))]
    public class WeaponStaticBox : MonoBehaviour
    {
        [SerializeField] PlayerPhysics playerPhysics;

        private BoxCollider _boxCollider;

        public BoxCollider GetBoxCollider() => _boxCollider;


        /// <summary>
        /// Sets the main collider and passes an instance of the class to the player’s physics.
        /// </summary>
        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                var health = collision.GetComponentInParent<EntityHealthManager>();

                if (health == null)
                    return;

                health.TakeDamage(WeaponController.Instance.EquippedWeapon.Damage);
            }
        }
    }
}