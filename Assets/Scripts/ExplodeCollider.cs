using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Com.MyCompany.MyGame;
public class ExplodeCollider : MonoBehaviour
{
    public int mindamage = 34;
    public GameObject fromplayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        // 8是character层
        if (other.gameObject.layer == 8)
        {
            other.GetComponent<CharacterAttr>().health -= mindamage;
            
            other.GetComponent<Rigidbody>().AddForce((other.transform.position - this.transform.position) * 1500);
            if (other.GetComponent<CharacterAttr>().health < 0)
            {
                if (other.GetComponent<PhotonView>().IsMine)
                {
                    Debug.Log("create leave button from expolde collider");
                    Cursor.lockState = CursorLockMode.None;
                    GameObject leaveButton = Instantiate(Resources.Load("Particle/LeaveCanvas") as GameObject, this.transform.position, this.transform.rotation); 
                }
                Destroy(other.gameObject);
                fromplayer.GetComponent<CharacterAttr>().money += 200;
            }
        }
        if(other.GetComponent<Rigidbody>())
        {
            other.GetComponent<Rigidbody>().AddForce((other.transform.position - this.transform.position) * 500);
        }
    }
        // Update is called once per frame
        void Update()
    {
        
    }
    
}
