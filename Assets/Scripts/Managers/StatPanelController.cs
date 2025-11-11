using UnityEngine;

public class StatPanelController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject statPanel;

    void Start()
    {
        if (statPanel != null)
            statPanel.SetActive(false);
    }

    public void ToggleStatPanel()
    {
        if (statPanel != null)
            statPanel.SetActive(!statPanel.activeSelf);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleStatPanel();
        }
    }
}
