using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed;
    Planet planet;
    SphereCollider col;

    private void Start()
    {
        col = GetComponent<SphereCollider>();
        UpdateCollider();
    }
    private void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * rotationSpeed * Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y") * rotationSpeed * Mathf.Deg2Rad;

        transform.RotateAround(Vector3.up, -rotX);
        transform.RotateAround(Vector3.right, rotY);

    }
    void UpdateCollider()
    {
        planet = GetComponent<Planet>();
        col.radius = planet.shapeSettings.planetRadius;

    }
}
