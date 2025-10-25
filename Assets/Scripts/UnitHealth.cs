using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    public float unitHealth = 100;
    public float unitArmor;



    public void takeDamage(float dmg)
    {
        unitHealth -= dmg;
        if(unitHealth <= 0)
        {
            Destroy(gameObject);
        }
    }


}
