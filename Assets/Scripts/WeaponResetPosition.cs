using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponResetPosition : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        transform.localPosition = Vector3.zero;  // Reset position
        transform.localRotation = Quaternion.identity;  // Reset rotation
    }

}
