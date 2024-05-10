using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditorInternal;
using UnityEngine;
using static EnemyBase;

public class Enemy_Child_KWS : MonoBehaviour
{
    [Range(1f, 10f)]
    public float jumpHeight = 1f;

    /// <summary>
    /// 점프 쿨타임
    /// </summary>
    public float cooltime = 0;

    public float currentY = 9;

    public float groundCheckDistance = 1.0f;

    /// <summary>
    /// 땅의 레이어마스크
    /// </summary>
    public LayerMask groundLayer;

    /// <summary>
    /// 부모 오브젝트
    /// </summary>
    Enemy enemyParent;

    /// <summary>
    /// 애니메이터
    /// </summary>
    Animator animator;

    Rigidbody rigid;



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
        rigid.useGravity = false;       // 중력 off
        animator.SetTrigger("Idle");    // 중력 off 상태는 Idle 상태
    }

    private void GravityOn()
    {
        Debug.Log("GravityOn 실행");
        rigid.useGravity = true;        // 중력 on
        animator.SetTrigger("Walk");    // 중력이 on 되었을 때는 플레이어를 추적하고 있는 상태
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
        if (IsGrounded())   // 땅이면
        {
            cooltime += Time.deltaTime;     // 쿨타임 누적
        }
    }

    private void FixedUpdate()
    {
        //samePosition();
    }

    public void Jump()
    {
        if (cooltime > 0.5f)        // 쿨타임이 0.5초 이상이면
        {
            Debug.Log("점프 실행");
            cooltime = 0;           // 쿨타임 초기화
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

    /// <summary>
    /// 플레이어를 잡았는지 확인하기 위한 bool 변수
    /// </summary>
    bool isCatch = false;

    /// <summary>
    /// 정해진 시간이 지났는지 확인하기 위한 bool 변수
    /// </summary>
    bool timerFinished = false;

    private void OnTriggerEnter(Collider other)
    {
        // 자식의 트리거에 플레이어가 닿았으면 얼굴에 붙음
        if (other.CompareTag("Player"))
        {
            isCatch = true;
            CheckChatch();
        }
    }

    void CheckChatch()
    {
        if (isCatch)
        {
            // 플레이어를 잡았다
            Debug.Log("OnTriggerEnter가 활성화");
            Debug.Log("플레이어를 잡았다");
            StopCoroutine(Timer()); // OnTriggerEnter가 발생하면 타이머 중단
            // 플레이어를 잡으면 아예 타이머를 중단 시켜서 플레이어가 빠져나오면 다시 코루틴이 실행안됨
        }
        else if(timerFinished)
        {
            // 플레이어를 못잡았다
            Debug.Log("10초가 지났지만 플레이어를 못잡았다");
            enemyParent.NoPath();
            enemyParent.StateStop();
        }
    }

    /// <summary>
    /// 10초 동안 기다린 후
    /// </summary>
    /// <returns></returns>
    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(10); // 10초 동안 대기
        timerFinished = true;
        CheckChatch();
    }
}

// 일정 시간 동안 플레이어를 잡지 못했으면 부모 스크립트의 NotCatch 코루틴 실행
// => 현재 플레이어가 부모의 트리거 영역에 들어오면 10초 기다리고 바로 위로 올라감(잡았는지 확인 안하고 있음)
// 일정 시간 동안 플레이어를 잡지 못했다? => 자식 스크립트의 OnTriggerEnter가 실행되지 않았다
// 코루틴으로 n초 기다리고 OnTriggerEnter가 실행되었는지 확인
// => 실행되지 않았으면 위로 올려야 함
// => 실행되었으면 플레이어의 얼굴에 붙어야 함

/*
*참고할 것
using UnityEngine;
using System.Collections;

public class ExampleScript : MonoBehaviour
{
    private bool triggered = false;
    private bool timerFinished = false;

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(10); // 10초 동안 대기
        timerFinished = true;
        CheckTriggerAndTimer();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("YourTag")) // 필요한 경우 태그를 변경하세요
        {
            triggered = true;
            CheckTriggerAndTimer();
        }
    }

    void CheckTriggerAndTimer()
    {
        if (triggered)
        {
            Debug.Log("OnTriggerEnter가 활성화되었습니다.");
            StopCoroutine(Timer()); // OnTriggerEnter가 발생하면 타이머 중단
        }
        else if (timerFinished)
        {
            Debug.Log("10초가 경과했지만 OnTriggerEnter가 활성화되지 않았습니다.");
        }
    }

    void Start()
    {
        StartCoroutine(Timer());
    }
}

*/

