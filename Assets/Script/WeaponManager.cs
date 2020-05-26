using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Photon.Pun;
using Com.MyCompany.MyGame;

public class WeaponManager : MonoBehaviour, IPunObservable
{
    public TwoBoneIKConstraint leftHand_IK;
    public TwoBoneIKConstraint rightHand_IK;
    
    public static List<KeyValuePair<object, int>> allWeapon; // 所有武器预设体

    public List<GameObject> weaponList = new List<GameObject>(); // 已有武器
    public int weaponCode = 0; // 当前武器在list的编号
    public GameObject currentWeapon; // 当前装备武器
    public WeaponAttr current_WeaponAttr;
    public GameObject weapon; // 武器节点添加在此节点下

    public GameObject bulletPrefab; // 子弹预设体
    public GameObject aim;
    public GameObject handGrenade;//手雷预制体
    private GameObject metalParticle;
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
            allWeapon = new List<KeyValuePair<object, int>>();
            object[] twist = { };
            allWeapon.Add(new KeyValuePair<object, int>(Resources.Load("weapon/Ak-47"), 200));
            allWeapon.Add(new KeyValuePair<object, int>(Resources.Load("weapon/M4A1 Sopmod"), 200));
            allWeapon.Add(new KeyValuePair<object, int>(Resources.Load("weapon/SCAR"), 180));
            allWeapon.Add(new KeyValuePair<object, int>(Resources.Load("weapon/AWP"), 300));
            allWeapon.Add(new KeyValuePair<object, int>(Resources.Load("weapon/SG550"), 200));
            allWeapon.Add(new KeyValuePair<object, int>(Resources.Load("weapon/B43"), 150));
            allWeapon.Add(new KeyValuePair<object, int>(Resources.Load("weapon/UMP-45"), 100));
            allWeapon.Add(new KeyValuePair<object, int>(Resources.Load("weapon/P90"), 100));
            allWeapon.Add(new KeyValuePair<object, int>(Resources.Load("weapon/Skorpion VZ"), 100));
            allWeapon.Add(new KeyValuePair<object, int>(Resources.Load("weapon/shotgun1"), 100));
            allWeapon.Add(new KeyValuePair<object, int>(Resources.Load("weapon/Skorpion VZ"), 100));//以后替换成连发霰弹
            allWeapon.Add(new KeyValuePair<object, int>(Resources.Load("weapon/RocketLauncher"), 300));
        }
        // 为角色添加武器 应该由商店完成
        addWeapon(0);
        handGrenade = Resources.Load("Hand_Grenade_Prefab") as GameObject;
        metalParticle = Resources.Load("Particle/Metal Impact Prefab") as GameObject;
    }
    void Update()
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
                GameObject bullet;
                if (!current_WeaponAttr.weaponName.Equals("RocketLauncher"))
                {
                    switch (current_WeaponAttr.weaponName)
                    {
                        case "shotgun1":
                            bullet = PhotonNetwork.Instantiate("ShotGunBullet", aim.transform.position, aim.transform.rotation);
                            break;
                        default:
                            bullet = PhotonNetwork.Instantiate(bulletPrefab.name, aim.transform.position, aim.transform.rotation);
                            break;
                    }
                    //GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, aim.transform.position, aim.transform.rotation);
                    bullet.GetComponent<BulletManager>().from_player = this.gameObject;
                    bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * current_WeaponAttr.bulletSpeed;
                    bullet.GetComponent<BulletManager>().damage = current_WeaponAttr.damage;
                    bullet.GetComponent<BulletManager>().from_player = this.gameObject;
                    GameObject particle = Instantiate(metalParticle, aim.transform.position, aim.transform.rotation);
                    particle.transform.Find("Metal Bullet Hole Particle").gameObject.SetActive(false);
                    Destroy(particle, 1f);
                }
                if(current_WeaponAttr.weaponName.Equals("RocketLauncher"))
                {
                    bullet = PhotonNetwork.Instantiate("RocketLauncherBullet", aim.transform.position, aim.transform.rotation);
                    bullet.GetComponent<BulletManager>().from_player = this.gameObject;
                    bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * current_WeaponAttr.bulletSpeed;
                    bullet.GetComponent<BulletManager>().damage = current_WeaponAttr.damage;
                    bullet.GetComponent<BulletManager>().from_player = this.gameObject;
                    //GameObject particle = Instantiate(metalParticle, aim.transform.position, aim.transform.rotation);
                    //particle.transform.Find("Metal Bullet Hole Particle").gameObject.SetActive(false);
                    //Destroy(particle, 1f);
                }
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

        // 换持枪姿势
        if (currentWeapon!=null){
            leftHand_IK.data.target.position = currentWeapon.GetComponent<WeaponAttr>().frontHandler.transform.position;
            leftHand_IK.data.target.rotation = currentWeapon.GetComponent<WeaponAttr>().frontHandler.transform.rotation;
            rightHand_IK.data.target.position = currentWeapon.GetComponent<WeaponAttr>().backHandler.transform.position;
            rightHand_IK.data.target.rotation = currentWeapon.GetComponent<WeaponAttr>().backHandler.transform.rotation;
        }
        //扔手雷
        if (pi.boom)
        {
            
            GameObject handgrenade = PhotonNetwork.Instantiate(handGrenade.name, aim.transform.position, aim.transform.rotation);
            handgrenade.GetComponent<Rigidbody>().velocity = handgrenade.transform.forward * 20f;
            handgrenade.GetComponent<HandGrenade>().fromplayer = this.gameObject;
            pi.boom = false;
        }

    }
    // 增加武器
    public void addWeapon(int i) {
        GameObject w = (GameObject)allWeapon[i].Key;
        Quaternion w_rotation = weapon.transform.rotation * w.transform.rotation;
        var newWeapon = Instantiate(w , Vector3.zero, w_rotation);
        newWeapon.transform.parent = weapon.transform;
        newWeapon.transform.localPosition = w.transform.position;
        weaponList.Add(newWeapon);
        
        if (currentWeapon != null)
            newWeapon.gameObject.SetActive(false);
        else {
            //currentWeapon = newWeapon;
            //current_WeaponAttr = currentWeapon.GetComponent<WeaponAttr>();
            ChangeWeapon(0);
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
    }

    // 换弹
    public void reload() {
        // 无后备子弹
        if (current_WeaponAttr.backup == 0 ||current_WeaponAttr.currentBulletNum==current_WeaponAttr.bulletNum) {
            reloading = false;
            reloadTimes = 0;
            return;
        }
        if (reloadTimes > 0)
            reloadTimes--;
        else
        {
            int temp = current_WeaponAttr.backup + current_WeaponAttr.currentBulletNum;
            current_WeaponAttr.currentBulletNum = temp > current_WeaponAttr.bulletNum ? current_WeaponAttr.bulletNum : temp;
            current_WeaponAttr.backup = temp - current_WeaponAttr.currentBulletNum;
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
