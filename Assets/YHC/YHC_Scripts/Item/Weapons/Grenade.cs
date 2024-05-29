using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grenade : WeaponBase, IEquipable, IBattler, IItemDataBase
{

    ItemDB grenadeData;
    Transform effectPrefab;

    int stunnedDuration = 15;

    /// <summary>
    /// 폭발 딜레이
    /// </summary>
    float delay = 3.0f;
    
    /// <summary>
    /// 폭발 데미지 반경
    /// </summary>
    public float explosionRadius = 5.0f;

    float throwWegiht = 10.0f;

    PlayerInput playerInput;    // 델리게이트 연결위한 필요한 컴포넌트

    Rigidbody rigid;

    TestInputActions inputActions;

    public float Hp { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void Start()
    {
        explosionRadius = 5.0f;
        rigid = GetComponent<Rigidbody>();
        playerInput = GameManager.Instance.Player.GetComponent<PlayerInput>(); // 델리게이트 연결위한 필요한 컴포넌트
        rigid.isKinematic = true;
        grenadeData = GameManager.Instance.ItemData.GetItemDB(ItemCode.Grenade);
        effectPrefab = transform.GetChild(2);
        effectPrefab.gameObject.SetActive(false);
    }

    public void Equip()
    {

    }

    public void Use()
    {
        StopAllCoroutines();
        Debug.Log("사용");
        rigid.isKinematic = false;
        rigid.AddForce(transform.forward * throwWegiht, ForceMode.VelocityChange);
        playerInput.onItemDrop?.Invoke(); //플레이어 onItemDrop 호출하는 델리게이트
        StartCoroutine(Bomb());
    }

    IEnumerator Bomb()
    {
        Debug.Log("발사");
        yield return new WaitForSeconds(delay);

        Collider[] collders = Physics.OverlapSphere(transform.position, explosionRadius, LayerMask.GetMask("Enemy"));
        EnemyBase[] enemies;
        IBattler[] battlers;
        int index = 0;
        foreach (Collider coll in collders)
        {
            Debug.Log("적 충돌");
            enemies = new EnemyBase[collders.Length];
            battlers = new IBattler[collders.Length];
            if(coll.GetComponent<IBattler>() != null)
            {
                enemies[index] = coll.GetComponent<EnemyBase>();
                battlers[index] = coll.GetComponent<IBattler>();
                index++;
            }

            foreach(EnemyBase enemy in enemies)
            {
                StartCoroutine(EnemyStunned(stunnedDuration, enemy.transform.position));
                NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
                enemy.onDebuffAttack?.Invoke(agent, stunnedDuration);
            }
        }

        yield return new WaitForSeconds(delay);
        Debug.Log("오브젝트 비활성화");

    }

    IEnumerator EnemyStunned(int stunnedTime, Vector3 enemyPos)
    {
        effectPrefab.gameObject.SetActive(true);
        effectPrefab.transform.position = transform.position;
        yield return new WaitForSeconds(stunnedTime);
        effectPrefab.gameObject.SetActive(false);
    }

    public void Attack(IBattler target)
    {

    }

    public void Defense(float attackPower)
    {

    }

    public ItemDB GetItemDB()
    {
        return grenadeData;
    }
}
