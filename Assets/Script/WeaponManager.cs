using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Photon.Pun;
using Com.MyCompany.MyGame;

public class WeaponManager : MonoBehaviour, IPunObservable
{
    //public TwoBoneIKConstraint leftHand_IK;
    //public TwoBoneIKConstraint rightHand_IK;

    static List<GameObject> allWeapon; // 所有武器预设体

    public List<GameObject> weaponList = new List<GameObject>(); // 已有武器
    public int weaponCode = 0; // 当前武器在list的编号
    public GameObject currentWeapon; // 当前装备武器
    public WeaponAttr current_WeaponAttr;
    public GameObject weapon; // 武器节点添加在此节点下

    public GameObject bulletPrefab; // 子弹预设体
    public GameObject aim;

    private PlayerInput pi;


    public bool reloading = false;
    // 计数器
    public int reloadTimes = 0;
    public int fireTimes = 0;

    void Start()
    {
        pi = GetComponent<PlayerInput>();
        // 全局静态资源只初始化一次
        if (allWeapon == null) {
            allWeapon = new List<GameObject>();
            allWeapon.Add((GameObject)Resources.Load("weapon/Ak-47"));
            allWeapon.Add((GameObject)Resources.Load("weapon/M4A1 Sopmod"));
        }
        // 为角色添加武器 应该由商店完成
        addWeapon(0);
        addWeapon(1);

    }
    void FixedUpdate()
    {
        // 切换武器
        if (pi.changeLeft)
            weaponCode = (weaponCode - 1 + weaponList.Count) % weaponList.Count;
        if (pi.changeRight)
            weaponCode = (weaponCode + 1) % weaponList.Count;
        ChangeWeapon(weaponCode);
        pi.changeLeft = false;
        pi.changeRight = false;
        // 换弹指令
        if (pi.reload&&!reloading)
        {
            reloading = true;
            reloadTimes = current_WeaponAttr.reload_time;
            pi.reload = false;
        }

        //射击 
        if (pi.fire && fireTimes == 0 && !reloading)
        {
            // 如果有子弹
            if (current_WeaponAttr.currentBulletNum > 0)
            {
                current_WeaponAttr.currentBulletNum--;
                fireTimes = current_WeaponAttr.shootSpeed;
                //GameObject bullet = Instantiate(bulletPrefab, aim.transform.position, aim.transform.rotation);
                GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, aim.transform.position, aim.transform.rotation);
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * current_WeaponAttr.bulletSpeed;
                bullet.GetComponent<BulletManager>().damage = current_WeaponAttr.damage;
                bullet.GetComponent<BulletManager>().from_player = this.gameObject;
            }
            else
            {
                reloading = true;
                reloadTimes = current_WeaponAttr.reload_time;
            }

        }
        else if (fireTimes > 0)
        {
            fireTimes--;
        }

        // 换弹
        if (reloading)
        {
            reload();
        }
    }
    // 增加武器
    public void addWeapon(int i) {
        Quaternion w_rotation = transform.rotation * weapon.transform.rotation * allWeapon[i].transform.rotation;
        var newWeapon = Instantiate(allWeapon[i], Vector3.zero, w_rotation);
        newWeapon.transform.parent = weapon.transform;
        newWeapon.transform.localPosition = allWeapon[i].transform.position;
        weaponList.Add(newWeapon);
        
        if (currentWeapon != null)
            newWeapon.gameObject.SetActive(false);
        else {
            currentWeapon = newWeapon;
            current_WeaponAttr = currentWeapon.GetComponent<WeaponAttr>();
        }

    }

    // 换武器
    public void ChangeWeapon(int i)
    {
        if (weaponList[i] == currentWeapon)
            return;
        // 重置计数
        reloading = false;
        reloadTimes = 0;
        fireTimes = 0;

        if (currentWeapon != null)
            currentWeapon.gameObject.SetActive(false);
        weaponList[i].gameObject.SetActive(true);
        currentWeapon = weaponList[i];
        current_WeaponAttr = currentWeapon.GetComponent<WeaponAttr>();
        // 换持枪姿势
        //leftHand_IK.data.target = currentWeapon.GetComponent<WeaponAttr>().frontHandler.transform;
        //leftHand_IK.data.target.parent = currentWeapon.GetComponent<WeaponAttr>().frontHandler.transform;

        //rightHand_IK.data.target = currentWeapon.GetComponent<WeaponAttr>().backHandler.transform;
        //rightHand_IK.data.target.parent = currentWeapon.GetComponent<WeaponAttr>().backHandler.transform;
    }

    // 换弹
    public void reload() {
        // 无后备子弹
        if (current_WeaponAttr.backup == 0) {
            reloading = false;
            reloadTimes = 0;
            return;
        }
        if (reloadTimes > 0)
            reloadTimes--;
        else
        {
            int temp = current_WeaponAttr.backup + current_WeaponAttr.currentBulletNum;
            current_WeaponAttr.currentBulletNum = current_WeaponAttr.backup > current_WeaponAttr.bulletNum ? current_WeaponAttr.bulletNum : current_WeaponAttr.backup;
            current_WeaponAttr.backup = temp-current_WeaponAttr.currentBulletNum;
            reloading = false;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(weaponCode);
        }
        else
        {
            weaponCode=(int)stream.ReceiveNext();
        }
    }
}
