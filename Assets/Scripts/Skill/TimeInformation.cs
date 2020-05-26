using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 记录物体的位置跟旋转信息,用于实现时光倒流的技能
/// </summary>
public class TimeInformation
{
    public Vector3 position;
    public Quaternion rotation;
    public TimeInformation(Vector3 _position, Quaternion _rotation)
    {
        position = _position;
        rotation = _rotation;
    }
}
