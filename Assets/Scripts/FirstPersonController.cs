using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstPersonController : MonoBehaviour
{
    [SerializeField]
    [Serializable]
    public struct Range
    {
        public float min, max;
        public Range(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
    }

    [Header("�ƶ��ٶ�")]
    public float moveSpeed = 100;

    [Header("��Ծ����")]
    public float jumpForce = 10;

    [Header("����")]
    public float gravity = 10f;

    [Header("���������")]
    public float mouseSensitivity = 100;

    [Header("�����Ƿ�Χ")]
    public Range angleRange = new Range(-90, 90);

    [Header("ƽ����ͷ")]
    public bool smoothCamera = false;

    [Header("��ͷ����ֵ")]
    [Range(0, 10)]
    public float damper = 5;

    private Vector3 direction;
    //�Ƿ��ڵ�����
    public bool isGround;
    private float myGravity;
    //�������ת�Ƕ�
    private Vector2 currentViewpointAngle;
    private Vector2 targetViewpointAngle;
    //������ת�Ƕ�
    private Vector2 currentPlayerAngle;
    private Vector2 targetPlayerAngle;
    private Rigidbody rb;
    private GameObject eyes;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        eyes = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        //PlayerMove();

        ViewpointControl();
    }
    private void LateUpdate()
    {
        

    }
    private void FixedUpdate()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        //bool isJump = Input.GetButton("Jump");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("�ո���!");

        }
        direction = (transform.forward * moveZ + transform.right * moveX).normalized;

        if (isGround)//�Ӵ���
        {
            myGravity = 0;
            if (Input.GetKey(KeyCode.Space))
            {
                isGround = false;
                //myGravity += jumpForce;
                rb.AddForce(transform.up*jumpForce);
            }
        }
        else
        {
            myGravity -= gravity * Time.fixedDeltaTime;
            
        }
        //direction.y = myGravity;
        //Debug.Log(direction.y + "�����ٶ�");
        rb.AddForce(direction*moveSpeed);

        //rb.velocity = direction * moveSpeed * Time.deltaTime;


    }

    private void ViewpointControl()
    {
        float moveX = Input.GetAxisRaw("Mouse X");
        float moveY = Input.GetAxisRaw("Mouse Y");
        targetPlayerAngle.y += moveX * mouseSensitivity * Time.deltaTime;
        targetViewpointAngle.x -= moveY * mouseSensitivity * Time.deltaTime;
        //����� �����ӽǷ�Χ���ơ�
        targetViewpointAngle.x = Mathf.Clamp(targetViewpointAngle.x, angleRange.min, angleRange.max);

        if (smoothCamera)
        {
            //ƽ��ת��
            currentPlayerAngle = Vector2.Lerp(currentPlayerAngle, targetPlayerAngle, damper * Time.deltaTime);
            currentViewpointAngle = Vector2.Lerp(currentViewpointAngle, targetViewpointAngle, damper * Time.deltaTime);
        }
        else
        {
            //ʵʱת��
            currentPlayerAngle = targetPlayerAngle;
            currentViewpointAngle = targetViewpointAngle;
        }
        transform.rotation = Quaternion.Euler(currentPlayerAngle);
        eyes.transform.localRotation = Quaternion.Euler(currentViewpointAngle);

    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            isGround = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            isGround = false;
        }
    }
}

