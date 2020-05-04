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
    public GameObject weapon;
    public WeaponManager weaponManager;
    public GameObject bulletPrefab;

    public float moveSpeed = 200f;
    public float jumpHeight = 5f;
    public int airTimes = 0;

    [SerializeField] private Animator anim;
    private PlayerInput pi;
    private Rigidbody rigi;
    private GroundSenser gs;
    private CapsuleCollider capcol;
    private WeaponAttr current_WeaponAttr;

    void Awake()
    {
        if (GetComponent<PhotonView>().IsMine)
            localPlayer = this.gameObject;
        pi = GetComponent<PlayerInput>();
        rigi = GetComponent<Rigidbody>();
        anim = model.GetComponent<Animator>();
        capcol = GetComponent<CapsuleCollider>();
        gs = GetComponent<GroundSenser>();

        // 武器
        weaponManager = GetComponent<WeaponManager>();
        weaponManager.ChangeWeapon(0);
        current_WeaponAttr = weaponManager.currentWeapon.GetComponent<WeaponAttr>();
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
        //direcMove.Normalize();
        direcMove.y = 0;

        // 移动
        rigi.velocity = new Vector3(0, rigi.velocity.y, 0);
        rigi.velocity += (Quaternion.AngleAxis(90, Vector3.up) * directAim * v_x + directAim * v_y) * Time.fixedDeltaTime * moveSpeed;
        // 设置移动动画
        anim.SetFloat("velocity", Mathf.Sqrt(v_x * v_x + v_y * v_y));
        anim.SetFloat("animSpeed", v_y >= -0.1 ? 1 : -1);

        // 离地
        if (!gs.isGrounded){
            anim.SetBool("isGrounded", false);
            pi.moveInputEnable = false;
        }
        // 在地面
        else{
            anim.SetBool("isGrounded", true);
            pi.moveInputEnable = true;
            // 关于跳跃
            if (pi.jump)
            {
                rigi.velocity += new Vector3(0, jumpHeight, 0);
                anim.SetTrigger("jump");
                anim.SetBool("isGrounded", false);
                pi.moveInputEnable = true;
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

        //射击
        if (pi.fire)
        {
            GameObject bullet = Instantiate(bulletPrefab, aim.transform.position, aim.transform.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * current_WeaponAttr.bulletSpeed;
        }
    }

}