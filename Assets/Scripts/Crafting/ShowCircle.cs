using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCircle : MonoBehaviour
{
    public Material circleMaterial;
    public Vector3 mousePos;

    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            mousePos = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }
        circleMaterial.SetVector("_Center", new Vector4(mousePos.x, mousePos.y, mousePos.z, 0f));
    }
}
