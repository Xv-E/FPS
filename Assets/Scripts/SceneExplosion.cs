using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneExplosion : MonoBehaviour
{
    public int dire = 1;
    public int isup = 0;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("setdire", 1, 0.3f);
        InvokeRepeating("setup", 1, 5);
    }

    // Update is called once per frame
    void Update()
    {
    }
    void setdire()
    {
        dire++;
        if(dire>8)
        {
            dire = dire % 8;
        }
    }
    void setup()
    {
        if(isup == 0)
        {
            isup = 1;
        }
        else
        {
            isup = 0;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Rigidbody>())
        {
            switch(dire)
            {
                case 1: other.GetComponent<Rigidbody>().AddForce((new Vector3(1,isup,0)) * 100);
                    break;
                case 2:
                    other.GetComponent<Rigidbody>().AddForce((new Vector3(1, isup, -1)) * 100);
                    break;
                case 3:
                    other.GetComponent<Rigidbody>().AddForce((new Vector3(0, isup, -1)) * 100);
                    break;
                case 4:
                    other.GetComponent<Rigidbody>().AddForce((new Vector3(-1, isup, -1)) * 100);
                    break;
                case 5:
                    other.GetComponent<Rigidbody>().AddForce((new Vector3(-1, isup, 0)) * 100);
                    break;
                case 6:
                    other.GetComponent<Rigidbody>().AddForce((new Vector3(-1, isup, 1)) * 100);
                    break;
                case 7:
                    other.GetComponent<Rigidbody>().AddForce((new Vector3(0, isup, 1)) * 100);
                    break;
                case 8:
                    other.GetComponent<Rigidbody>().AddForce((new Vector3(1, isup, 1)) * 100);
                    break;
                default:break;
            }
        }
    }
}
