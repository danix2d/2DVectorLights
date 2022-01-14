using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowObject : MonoBehaviour
{
    private GameObject shadowParent;
    public Mesh mesh;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    void Awake()
    {
        // gameObject.hideFlags = HideFlags.HideInHierarchy;
        shadowParent = GameObject.Find("Shadows");
        gameObject.transform.parent = shadowParent.transform;

        mesh = new Mesh();

        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>();

        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
}
