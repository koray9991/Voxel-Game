using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public CubeParent cubeParent;
    public float downCubePosY;
    public Transform smallCubes;
    public ParticleSystem explosionParticle;
    public Material mat;
    public Transform coinCubes;
    public void Start()
    {
        cubeParent = transform.parent.GetComponent<CubeParent>();
        mat = GetComponent<MeshRenderer>().material;
        for (int i = 0; i < smallCubes.childCount; i++)
        {
            smallCubes.GetChild(i).GetComponent<MeshRenderer>().material = mat;
        }
        for (int i = 0; i < coinCubes.childCount; i++)
        {
            coinCubes.GetChild(i).GetComponent<MeshRenderer>().material = mat;
        }
    }


  
}
