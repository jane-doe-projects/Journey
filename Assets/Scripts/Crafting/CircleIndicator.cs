using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleIndicator : MonoBehaviour
{
    public float radius = 3f;
    public GameObject centerPoint;
    public GameObject ogCube;
    public List<GameObject> cubes;
    public bool outOfRange;
    public Material indicatorMaterial;
    public Color defaultColor;
    public Color oorColor;

    // Start is called before the first frame update
    void Awake()
    {
        cubes.Clear();
        outOfRange = false;
        InstantiateCubes();
        AlignCubes();
    }

    public void InstantiateCubes()
    {
        int amount = ((int)radius) * 6;
        for (int i = 0; i < amount ; i++)
        {
            cubes.Add(Instantiate(ogCube, transform));
        }
        ogCube.SetActive(false);
    }

    public void AlignCubes()
    {
        float angle = 360 / cubes.Count;
        float currentAngle = 0;

        foreach (GameObject cube in cubes)
        {
            cube.transform.position = centerPoint.transform.position + new Vector3(0, 0, radius);
            cube.transform.RotateAround(centerPoint.transform.position, Vector3.up, currentAngle);
            currentAngle += angle;
        }
    }

    public void SetOutOfRange(bool isOOR)
    {
        outOfRange = isOOR;
        if (outOfRange)
            indicatorMaterial.color = oorColor;
        else
            indicatorMaterial.color = defaultColor;
    }
}
