using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThreePersonCameraController : MonoBehaviour
{
    //来点金币
    [Serializable]
    public struct Range
    {
        public float min;
        public float max;
        public Range(int min, int max)
        {
            this.min = min;
            this.max = max;
            //来点金币
        }
    }
    public GameObject target;
    [SerializeField]
    public Range angleRange = new Range(-45, 45);
    [SerializeField]
    public Range distanceRange = new Range(5, 8);
    //来点金币
    public float mouseSpeed = 10;
    public float mouseScaleSpeed = 10;
    Vector2 direction = new Vector2();
    float distance = new float();
    //我复制到我vs李看看

    //z这code都不会检错
    void Start()
    {
        //Debug.Log(angleRange+distanceRange);
        target = GameObject.Find("Cube");
    }



    void Update()
    {

    }

    void LateUpdate()
    {
        ViewpointControl();
    }

    //你这vscode怎么感觉没连上unity 不会自动填充
    public void ViewpointControl()
    {
        float moveX = Input.GetAxis("Mouse X");
        float moveY = Input.GetAxis("Mouse Y");

        //相机方向控制
        
        direction.x -= moveY * mouseSpeed ;
        direction.y += moveX * mouseSpeed ;
        //俯仰角限制
        direction.x = Mathf.Clamp(direction.x, angleRange.min, angleRange.max);

        
        //相机缩放距离控制
        distance -= Input.GetAxis("Mouse ScrollWheel") * mouseScaleSpeed * Time.deltaTime;
        //西厢记缩放距离限制
        distance = Mathf.Clamp(distance, distanceRange.min, distanceRange.max);

        Debug.Log(direction);
        //相机转动
        this.transform.rotation = Quaternion.Euler(direction);
        //相对位置移动
        this.transform.position = target.transform.position - this.transform.forward * distance;//yuancheng

    }
}

