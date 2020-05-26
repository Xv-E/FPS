using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ShockWave : MonoBehaviour
{
    private GameObject holeParticle;
    public GameObject aim;
    // Start is called before the first frame update
    void Start()
    {
        holeParticle = Resources.Load("Particle/Explosion9") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //if(!this.GetComponent<PhotonView>().IsMine)
        //{
        //    return;
        //}
        Debug.DrawRay(aim.transform.position, aim.transform.forward*100,Color.red);
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            RaycastHit hit;
            bool grounded = Physics.Raycast(aim.transform.position, aim.transform.forward*100, out hit);
           
            // 可控制投射距离bool grounded = Physics.Raycast(transform.position, -Vector3.up, out hit,100.0);
            if (grounded)
            {
                GameObject particle = Instantiate(holeParticle, hit.point, hit.transform.rotation);
                particle.GetComponent<ExplodeCollider>().fromplayer = this.gameObject;
                particle.GetComponentInChildren<ExplodeCollider>().fromplayer = this.gameObject;
                Destroy(particle, 2f);
            }
            else
            {
            }
            
        }
    }
}
