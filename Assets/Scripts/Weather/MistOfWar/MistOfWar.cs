using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistOfWar : MonoBehaviour
{
    public ParticleSystem parSys;
    public float width;
    public float length;

    public ParticleSystem.ShapeModule moduleShape;
    // Start is called before the first frame update
    void Start()
    {
        moduleShape = parSys.shape;
        moduleShape.enabled = true;
        moduleShape.scale = new Vector3(width, length, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    private void OnValidate()
    {
        moduleShape.enabled = true;
        moduleShape.scale = new Vector3(width, length, 1);
    }
    */
}
