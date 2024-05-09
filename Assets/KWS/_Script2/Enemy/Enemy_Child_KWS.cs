using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditorInternal;
using UnityEngine;

public class Enemy_Child_KWS : MonoBehaviour
{
    Rigidbody rigid;

    [Range(1f, 10f)]
    public float jumpHeight = 1f;

    public float cooltime = 0;

    public float currentY = 9;

    public float groundCheckDistance = 1.0f;

    public LayerMask groundLayer;

    /// <summary>
    /// 부모 오브젝트
    /// </summary>
    Enemy enemyParent;

    Animator animator;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.constraints = RigidbodyConstraints.FreezeRotation;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        enemyParent = transform.parent.GetComponent<Enemy>();
        enemyParent.onRaise += GravityOff;
        enemyParent.OnChase += samePosition;
        enemyParent.OnChase += GravityOn;
    }

    private void GravityOff()
    {
        Debug.Log("GravityOff 실행");
        rigid.useGravity = false;
        animator.SetTrigger("Idle");
    }

    private void GravityOn()
    {
        Debug.Log("GravityOn 실행");
        rigid.useGravity = true;
        animator.SetTrigger("Walk");
    }

    private void samePosition()
    {
        // 부모 오브젝트의 위치 정보 가져오기
        Vector3 parentPosition = transform.parent.position;

        // 부모 오브젝트의 x와 z 값을 자식 오브젝트에게 적용
        transform.position = new Vector3(parentPosition.x, transform.position.y, parentPosition.z);
    }

    private void Update()
    {
        /*if (cooltime >= 0.5f)
        {
            Jump();
        }*/
        if (IsGrounded())
        {
            cooltime += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        //samePosition();
    }

    public void Jump()
    {
        if(cooltime > 0.5f)
        {
            Debug.Log("점프 실행");
            cooltime = 0;
            // 점프할 방향과 힘 설정
            Vector3 jumpDirection = Vector3.up * jumpHeight;

            // Rigidbody에 힘을 가해서 점프시킴
            rigid.AddForce(jumpDirection, ForceMode.Impulse);

            // 점프할 때 회전을 막기 위해 angularVelocity를 0으로 설정
            rigid.angularVelocity = Vector3.zero;
        }
    }
    
    public bool IsGrounded()
    {
        // 캐릭터의 아래에 레이캐스트를 쏴서 바닥에 닿았는지 확인
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, layerMask: groundLayer);
    }

    public IEnumerator RaiseTrap()
    {
        while (transform.position.y < currentY)
        {
            rigid.velocity = Vector3.zero;
            float newY = transform.position.y + 1.0f * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, Mathf.Min(newY, currentY), transform.position.z);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 자식의 트리거에 플레이어가 닿았으면 얼굴에 붙음
    }

    // 일정 시간 동안 플레이어를 잡지 못했으면 부모 스크립트의 NotCatch 코루틴 실행
    // => 현재 플레이어가 부모의 트리거 영역에 들어오면 10초 기다리고 바로 위로 올라감(잡았는지 확인 안하고 있음)
    // 일정 시간 동안 플레이어를 잡지 못했다? => 자식 스크립트의 OnTriggerEnter가 실행되지 않았다
    // 코루틴으로 n초 기다리고 OnTriggerEnter가 실행되었는지 확인
    // => 실행되지 않았으면 위로 올려야 함
    // => 실행되었으면 플레이어의 얼굴에 붙어야 함
}
