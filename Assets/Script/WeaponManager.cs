using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Photon.Pun;

public class WeaponManager : MonoBehaviour
{
    public TwoBoneIKConstraint leftHand_IK;
    public TwoBoneIKConstraint rightHand_IK;

    public GameObject[] weaponList;
    public GameObject weapon;
    public GameObject currentWeapon;

    void Start()
    {
        
    }
    void FixedUpdate()
    {
        // 实验
        //if (Input.GetKey("q"))
        //    ChangeWeapon(1);
    }

    // 换武器
    public void ChangeWeapon(int i)
    {
        if(currentWeapon!=null)
            Destroy(currentWeapon);

        Quaternion w_rotation = transform.rotation * weapon.transform.rotation * weaponList[i].transform.rotation;
        currentWeapon = Instantiate(weaponList[i], Vector3.zero, w_rotation);
        currentWeapon.transform.parent = weapon.transform;
        currentWeapon.transform.localPosition = weaponList[i].transform.position;
        // 换持枪姿势
        //leftHand_IK.data.target = currentWeapon.GetComponent<WeaponAttr>().frontHandler.transform;
        //leftHand_IK.data.target.parent = currentWeapon.GetComponent<WeaponAttr>().frontHandler.transform;

        //rightHand_IK.data.target = currentWeapon.GetComponent<WeaponAttr>().backHandler.transform;
        //rightHand_IK.data.target.parent = currentWeapon.GetComponent<WeaponAttr>().backHandler.transform;

    }
}
