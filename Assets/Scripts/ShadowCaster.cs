using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCaster : MonoBehaviour
{

    public bool allLights; // GET ALL LIGHTS OR USE SPECIFIC LIGHTS
    public bool isStatic = false; // WHEN LIGHTS AND OBJECT IS STATIC

    public List<Vector2DLight> lights = new List<Vector2DLight>(); //USE SPECIFIC LIGHTS

    [HideInInspector]
    public Transform trans;
    [HideInInspector]
    public List<Vector2> worldPoints = new List<Vector2>();
    [HideInInspector]
    public List<ShadowObject> shadowObjects = new List<ShadowObject>();

    private List<Vector2> localPoints = new List<Vector2>();

    private PolygonCollider2D objCollider;
    private ShadowObject shadow;

    void Awake()
    {

        objCollider = GetComponent<PolygonCollider2D>();
        trans = GetComponent<Transform>();

        LocalPoints();
        LocalToWorldPoints();

        if (allLights)
            lights.AddRange(GameObject.FindObjectsOfType<Vector2DLight>());

    }

    void Start()
    {
        CreateShadowsObjects();
    }

    void Update()
    {
        if (!isStatic)
        {
            LocalToWorldPoints();
        }
    }

    void CreateShadowsObjects()
    {
        if (isStatic)
            return;
        foreach (var light in lights)
        {
            GameObject shadowObject = new GameObject();
            shadowObject.AddComponent<ShadowObject>();
            shadow = shadowObject.GetComponent<ShadowObject>();
            shadow.meshRenderer.material = light.shadowMat;
            shadow.name = "Shadow of " + this.gameObject.name;
            shadowObjects.Add(shadow);
            light.castersAndShadows.Add(this, shadow);
            shadowObject.hideFlags = HideFlags.HideInHierarchy;
        }
    }

    public void LocalPoints()
    {
        localPoints.Clear();

        for (int i = 0; i < objCollider.pathCount; i++)
        {
            localPoints.AddRange(objCollider.GetPath(i));
        }

    }

    public void LocalToWorldPoints()
    {

        worldPoints.Clear();
        for (int i = 0; i < localPoints.Count; i++)
        {
            worldPoints.Add(trans.localToWorldMatrix.MultiplyPoint(localPoints[i]));
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < shadowObjects.Count; i++)
        {
            if (shadowObjects[i] != null)
                shadowObjects[i].gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {

        foreach (var light in lights)
        {
            if (light != null)
                light.castersAndShadows.Remove(this);
        }

        for (int i = 0; i < shadowObjects.Count; i++)
        {
            if (shadowObjects[i] != null)
                Destroy(shadowObjects[i]);
        }
    }
}
