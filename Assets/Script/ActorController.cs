using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ActorController : MonoBehaviour
{
    //模型
    public GameObject model;
    public GameObject step_rotation;
    public GameObject aim;
    public float moveSpeed = 200;
    public float jumpHeight = 5;

    private bool jumping = false;
    private int airTimes = 0;  // 在空中的计数
    [SerializeField] private Animator anim;
    private PlayerInput pi;
    private CharacterController cc;


    void Awake()
    {
        pi = GetComponent<PlayerInput>();
        anim = model.GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
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
        direcMove.Normalize();
        direcMove.y = 0;


        // 移动
        cc.SimpleMove((Quaternion.AngleAxis(90, Vector3.up) * directAim * v_x + directAim * v_y) * Time.fixedDeltaTime * moveSpeed);

        // 设置移动动画
        anim.SetFloat("velocity", Mathf.Sqrt(v_x * v_x + v_y * v_y));
        anim.SetFloat("animSpeed", v_y >= -0.1 ? 1 : -1);



        // 离地
        if (!cc.isGrounded)
        {
            airTimes++;
            // 多于n帧离地才播放空中动作
            if (airTimes > 20)
            {
                anim.SetBool("isGrounded", false);
                pi.moveInputEnable = false;
            }
            if (jumping)
                cc.Move((new Vector3(0, jumpHeight, 0)) * Time.fixedDeltaTime);
        }
        // 在地面
        else
        {
            airTimes = 0;
            anim.SetBool("isGrounded", true);
            if (jumping) {
                jumping = false;
                pi.jump = false;
            } 
            // 关于跳跃
            else if (pi.jump)
            {
                jumping = true;
                anim.SetTrigger("jump");
                cc.Move((cc.velocity + new Vector3(0, jumpHeight, 0)) * Time.fixedDeltaTime);
                anim.SetBool("isGrounded", false);
            }
            pi.moveInputEnable = true;
        }
        // 设置下身旋转
        if (anim.GetBool("isGrounded"))
        {
            step_rotation.transform.rotation = Quaternion.Lerp(step_rotation.transform.rotation, Quaternion.LookRotation(Quaternion.AngleAxis(75 - Mathf.Acos(v_y > -0.1 ? direcMove.x : -direcMove.x) / 3.14f * 150f, Vector3.up) * directAim), 0.3f);
        }
        else {
            step_rotation.transform.rotation = Quaternion.Lerp(step_rotation.transform.rotation, Quaternion.LookRotation(directAim), 0.3f);
        }

    }

}
