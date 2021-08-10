using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{

    public float DestroyTme = 3;
    public Vector3 Offset = new Vector3(0, 0.5f, 0);
    public Vector3 RandomizeIntensity = new Vector3(0.5f, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, DestroyTme);
        transform.localPosition += Offset;
        transform.localPosition += new Vector3(Random.Range(-RandomizeIntensity.x,RandomizeIntensity.x), 
            Random.Range(-RandomizeIntensity.y, RandomizeIntensity.y),
            Random.Range(-RandomizeIntensity.z, RandomizeIntensity.z));

        transform.GetComponent<Rigidbody>().AddForce(transform.up * 0.5f, ForceMode.Impulse);
        transform.GetComponent<Rigidbody>().AddForce(-transform.forward * 0.2f, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
