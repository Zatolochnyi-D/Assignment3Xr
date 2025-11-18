using UnityEngine;
using UnityEngine.UI;

public class SpawnButtonUi : MonoBehaviour
{
    [SerializeField] private Button button;

    void Start()
    {
        button.onClick.AddListener(() =>
        {
            WorldSpawner.Instance.SpawnObjects();
        });  
    }
}
