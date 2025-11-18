using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ObjectSpawner : MonoBehaviour
{
    private static readonly List<ARRaycastHit> hits = new();

    [SerializeField] private GameObject prefabToPlace;
    [SerializeField] private ARRaycastManager raycastManager;

    void Start()
    {
        InputManager.Instance.OnPointerClick += SpawnObject;
    }

    private void SpawnObject()
    {
        if (raycastManager.Raycast(InputManager.Instance.PointerScreenPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            Instantiate(prefabToPlace, hitPose.position, hitPose.rotation);
        }
    }
}