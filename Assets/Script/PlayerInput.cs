using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 玩家输入模块
public class PlayerInput : MonoBehaviour, IPunObservable
{
    //intput key
    public string keyUp;
    public string keyDown;
    public string keyLeft;
    public string keyRight;
    public string keyJump;

    public float mouseX;
    public float mouseY;

    public bool moveInputEnable=true;
    public Vector2 velocity;  // 移动速度
    public bool jump = false; // 跳跃
    public bool fire = false; // 开火

    //切换镜头
    public bool changeCamera = false;
    // 切换武器
    public bool changeLeft = false;
    public bool changeRight = false;
    // 换弹
    public bool reload = false;

    private float Dup;
    private float Dright;
    private float targetDup;
    private float targetDright;
    private float velocityDup;
    private float velocityDright;


    static public GameObject localPlayer;

    void Awake()
    {
        Screen.fullScreen = false;  //退出全屏           
        Screen.SetResolution(800, 600, false);
        if (GetComponent<PhotonView>().IsMine) localPlayer = this.gameObject;
        //DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 不是本地用户角色
        if (this.gameObject != PlayerManager.localPlayer) return;

        // 鼠标事件
        if (Input.GetButtonDown("Fire1"))
            fire = true;
        if (Input.GetButtonUp("Fire1"))
            fire = false;
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        // 切换第一(三)人称
        changeCamera = Input.GetKeyDown("t");
        // 切换武器
        changeLeft = Input.GetKeyDown("q");
        changeRight = Input.GetKeyDown("e");
        //换弹
        reload = Input.GetKeyDown("r");

        // 移动跳跃指令
        if (moveInputEnable) { 
            targetDup = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);
            targetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);

            Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.2f);
            Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.2f);

            velocity = squareToCircle(new Vector2(Dright, Dup));

            //jump = false;
            if (Input.GetKeyDown(KeyCode.Space)) {
                jump = true;
            }
        }


    }

    // 椭圆映射
    Vector2 squareToCircle(Vector2 square) {
        Vector2 circle = Vector2.zero;

        circle.x = square.x * Mathf.Sqrt(1.0f - (square.y * square.y) / 2.0f);
        circle.y = square.y * Mathf.Sqrt(1.0f - (square.x * square.x) / 2.0f);

        return circle;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            //stream.SendNext(fire);
            stream.SendNext(changeLeft);
            stream.SendNext(changeRight);
            stream.SendNext(reload);
        }
        else
        {
            //fire = (bool)stream.ReceiveNext();
            changeLeft = (bool)stream.ReceiveNext();
            changeRight = (bool)stream.ReceiveNext();
            reload = (bool)stream.ReceiveNext();
        }
    }
}
