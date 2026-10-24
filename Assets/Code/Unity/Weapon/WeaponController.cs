using ProjectProvines.Core.Weapon;
using ProjectProvines.Unity.Animations;
using ProjectProvines.Unity.Input;
using ProjectProvines.Unity.Player;
using ProjectProvines.Unity.Scriptables;
using System;
using System.Collections;
using UnityEngine;

namespace ProjectProvines.Unity.Weapon
{
    /// <summary>
    /// Performs the main interaction with the weapon’s components.
    /// It ties everything together in the Unity functions.
    /// </summary>
    [RequireComponent(typeof(WeaponCell))]
    [RequireComponent(typeof(PlayerData))]
    [RequireComponent(typeof(PlayerInputController))]
    public class WeaponController : MonoBehaviour
    {
        public event Action OnPlayerAttacking;

        public static WeaponController Instance { get; private set; }

        public WeaponCell EquippedWeapon => _weaponData.CurrentWeapon;

        [SerializeField] private WeaponCell defaultWeapon;
        [SerializeField] private PlayerData playerData;

        [SerializeField] private PlayerInputController inputManager;

        [SerializeField] private float colliderDelayTime = 0.25f;

        // Collider for attack processing.
        private BoxCollider _staticCollider;

        private EquippedWeapon _weaponData;
        private PlayerAnimations _playerAnimations;

        private bool _isAttacking => _playerAnimations.IsPlayerAttacking;

        private void Awake()
        {
            Instance = this;
            _weaponData = new EquippedWeapon(defaultWeapon);
        }

        /// <summary>
        /// Initializes the main components and subscribes to events.
        /// </summary>
        private void Start()
        {
            var physics = playerData.Prefab.GetComponent<PlayerPhysics>();

            _playerAnimations = playerData.Prefab.GetComponent<PlayerAnimations>();
            _staticCollider = physics.WeaponStaticBox.GetBoxCollider();

            inputManager.OnAttackInput += OnPlayerAttack;
        }

        /// <summary>
        /// Processes the attack by the player.
        /// </summary>
        private void OnPlayerAttack()
        {
            OnPlayerAttacking?.Invoke();

            if (EquippedWeapon.WeaponType == WeaponCellType.Static)
                if (!_isAttacking)
                    StartCoroutine(SwitchColliderStatus());
        }

        /// <summary>
        /// Adds an additional delay to the collider.
        /// </summary>
        private IEnumerator SwitchColliderStatus()
        {
            _staticCollider.enabled = true;

            yield return new WaitForSeconds(colliderDelayTime);

            _staticCollider.enabled = false;
        }

        private void OnDestroy()
        {
            inputManager.OnAttackInput -= OnPlayerAttack;
        }
    }

}