using UnityEngine;

public class MapControllerMainEnv : MonoBehaviour
{
   [Header("Zoom Settings")]
    public float minZoom = 2f;
    public float maxZoom = 10f;
    public float zoomSpeed = 2f;

    [Header("Drag Settings")]
    public float dragSpeed = 1f;

    [Header("Boundary Collider")]
    public BoxCollider2D boundaryCollider;

    private Camera cam;
    private Vector3 dragOrigin;

    private float mapMinX, mapMaxX, mapMinY, mapMaxY;

    void Start()
    {
        cam = Camera.main;
        CalculateBoundaryLimits();
    }

    void Update()
    {
        HandleZoom();
        HandleDrag();
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
            ClampCameraPosition();
        }
    }

    void HandleDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position += difference * dragSpeed;
            ClampCameraPosition();
        }
    }

    void CalculateBoundaryLimits()
    {
        Bounds bounds = boundaryCollider.bounds;
        mapMinX = bounds.min.x;
        mapMaxX = bounds.max.x;
        mapMinY = bounds.min.y;
        mapMaxY = bounds.max.y;
    }

    void ClampCameraPosition()
    {
        float cameraHeight = cam.orthographicSize;
        float cameraWidth = cameraHeight * cam.aspect;

        float minX = mapMinX + cameraWidth;
        float maxX = mapMaxX - cameraWidth;
        float minY = mapMinY + cameraHeight;
        float maxY = mapMaxY - cameraHeight;

        float newX = Mathf.Clamp(cam.transform.position.x, minX, maxX);
        float newY = Mathf.Clamp(cam.transform.position.y, minY, maxY);

        cam.transform.position = new Vector3(newX, newY, cam.transform.position.z);
    }

#if UNITY_EDITOR
    // 实时更新边界，便于调整
    void OnDrawGizmosSelected()
    {
        if (boundaryCollider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(boundaryCollider.bounds.center, boundaryCollider.bounds.size);
        }
    }
#endif
}
