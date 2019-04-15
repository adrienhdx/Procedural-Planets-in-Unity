using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed;
    Planet planet;
    SphereCollider col;
    public bool updateRadius = false;

    private void Start()
    {
        if (updateRadius)
        {
        col = GetComponent<SphereCollider>();
        UpdateCollider();

        }
    }
    private void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * rotationSpeed;
        float rotY = Input.GetAxis("Mouse Y") * rotationSpeed;

        transform.RotateAround(transform.position, Vector3.up, -rotX);
        transform.RotateAround(transform.position, Vector3.right, rotY);


    }
    void UpdateCollider()
    {
        
        planet = GetComponent<Planet>();
        col.radius = planet.shapeSettings.planetRadius;
        
    }
}
