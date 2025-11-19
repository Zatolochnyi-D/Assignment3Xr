using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using System.Linq;

public class WorldSpawner : MonoBehaviour
{
    public static WorldSpawner Instance { get; private set; }

    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private float spawnRadius = 1f;
    [SerializeField] private GameObject[] grassBushes;
    [SerializeField] private GameObject[] flowers;
    [SerializeField] private float bushChance = 0.7f;
    [SerializeField] private int objectsToSpawn = 20;

    void Awake()
    {
        Instance = this;
    }

    private bool CheckIfWorldPointIsInsidePlane(ARPlane plane, Vector3 worldPosition)
    {
        var local = plane.transform.InverseTransformPoint(worldPosition);

        if (local.x >= -plane.extents.x && local.x <= plane.extents.x && local.z >= -plane.extents.y && local.z <= plane.extents.y)
            return true;
        return false;
    }

    public void SpawnObjects()
    {
        var userPosition = Camera.main.transform.position;
        var planes = new List<ARPlane>();
        foreach (var plane in planeManager.trackables)
            if (plane.alignment == UnityEngine.XR.ARSubsystems.PlaneAlignment.HorizontalUp)
                if (CheckIfWorldPointIsInsidePlane(plane, userPosition))
                    planes.Add(plane);

        for (int i = 0; i < objectsToSpawn; i++)
        {
            var inCylinder = Random.insideUnitCircle * spawnRadius;
            var x = inCylinder.x + userPosition.x;
            var z = inCylinder.y + userPosition.z;
            var appropriatePlanes = planes.Where(plane => CheckIfWorldPointIsInsidePlane(plane, new(x, 0f, z))).ToArray();
            if (appropriatePlanes.Length == 0)
            {
                i--;
                continue;
            }
            var plane = appropriatePlanes[Random.Range(0, appropriatePlanes.Length)].transform;
            var a = plane.up.x;
            var b = plane.up.y;
            var c = plane.up.z;
            var d = -(a * plane.position.x + b * plane.position.y + c * plane.position.z);
            var y = -(a * x + c * z + d) / b;

            GameObject[] objectsToSpawn;
            if (Random.value < bushChance)
                objectsToSpawn = grassBushes;
            else
                objectsToSpawn = flowers;
            var newObject = Instantiate(objectsToSpawn[Random.Range(0, objectsToSpawn.Length)]).transform;
            newObject.position = new Vector3(x, y, z);
            newObject.eulerAngles = new(0f, Random.Range(0f, 360f), 0f);
            var s = Random.Range(0.75f, 1.25f);
            newObject.localScale = new(s, s, s);
        }
    }
}