using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Enemy_Child_KWS : MonoBehaviour
{
    Rigidbody rigid;

    [Range(1f, 10f)]
    public float jumpHeight = 1f;

    public float cooltime = 0;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        if (cooltime >= 0.5f)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        // 부모 오브젝트의 위치 정보 가져오기
        Vector3 parentPosition = transform.parent.position;

        // 부모 오브젝트의 x와 z 값을 자식 오브젝트에게 적용
        transform.position = new Vector3(parentPosition.x, transform.position.y, parentPosition.z);
    }

    private void Jump()
    {
        cooltime = 0;

        // 점프할 방향과 힘 설정
        Vector3 jumpDirection = Vector3.up * jumpHeight;

        // Rigidbody에 힘을 가해서 점프시킴
        rigid.AddForce(jumpDirection, ForceMode.Impulse);

        // 점프할 때 회전을 막기 위해 angularVelocity를 0으로 설정
        rigid.angularVelocity = Vector3.zero;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Jump();
            cooltime += Time.deltaTime;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            
        }
    }


}
