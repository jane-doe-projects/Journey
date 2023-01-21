using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    Vector3 forceDirection;

    // Start is called before the first frame update
    void Start()
    {
        // add rigid body
        // start routine to add force and destroy component on hitting the ground after  few seconds
        //Debug.Log("Adding drop item functionality.");
        StartCoroutine("DropItemRoutine");
    }

    IEnumerator DropItemRoutine()
    {
        Vector3 randomOffset = RandomVector();
        forceDirection = Vector3.up + PlayerController.Instance.transform.forward + randomOffset;
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        Collider col = gameObject.GetComponent<Collider>();
        bool colOption = col.isTrigger;
        col.isTrigger = false;
        rb.AddForce(forceDirection, ForceMode.Impulse);

        yield return new WaitForSeconds(2);

        col.isTrigger = colOption;
        Destroy(rb);
        Destroy(this);
    }

    Vector3 RandomVector()
    {
        return new Vector3(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10))/10;
    }

    public static void DropIntoWorld(ItemObject item, int amount)
    {
        GameObject spawnPrefab = item.prefab;
        if (spawnPrefab == null)
            spawnPrefab = GameManager.Instance.globalIcons.MissingPrefabModel;
        GameObject droppedItem = Instantiate(spawnPrefab, PlayerController.Instance.itemSpawn.position, Quaternion.identity, GameManager.Instance.gameState.dropParent);

        // set new item infos for loot
        Lootable lootInfo = droppedItem.GetComponent<Lootable>();
        if (lootInfo)
        {
            lootInfo.quantity = amount;
            // start drop routine
            droppedItem.AddComponent<DropItem>();
        }
        else
        {
            Debug.Log("No lootable component on gameobject");
            Destroy(droppedItem);
        }
    }
}
