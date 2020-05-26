using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDate : MonoBehaviour
{
    public bool isRewinding = false;//用来判断是否需要时光逆流
    public float recordTime;//时光逆流时间

    private List<TimeInformation> informations;
    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        informations = new List<TimeInformation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject != PlayerManager.localPlayer) return;
        //按下shift开始时光倒流
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartRewind();
            GetComponent<GroundSenser>().isTimeTrans = true;

        }
        //松开时停止
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopRewind();
            GetComponent<GroundSenser>().isTimeTrans = false;
        }
    }

    void FixedUpdate()
    {
        if (isRewinding)
            Rewind();
        else
            Record();

    }

    /// <summary>
    /// 开始时光逆流
    /// </summary>
    private void StartRewind()
    {
        isRewinding = true;
        //rb.isKinematic = true;//使物体不受力
    }

    /// <summary>
    /// 停止时光逆流
    /// </summary>
    private void StopRewind()
    {
        isRewinding = false;
        //rb.isKinematic = false;//物体开始受力
    }

    /// <summary>
    /// 时光逆流
    /// </summary>
    private void Rewind()
    {
        //记录点数量大于0时才可以倒流,二倍速回流
        if (informations.Count > 1)
        {
            TimeInformation information = informations[1];
            transform.position = information.position;
            transform.rotation = information.rotation;
            informations.RemoveAt(0);
            informations.RemoveAt(0);
        }
    }

    /// <summary>
    /// 记录物体的信息
    /// </summary>
    private void Record()
    {
        if (informations.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
        {
            informations.RemoveAt(informations.Count - 1);
        }
        informations.Insert(0, new TimeInformation(transform.position, transform.rotation));
    }
}
