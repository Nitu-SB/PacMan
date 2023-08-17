using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThreePersonCameraController : MonoBehaviour
{
    //������
    [Serializable]
    public struct Range
    {
        public float min;
        public float max;
        public Range(int min, int max)
        {
            this.min = min;
            this.max = max;
            //������
        }
    }
    public GameObject target;
    [SerializeField]
    public Range angleRange = new Range(-45, 45);
    [SerializeField]
    public Range distanceRange = new Range(5, 8);
    //������
    public float mouseSpeed = 10;
    public float mouseScaleSpeed = 10;
    Vector2 direction = new Vector2();
    float distance = new float();
    //�Ҹ��Ƶ���vs���

    //z��code��������
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

    //����vscode��ô�о�û����unity �����Զ����
    public void ViewpointControl()
    {
        float moveX = Input.GetAxis("Mouse X");
        float moveY = Input.GetAxis("Mouse Y");

        //����������
        
        direction.x -= moveY * mouseSpeed ;
        direction.y += moveX * mouseSpeed ;
        //����������
        direction.x = Mathf.Clamp(direction.x, angleRange.min, angleRange.max);

        
        //������ž������
        distance -= Input.GetAxis("Mouse ScrollWheel") * mouseScaleSpeed * Time.deltaTime;
        //��������ž�������
        distance = Mathf.Clamp(distance, distanceRange.min, distanceRange.max);

        Debug.Log(direction);
        //���ת��
        this.transform.rotation = Quaternion.Euler(direction);
        //���λ���ƶ�
        this.transform.position = target.transform.position - this.transform.forward * distance;//yuancheng

    }
}

