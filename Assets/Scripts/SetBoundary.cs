using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBoundary : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
         transform.position = target.position;
    }

}
