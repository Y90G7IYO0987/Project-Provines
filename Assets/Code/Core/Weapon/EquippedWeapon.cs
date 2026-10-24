using ProjectProvines.Unity.Scriptables;

namespace ProjectProvines.Core.Weapon
{
    /// <summary>
    /// Stores and records information about the weapon being worn.
    /// </summary>
    public class EquippedWeapon
    {
        public WeaponCell CurrentWeapon { get; private set; }

        /// <summary>
        /// Updates the weapon.
        /// </summary>
        public void SetNewWeapon(WeaponCell weapon) => CurrentWeapon = weapon;

        /// <summary>
        /// Initializes the basic weapon.
        /// </summary>
        public EquippedWeapon(WeaponCell defaultWeapon)
        {
            CurrentWeapon = defaultWeapon;
        }
    }
}