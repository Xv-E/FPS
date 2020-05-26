using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
