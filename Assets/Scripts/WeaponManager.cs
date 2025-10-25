using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; } // Singleton Instance

    public Transform weaponHolder; // The hand where weapons are attached
    private GameObject currentWeapon;

    void Awake()
    {
        // Ensure only one instance of WeaponManager exists
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void EquipWeapon(GameObject weaponPrefab)
    {
        // ✅ Destroy the previous weapon if it exists
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        // ✅ Spawn and attach the new weapon to the hand
        currentWeapon = Instantiate(weaponPrefab, weaponHolder);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;
    }
}
