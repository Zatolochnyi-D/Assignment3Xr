using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnButtonUi : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI textCounter;

    private int counter = 0;

    void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (WorldSpawner.Instance.SpawnObjects())
            {
                button.gameObject.SetActive(false);
            }
        });
        textCounter.text = counter.ToString();
    }

    public void IncrementCounter()
    {
        counter++;
        textCounter.text = counter.ToString();
    }
}
