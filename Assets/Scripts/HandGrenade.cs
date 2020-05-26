using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.MyCompany.MyGame;
using Photon.Pun;
public class HandGrenade : MonoBehaviour
{
    private GameObject explodeParticle;
    public GameObject fromplayer;
    // Start is called before the first frame update
    void Start()
    {
        explodeParticle = Resources.Load("Particle/Gas Tank Explosion Prefab") as GameObject;
        Invoke("boom", 3f);
    }

    void boom()
    {
        GameObject particle = Instantiate(explodeParticle, this.transform.position, this.transform.rotation);
        particle.GetComponent<ExplodeCollider>().fromplayer = this.fromplayer;
        particle.GetComponentInChildren<ExplodeCollider>().fromplayer = this.fromplayer;
        Destroy(particle,1f);
        PhotonNetwork.Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
