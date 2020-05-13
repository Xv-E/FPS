using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class PlayerManager : MonoBehaviour
{
    static public GameObject localPlayer;

    public GameObject model; // 模型
    public GameObject step_rotation;
    public GameObject aim;

    [SerializeField] private Animator anim;
    private PlayerInput pi;
    private CharacterAttr ca;
    private Rigidbody rigi;
    private GroundSenser gs;
    private CapsuleCollider capcol;

    private Vector3 speedAir=Vector3.zero; 
    // 计数
    public int airTimes = 0;

    void Awake()
    {
        if (GetComponent<PhotonView>().IsMine)
            localPlayer = this.gameObject;
        pi = GetComponent<PlayerInput>();
        ca = GetComponent<CharacterAttr>();
        rigi = GetComponent<Rigidbody>();
        anim = model.GetComponent<Animator>();
        capcol = GetComponent<CapsuleCollider>();
        gs = GetComponent<GroundSenser>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float v_x = pi.velocity.x;
        float v_y = pi.velocity.y;

        Vector3 directAim = aim.transform.position - transform.position;
        directAim.Normalize();
        directAim.y = 0;
        Vector3 direcMove = new Vector3(v_x, 0, v_y);
 
        // 离地
        if (!gs.isGrounded){
            anim.SetBool("isGrounded", false);
            pi.moveInputEnable = false;
            rigi.velocity = new Vector3(0, rigi.velocity.y, 0);
            rigi.velocity += speedAir;
        }
        // 在地面
        else{
            // 移动
            rigi.velocity = new Vector3(0, rigi.velocity.y, 0);
            rigi.velocity += (Quaternion.AngleAxis(90, Vector3.up) * directAim * v_x + directAim * v_y) * Time.fixedDeltaTime * ca.moveSpeed;
            // 设置移动动画
            anim.SetFloat("velocity", Mathf.Sqrt(v_x * v_x + v_y * v_y));
            anim.SetFloat("animSpeed", v_y >= -0.1 ? 1 : -1);
            anim.SetBool("isGrounded", true);
            pi.moveInputEnable = true;
            // 关于跳跃
            if (pi.jump)
            {
                speedAir = rigi.velocity;
                speedAir.y = 0;
                rigi.velocity += new Vector3(0, ca.jumpHeight, 0);
                anim.SetTrigger("jump");
                anim.SetBool("isGrounded", false);
                pi.moveInputEnable = false;
                pi.jump = false;
            }
        }

        // 设置下身旋转
        if (anim.GetBool("isGrounded"))
        {
            step_rotation.transform.rotation = Quaternion.Lerp(step_rotation.transform.rotation, Quaternion.LookRotation(Quaternion.AngleAxis(75 - Mathf.Acos(v_y > -0.1 ? direcMove.x : -direcMove.x) / 3.14f * 150f, Vector3.up) * directAim), 0.3f);
        }
        else
        {
            step_rotation.transform.rotation = Quaternion.Lerp(step_rotation.transform.rotation, Quaternion.LookRotation(directAim), 0.3f);
        }


    }

}