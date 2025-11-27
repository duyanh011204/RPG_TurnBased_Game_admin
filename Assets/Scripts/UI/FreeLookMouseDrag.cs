using UnityEngine;
using Cinemachine;

public class FreeLookMouseDrag : MonoBehaviour
{
    public CinemachineFreeLook freeLook;
    public int mouseButton = 0; 
    public float sensitivity = 0.002f; 
    private Vector3 lastMousePosition;
    private bool dragging = false;

    void Start()
    {
        if (freeLook == null)
            freeLook = GetComponent<CinemachineFreeLook>();

    
        freeLook.m_XAxis.m_InputAxisName = "";
        freeLook.m_YAxis.m_InputAxisName = "";
    }

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(mouseButton))
        {
            dragging = true;
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(mouseButton))
        {
            dragging = false;
            freeLook.m_XAxis.m_InputAxisValue = 0;
            freeLook.m_YAxis.m_InputAxisValue = 0;
        }

        if (dragging)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;

         
            freeLook.m_XAxis.m_InputAxisValue = delta.x * sensitivity;
            freeLook.m_YAxis.m_InputAxisValue = delta.y * sensitivity;
        }
    }
}

