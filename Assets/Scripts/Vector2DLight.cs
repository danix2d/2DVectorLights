using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector2DLight : MonoBehaviour
{
    public float lightRadius = 10;
    public Color color;
    [Range(1, 255)]
    public int lightIndex = 1;

    public float center = 1.55f;
    public float fallOff = 4f;

    //SHADOWS
    [HideInInspector]
    private bool allShadows = false;
    public Material shadowMat;
    private List<Vector2> worldPoints = new List<Vector2>();
    public Dictionary<ShadowCaster, ShadowObject> castersAndShadows = new Dictionary<ShadowCaster, ShadowObject>();
    private Vector2 dir;
    private float maxLenght;
    private float distance;

    //LIGHT
    private bool updateShadows;
    private Transform trans;
    private Vector3 lightPosition;
    private MeshRenderer lightRenderer;
    private Material lightMat;

    //MESH
    private Mesh mesh;
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private int idx = 2, idx2 = 1, idx3 = 0, idx4 = 3;



    void Awake()
    {
        trans = GetComponent<Transform>();
        lightPosition = trans.position;

        shadowMat = new Material(Shader.Find("Custom/Light2D/StencilMask"));
        shadowMat.SetInt("_RefValue", lightIndex);
        shadowMat.SetInt("_StencilMask", lightIndex);
    }

    void Start()
    {
        lightRenderer = GetComponent<MeshRenderer>();
        lightMat = new Material(Shader.Find("Custom/Light2D/Light"));
        lightMat.SetInt("_RefValue", lightIndex);
        lightMat.SetInt("_StencilMask", lightIndex);
        lightMat.SetColor("_Color", color);
        lightMat.SetFloat("_cenFalloff", center);
        lightMat.SetFloat("_Falloff", fallOff);
        lightRenderer.material = lightMat;
    }

    void LateUpdate()
    {
        if (lightPosition != trans.position)
        {
            updateShadows = true;
            lightPosition = trans.position;
        }
        else
        {
            updateShadows = false;
        }

        ObjectsInRange();
    }

    void ObjectsInRange()
    {

        foreach (var item in castersAndShadows)
        {

            if (item.Value.gameObject == null) continue;

            if (allShadows && item.Key.isStatic)
            {
                if (!updateShadows)
                    continue;
            }

            if (item.Key.gameObject.activeInHierarchy)
            {
                item.Value.gameObject.SetActive(true);
                item.Value.meshRenderer.enabled = true;

                if ((item.Key.trans.position - trans.position).sqrMagnitude < lightRadius * lightRadius)
                {
                    item.Value.meshRenderer.enabled = true;
                    CheckWorldPoints(item.Key, item.Value.mesh);
                }
                else
                {
                    item.Value.meshRenderer.enabled = false;
                }
            }
            else
            {
                item.Value.meshRenderer.enabled = false;
                item.Value.gameObject.SetActive(false);
            }
        }

        allShadows = true;
    }


    void CheckWorldPoints(ShadowCaster caster, Mesh shadowMesh)
    {
        mesh = shadowMesh;

        worldPoints.Clear();

        for (int i = 0; i < caster.worldPoints.Count; i++)
        {
            worldPoints.Add(caster.worldPoints[i]);
        }

        vertices.Clear();
        triangles.Clear();

        for (int i = 0; i < worldPoints.Count; i++)
        {
            dir = (Vector2)trans.position - worldPoints[i];
            distance = dir.sqrMagnitude;
            maxLenght = distance / lightRadius;

            vertices.Add(worldPoints[i]);
            vertices.Add(worldPoints[i] - ClampM(NormalizePrecise(dir) * 2000, maxLenght, lightRadius));

             Debug.DrawLine(worldPoints[i], trans.position,Color.blue);
             Debug.DrawLine(worldPoints[i], worldPoints[i] - ClampM(NormalizePrecise(dir) * 2000, maxLenght, lightRadius),Color.green);
        }

        SetTriangles();
        CreateMesh();

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, lightRadius);
    }

    void SetTriangles()
    {

        for (int e = 0; e < vertices.Count * 3; e++)
        {
            triangles.Add(e);
        }

        idx = 2; idx2 = 1; idx3 = 0; idx4 = 3;

        for (int j = 0; j < vertices.Count * 3; j += 6)
        {
            if (j != (vertices.Count * 3) - 6)
            {
                triangles[j] = idx3 * 2;
                triangles[j + 1] = idx2;
                triangles[j + 2] = idx;
                triangles[j + 3] = idx2;
                triangles[j + 4] = idx4;
                triangles[j + 5] = idx;

                idx += 2; idx2 += 2; idx3++; idx4 += 2;

            }
            else
            {
                triangles[j] = idx3 * 2;
                triangles[j + 1] = idx2;
                triangles[j + 2] = 0;
                triangles[j + 3] = idx2;
                triangles[j + 4] = 1;
                triangles[j + 5] = 0;
            }
        }
    }

    void CreateMesh()
    {
        mesh.Clear();
        mesh.MarkDynamic();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
    }

    public Vector2 ClampM(Vector2 v, float max, float radius)
    {
        if (v.sqrMagnitude > lightRadius * lightRadius)
        {
            v = v.normalized * Mathf.Abs(max - radius);
        }
        return v;
    }

    public static Vector2 NormalizePrecise(Vector2 v)
    {
        float mag = v.sqrMagnitude;
        if (mag == 0)
        {
            return Vector3.zero;
        }
        return (v / mag);
    }
}


