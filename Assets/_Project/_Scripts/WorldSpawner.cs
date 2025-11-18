using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using System.Linq;

public class WorldSpawner : MonoBehaviour
{
    public static WorldSpawner Instance { get; private set; }

    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private float spawnRadius = 1f;
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private int objectsToSpawn = 20;

    void Awake()
    {
        Instance = this;
    }

    public void SpawnObjects()
    {
        var userPosition = Camera.main.transform.position;
        var planes = new List<ARPlane>();
        foreach (var plane in planeManager.trackables)
            if (plane.alignment == UnityEngine.XR.ARSubsystems.PlaneAlignment.HorizontalUp)
            {
                var planeTopY = plane.transform.position.y + plane.extents.y;
                var planeTopX = plane.transform.position.x + plane.extents.x;
                var planeBottomY = plane.transform.position.y - plane.extents.y;
                var planeBottomX = plane.transform.position.y - plane.extents.x;
                var userLocal = plane.transform.worldToLocalMatrix.MultiplyPoint(userPosition);
                var userX = userLocal.x;
                var userY = userLocal.y;

                if (userY <= planeTopY && userY >= planeBottomY && userX <= planeTopX && userX >= planeBottomX)
                    planes.Add(plane);
            }

        var points = new List<Vector3>();
        for (int i = 0; i < objectsToSpawn; i++)
        {
            var inCylinder = Random.insideUnitCircle * spawnRadius;
            var plane = planes[Random.Range(0, planes.Count)].transform;
            var a = plane.up.x;
            var b = plane.up.y;
            var c = plane.up.z;
            var d = -(a * plane.position.x + b * plane.position.y + c * plane.position.z);
            var x = inCylinder.x;
            var z = inCylinder.y;
            var y = -(a * x + c * z + d) / b;
            Instantiate(objectToSpawn, new Vector3(x, y, z), Quaternion.identity);
        }
    }
}