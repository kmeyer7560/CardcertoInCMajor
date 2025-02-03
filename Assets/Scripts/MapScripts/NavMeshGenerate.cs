using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavMeshPlus.Components;

public class NavMeshGenerate : MonoBehaviour
{
    NavMeshSurface navMeshSurface;

    void Start()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
    }
    public void GenerateNavmesh()
    {
        navMeshSurface.BuildNavMesh();
    }
}
