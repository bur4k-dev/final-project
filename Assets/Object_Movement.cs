using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false;
    private float distanceToCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        //Mouse'a týklayýnca
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    isDragging = true;
                    distanceToCamera = hit.distance;
                }
            }
        }

        //Mouse'u sürüklerken
        if (isDragging)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Vector3 worldPosition = ray.GetPoint(distanceToCamera);
            transform.position = new Vector3(worldPosition.x, transform.position.y, worldPosition.z);
        }

        //Mouse'u býraktýðýnda
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}
