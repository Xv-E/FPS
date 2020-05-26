using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneExplosionManager : MonoBehaviour
{
    public bool isex = false;
    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("setexplosion", 10, 10);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void setexplosion()
    {
        if(isex == false)
        {
            isex = true;
            explosion.SetActive(true);
        }
        else
        {
            isex = false;
            explosion.SetActive(false);
        }
    }
}
