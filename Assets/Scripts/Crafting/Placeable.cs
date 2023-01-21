using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    public bool isPlaced;
    public LayerMask hitMask;

    public CraftingRecipeObject craftObj;

    private void Awake()
    {
        isPlaced = false;
        hitMask = 1 << LayerMask.NameToLayer("Ground");
    }

    private void Start()
    {
        // set colliders to triggers while not placed


        // set crafting state
        CrosshairInteraction.Instance.craftingState.SetCraftObjectOnMouse(transform.gameObject);
    }

    void Update()
    {
        if (!isPlaced)
        {
            PlacingRoutine();
            HandleRotation();
        }
    }

    public void PlacingRoutine()
    {
        Vector3 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = Vector3.forward;
        float maxDist = Mathf.Infinity;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDist, hitMask))
        {
            if (hit.collider != null)
            {
                //Debug.DrawRay(origin, direction * hit.distance, Color.red);
                //Debug.Log(hit.collider.name);
                transform.position = hit.point;
            }
            else
            {
                //Debug.DrawRay(origin, direction * 1000, Color.red);
                //Debug.Log("Hit nothing");
            }
        }
    }

    private void HandleRotation()
    {
        if (Input.mouseScrollDelta != Vector2.zero)
            transform.rotation = transform.rotation * Quaternion.Euler(0, 45, 0);
    }
}
