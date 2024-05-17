using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditorInternal;
using UnityEngine;
using static EnemyBase;
using static UnityEngine.EventSystems.EventTrigger;

public class Enemy_Child_KWS : MonoBehaviour
{
    /// <summary>
    /// 점프 높이
    /// </summary>
    [Range(1f, 10f)]
    public float jumpHeight = 1f;

    /// <summary>
    /// 점프 쿨타임
    /// </summary>
    public float cooltime = 0;

    /// <summary>
    /// stop 상태일때 올라갈 높이
    /// </summary>
    public float currentY = 9;

    /// <summary>
    /// 땅인지 확인할 거리
    /// </summary>
    public float groundCheckDistance = 1.0f;

    /// <summary>
    /// 땅의 레이어마스크
    /// </summary>
    public LayerMask groundLayer;

    /// <summary>
    /// 부모 오브젝트(agent로 움직임)
    /// </summary>
    Enemy_Spider enemyParent;

    /// <summary>
    /// 애니메이터
    /// </summary>
    Animator animator;

    Rigidbody rigid;

    /// <summary>
    /// 플레이어를 잡았는지 확인하기 위한 bool 변수
    /// </summary>
    bool isCatch = false;

    /// <summary>
    /// isCatch를 참조하기 위한 프로퍼티
    /// </summary>
    public bool IsCatch => isCatch;

    /// <summary>
    /// 정해진 시간이 지났는지 확인하기 위한 bool 변수
    /// </summary>
    bool timerFinished = false;

    //Transform spiderPosition;

    /// <summary>
    /// 애니메이션을 실행할 트리거의 해쉬
    /// </summary>
    int idleHash = Animator.StringToHash("Idle");
    int walkHash = Animator.StringToHash("Walk");
    int attackHash = Animator.StringToHash("Attack");
    int dieHash = Animator.StringToHash("Die");

    // 콜라이더
    BoxCollider box;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.constraints = RigidbodyConstraints.FreezeRotation;        // 물체에 충돌했을 때 회전이 틀어지는 것 방지
        animator = GetComponent<Animator>();

        box = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        enemyParent = transform.parent.GetComponent<Enemy_Spider>();
        enemyParent.onRaise += GravityOff;
        enemyParent.onLower += GravityOn;
        enemyParent.hpChange += HPChange;
        enemyParent.onPlayerDie += OnPlayerDie;
        //enemyParent.onChase += samePosition;

        //Player player = GameManager.Instance.Player;
        //spiderPosition = player.transform.GetChild(5);
    }

    /// <summary>
    /// 적이 죽었는지 확인하기 위한 변수 true면 죽었다, false면 안죽었다
    /// </summary>
    bool die = false;

    /// <summary>
    /// HP가 변경되었을 때 0이면 실행될 함수
    /// </summary>
    /// <param name="HP"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void HPChange(float HP)
    {
        if(HP < 1)
        {
            StopCoroutine(DisableCollider());
            Player player = GameManager.Instance.Player;
            Quaternion playerRotation = player.transform.rotation;

            die = true;
            enemyParent.Hp = 0;
            animator.ResetTrigger(idleHash);
            animator.ResetTrigger(walkHash);
            animator.ResetTrigger(attackHash);

            GravityOn();
            // 부모를 옮기기
            this.gameObject.transform.SetParent(null);

            // 죽음 상태의 로테이션으로 변경
            this.gameObject.transform.rotation = playerRotation;

            Debug.Log("다이 애니메이션 실행");
            animator.SetTrigger(dieHash);

            //enemyParent.NoPath();
            enemyParent.StateDie();

            StartCoroutine(DisableCollider());
        }
    }

    /// <summary>
    /// 콜라이더를 끄고 게임 오브젝트를 비활성화하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(3.0f);      // 3초 기다리고
        
        box.enabled = false;                        // 콜라이더 끄기

        yield return new WaitForSeconds(3.0f);      // 3초 기다리고

        this.gameObject.SetActive(false);           // 이 게임 오브젝트 비활성화
        enemyParent.gameObject.SetActive(false);    // agent를 가지고 있는 부모오브젝트 비활성화
    }

    /// <summary>
    /// 중력을 끄기 위한 함수
    /// </summary>
    private void GravityOff()
    {
        //Debug.Log("GravityOff 실행");
        rigid.useGravity = false;           // 중력 off
        animator.ResetTrigger(walkHash);    // 쌓여있던 트리거 초기화
        animator.ResetTrigger(dieHash);
        animator.SetTrigger(idleHash);      // 중력 off 상태는 Idle 상태
    }

    /// <summary>
    /// 중력을 켜기 위한 함수
    /// </summary>
    private void GravityOn()
    {
        //Debug.Log("GravityOn 실행");
        rigid.useGravity = true;            // 중력 on
        animator.ResetTrigger(idleHash);    // 쌓여있던 트리거 초기화
        animator.ResetTrigger(dieHash);
        animator.SetTrigger(walkHash);      // 중력이 on 되었을 때는 플레이어를 추적하고 있는 상태
    }

    /// <summary>
    /// 점프하면서 부모 오브젝트의 위치와 틀어지는 것을 막기 위한 함수
    /// </summary>
    private void samePosition()
    {
        // 부모 오브젝트의 위치 정보 가져오기
        Vector3 parentPosition = transform.parent.position;

        // 부모 오브젝트의 x와 z 값을 자식 오브젝트에게 적용
        transform.position = new Vector3(parentPosition.x, transform.position.y, parentPosition.z);
    }

    private void Update()
    {
        /*if (IsGrounded())   // 땅이면
        {
            cooltime += Time.deltaTime;     // 쿨타임 누적
        }

        if (isCatch)        // 플레이어를 잡았을 경우
        {
            //Player player = GameManager.Instance.Player;
            //spiderPosition = player.transform.GetChild(5);

            Player player = GameManager.Instance.Player;                                    // 플레이어 찾기
            spiderPosition = player.transform.GetChild(3);                                  // 플레이어의 5번째 자식 spiderPosition 찾기

            // 이 오브젝트를 플레이어의 자식으로 변경
            this.gameObject.transform.SetParent(player.transform);
            this.gameObject.transform.localPosition = new Vector3(0, 1, 1);
            
            Quaternion playerRotation = player.transform.rotation;                          // 플레이어의 회전
            Quaternion additionalRotation = Quaternion.Euler(90f, 0f, 0f);                  // x축으로 90도 회전
            Quaternion finalRotation = playerRotation * additionalRotation;                 // 플레이어의 회전에 x축 90도 더하기
            this.gameObject.transform.rotation = finalRotation;                             // 이 오브젝트의 회전 설정

            *//*Quaternion playerRotation = player.gameObject.transform.rotation;               // 플레이어의 회전
            //playerRotation.eulerAngles.z
            // 플레이어의 회전 + x축으로 90도 회전
            Quaternion adjustedRotation = Quaternion.Euler(90.0f, playerRotation.eulerAngles.y, playerRotation.eulerAngles.z);
            this.gameObject.transform.rotation = adjustedRotation;                          // 이 오브젝트의 회전을 플레이어 + x축 90도
            this.gameObject.transform.position = spiderPosition.transform.position + new Vector3(0, -0.5f, 1.0f);         // 이 오브젝트의 위치를 spiderPosition와 같게
            //this.gameObject.transform.rotation = player.gameObject.transform.rotation;      // 이 오브젝트의 회전을 플레이어와 같게*//*
            enemyParent.StateAttack();
            
            rigid.velocity = Vector3.zero;        // 가해지던 힘 제거
            GravityOff();                         // 중력 끄기

            animator.ResetTrigger(idleHash);      // 쌓여있던 트리거 초기화
            animator.ResetTrigger(walkHash);      // 쌓여있던 트리거 초기화
            animator.SetTrigger(attackHash);      // Attack 애니메이션 실행
        }*/
    }

    private void FixedUpdate()
    {
        if (IsGrounded() && !die)   // 땅이면
        {
            cooltime += Time.deltaTime;     // 쿨타임 누적
        }

        if (isCatch && !die)        // 플레이어를 잡았을 경우
        {
            //Player player = GameManager.Instance.Player;
            //spiderPosition = player.transform.GetChild(5);

            Player player = GameManager.Instance.Player;                                    // 플레이어 찾기
            //spiderPosition = player.transform.GetChild(3);                                  // 플레이어의 5번째 자식 spiderPosition 찾기

            // 이 오브젝트를 플레이어의 자식으로 변경
            this.gameObject.transform.SetParent(player.transform);
            this.gameObject.transform.localPosition = new Vector3(0, 1, 1);

            Quaternion playerRotation = player.transform.rotation;                          // 플레이어의 회전
            Quaternion additionalRotation = Quaternion.Euler(90f, 0f, 0f);                  // x축으로 90도 회전
            Quaternion finalRotation = playerRotation * additionalRotation;                 // 플레이어의 회전에 x축 90도 더하기
            this.gameObject.transform.rotation = finalRotation;                             // 이 오브젝트의 회전 설정

            /*Quaternion playerRotation = player.gameObject.transform.rotation;               // 플레이어의 회전
            //playerRotation.eulerAngles.z
            // 플레이어의 회전 + x축으로 90도 회전
            Quaternion adjustedRotation = Quaternion.Euler(90.0f, playerRotation.eulerAngles.y, playerRotation.eulerAngles.z);
            this.gameObject.transform.rotation = adjustedRotation;                          // 이 오브젝트의 회전을 플레이어 + x축 90도
            this.gameObject.transform.position = spiderPosition.transform.position + new Vector3(0, -0.5f, 1.0f);         // 이 오브젝트의 위치를 spiderPosition와 같게
            //this.gameObject.transform.rotation = player.gameObject.transform.rotation;      // 이 오브젝트의 회전을 플레이어와 같게*/

            enemyParent.StateAttack();

            rigid.velocity = Vector3.zero;        // 가해지던 힘 제거
            GravityOff();                         // 중력 끄기

            animator.ResetTrigger(idleHash);      // 쌓여있던 트리거 초기화
            animator.ResetTrigger(walkHash);      // 쌓여있던 트리거 초기화
            animator.ResetTrigger(dieHash);
            animator.SetTrigger(attackHash);      // Attack 애니메이션 실행
        }
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

            samePosition();
        }
    }

    /// <summary>
    /// 바닥인지 확인하기 위한 bool 변수
    /// </summary>
    /// <returns>바닥이면 true, 바닥이 아니면 false</returns>
    public bool IsGrounded()
    {
        // 캐릭터의 아래에 레이캐스트를 쏴서 바닥에 닿았는지 확인
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, layerMask: groundLayer);
    }

    /// <summary>
    /// 적을 위로 올리는 코루틴
    /// </summary>
    /// <returns></returns>
    public IEnumerator RaiseTrap()
    {
        while (transform.position.y < currentY)
        {
            box.enabled = false;
            rigid.velocity = Vector3.zero;
            float newY = transform.position.y + 1.0f * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, Mathf.Min(newY, currentY), transform.position.z);
            yield return null;
        }
        box.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 자식의 트리거에 플레이어가 닿았으면 얼굴에 붙음
        if (other.CompareTag("Player"))
        {
            isCatch = true;
            CheckChatch();
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        // 자식의 트리거에 플레이어가 닿았으면 얼굴에 붙음
        if (CompareTag("Player"))
        {
            isCatch = true;
            CheckChatch();
        }
    }*/

    /// <summary>
    /// 플레이어를 잡았는지 확인하는 함수
    /// </summary>
    void CheckChatch()
    {
        if (isCatch && !die)
        {
            // 플레이어를 잡았다
            //Debug.Log("OnTriggerEnter가 활성화");
            //Debug.Log("플레이어를 잡았다");
            StopCoroutine(Timer()); // OnTriggerEnter가 발생하면 타이머 중단

            //Debug.Log("위치 조정 실행");

            //enemyParent.NoPath();
            // 플레이어를 잡으면 아예 타이머를 중단 시켜서 플레이어가 빠져나오면 다시 코루틴이 실행안됨
            // 죽기 전까지 안떨어지므로 상관X
        }
        else if(timerFinished && !die)
        {
            // 플레이어를 못잡았다
            Debug.Log("10초가 지났지만 플레이어를 못잡았다");
            samePosition();
            enemyParent.NoPath();
            enemyParent.StateStop();
            timerFinished = false;
        }
    }

    /// <summary>
    /// 부모 트리거에 닿았으면 10초 동안 기다린 후 실행될 코루틴
    /// </summary>
    /// <returns></returns>
    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(10); // 10초 동안 대기
        timerFinished = true;
        CheckChatch();
    }

    /// <summary>
    /// 플레이어가 죽어서 플레이어의 자식에서 떨어지기 위한 함수
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void OnPlayerDie()
    {
        // 게임 오브젝트 삭제
        Destroy(this.gameObject);
    }
}

// 일정 시간 동안 플레이어를 잡지 못했으면 부모 스크립트의 NotCatch 코루틴 실행
// => 현재 플레이어가 부모의 트리거 영역에 들어오면 10초 기다리고 바로 위로 올라감(잡았는지 확인 안하고 있음)
// 일정 시간 동안 플레이어를 잡지 못했다? => 자식 스크립트의 OnTriggerEnter가 실행되지 않았다
// 코루틴으로 n초 기다리고 OnTriggerEnter가 실행되었는지 확인
// => 실행되지 않았으면 위로 올려야 함
// => 실행되었으면 플레이어의 얼굴에 붙어야 함