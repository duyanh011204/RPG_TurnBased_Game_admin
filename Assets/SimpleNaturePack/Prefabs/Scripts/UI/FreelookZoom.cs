using UnityEngine;
using Cinemachine;

public class FreeLookZoom : MonoBehaviour
{
    public CinemachineFreeLook freeLook;
    public float zoomSpeed = 2f;
    public float minRadius = 5f;
    public float maxRadius = 15f;

    void Start()
    {
        if (freeLook == null)
            freeLook = GetComponent<CinemachineFreeLook>();
    }

    void Update()
    {
        if (freeLook == null) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            for (int i = 0; i < 3; i++)
            {
                var orbit = freeLook.m_Orbits[i];
                orbit.m_Radius = Mathf.Clamp(orbit.m_Radius - scroll * zoomSpeed, minRadius, maxRadius);
                freeLook.m_Orbits[i] = orbit;
            }
        }
    }
}
