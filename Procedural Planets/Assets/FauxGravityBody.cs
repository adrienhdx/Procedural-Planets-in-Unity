using UnityEngine;

public class FauxGravityBody : MonoBehaviour
{
    public FauxGravityAttractor attractor;
    private Transform currentTransform;

    // Start is called before the first frame update
    void Start()
    {
        

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        rigidbody.useGravity = false;

        currentTransform = transform;
    }

    // Update is called once per frame
    void Update()
    { 
        attractor.Attract(currentTransform);
    }

   
}
