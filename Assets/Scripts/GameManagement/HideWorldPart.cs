using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Code based on https://gamedevacademy.org/how-to-handle-huge-worlds-in-unity-part-1-deactivating-distant-regions-to-improve-performance/ */

public class HideWorldPart : MonoBehaviour
{
    GameObject[] seasons;
    public int maxDistance;

    // Start is called before the first frame update
    void Start()
    {
        seasons = GameObject.FindGameObjectsWithTag("Season");
        HideDistantWorldPart();
    }

    // Update is called once per frame
    void Update()
    {
        HideDistantWorldPart();
    }

    void HideDistantWorldPart()
    {
        Vector3 playerPosition = this.gameObject.transform.position;

        foreach (GameObject season in seasons)
        {
            Vector3 sPosition = season.transform.position;

            float xDistance = Mathf.Abs(sPosition.x - playerPosition.x);
            float zDistance = Mathf.Abs(sPosition.z - playerPosition.z);

            if (xDistance + zDistance > maxDistance)
            {
                season.SetActive(false);
            } else
            {
                season.SetActive(true);
            }
        }

    }
}
