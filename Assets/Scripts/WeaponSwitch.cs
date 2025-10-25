using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public GameObject sword1Prefab; // Prefab of Sword1
    public GameObject sword2Prefab; // Prefab of Sword2

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            WeaponManager.Instance.EquipWeapon(sword1Prefab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WeaponManager.Instance.EquipWeapon(sword2Prefab);
        }
    }
}
