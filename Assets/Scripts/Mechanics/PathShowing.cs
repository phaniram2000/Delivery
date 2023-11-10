using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathShowing : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation,
            Quaternion.LookRotation(target.position - transform.position),
            rotationSpeed * Time.deltaTime);
    }
}