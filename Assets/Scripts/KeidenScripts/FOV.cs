using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        float fov = 90f;
        int rayCount = 2;
        float angle = 0f;
        float angleIncrease = fov / rayCount;
        float viewDistance = 50f;

        Vector3[] vertices = new Vector3[];
        Vector2[] uv = new Vector2[];
        int[] triangles = new int[];




        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
