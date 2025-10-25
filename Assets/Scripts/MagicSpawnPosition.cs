using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSpawnPosition : MonoBehaviour
{

    public GameObject magicBall;
    public Transform magicSpawn;
    public Camera playerCamera;
    public float magicshootForce = 10f;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
        FireMagic();
        }
    }

    private void FireMagic()
    {
        GameObject magic = Instantiate(magicBall, magicSpawn.position, Quaternion.identity);

        Vector3 shootDirection = playerCamera.transform.forward;

        MagicAttack magicScript = magic.GetComponent<MagicAttack>();
        if(magicScript != null)
        {
            magicScript.Initialize(shootDirection);
        }
    }
}
