using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PickAndDropObjects : MonoBehaviour
{

    public Camera playerCamera;
    public float detectionRange = 15f;
    public LayerMask interactable;
    public Transform itemHolderRight;

    private QuickOutline lastOutlinedObject = null;
    private Transform holdingItem = null;
    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * detectionRange, Color.red);  // This will visualize the ray

        int layerMask = interactable.value;

        layerMask = layerMask & ~LayerMask.GetMask("Player");

        if (Physics.Raycast(ray, out hit, detectionRange, layerMask))
        {
            Debug.Log($"Raycast hits: {hit.collider.gameObject.name}");

            QuickOutline outline = hit.collider.gameObject.GetComponent<QuickOutline>();

            if (outline != null)
            {

                //Outlining
                if (lastOutlinedObject != outline)
                {
                    ClearLastOutline();
                    lastOutlinedObject = outline;
                    lastOutlinedObject.enabled = true;
                }

                //Picking Up
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PickUpObject(hit.collider.gameObject);
                }

                return;
            }
        }
        else
        {
            ClearLastOutline();
        }


        if (Input.GetKeyDown(KeyCode.F))
        {
        DropItem();
        }
    }

    private void ClearLastOutline()
    {
        if(lastOutlinedObject != null)
        {

            lastOutlinedObject.enabled = false;
            lastOutlinedObject = null;
        }
    }

    void PickUpObject(GameObject pickedObject)
{

    if(pickedObject == null) return;

    holdingItem = pickedObject.transform;
    // Set the object's parent to the player's hand (or a specific parent like ItemHolderRight)
    holdingItem.SetParent(itemHolderRight);  
    holdingItem.localPosition = Vector3.zero;  // Reset position relative to the parent
    holdingItem.localRotation = Quaternion.identity;  // Reset rotation relative to the parent

    // Ensure that the object's renderer is enabled
    Renderer renderer = holdingItem.GetComponent<Renderer>();
    if (renderer != null)
    {
        renderer.enabled = true;  // Make sure the object is visible
    }

    // Optional: Disable the object's collider if you want it to no longer interact with physics
    Collider collider = holdingItem.GetComponent<Collider>();
    if (collider != null)
    {
        collider.enabled = false;
    }

    // Optional: You can disable gravity or use kinematic Rigidbody if the object has one
    Rigidbody rb = holdingItem.GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.isKinematic = true;  // Prevent physics interactions while held
        rb.useGravity = false;  // Disable gravity while held
    }

    // Optional: Play sound or animation for picking up the object
    Debug.Log("Picked up: " + holdingItem.name);
}

    private void DropItem()
    {
        if(holdingItem != null)
        {
            Rigidbody rb = holdingItem.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;

                rb.constraints = RigidbodyConstraints.None;

                rb.AddForce(playerCamera.transform.forward * 10f, ForceMode.Impulse);
            }

            Collider collider = holdingItem.GetComponent<Collider>();
            if(collider != null)
            {
                collider.enabled = true;
            }

            holdingItem.SetParent(null);
            holdingItem = null;
        }
    }
}
