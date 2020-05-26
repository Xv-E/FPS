using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSenser : MonoBehaviour
{
    public CapsuleCollider capcol;
    public float senser_option=0.05f;
    public bool isGrounded;
    public bool isTimeTrans = false;
    private Vector3 point1;
    private Vector3 point2;
    private float radius;
    // Start is called before the first frame update
    void Start()
    {
        radius = capcol.radius - 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        point1 = transform.position + transform.up * radius + Vector3.down * senser_option;
        point2 = transform.position - transform.up * radius + transform.up * capcol.height;

        Collider[] outputCols = Physics.OverlapCapsule(point1, point2, radius, LayerMask.GetMask("Character")|LayerMask.GetMask("Ground"));
        if (outputCols.Length > 1 && !isTimeTrans)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
