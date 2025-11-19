using System.Linq;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    [SerializeField] private SpawnButtonUi ui;

    void Start()
    {
        InputManager.Instance.OnPointerClick += HandleClick;
    }

    private void HandleClick()
    {
        var ray = Camera.main.ScreenPointToRay(InputManager.Instance.PointerScreenPosition);
        var result = Physics.RaycastAll(ray).Where(x => x.collider.gameObject.TryGetComponent(out ButterflyController _));
        if (result.Any())
        {
            ui.IncrementCounter();
            Destroy(result.First().collider.gameObject);
        }
    }
}