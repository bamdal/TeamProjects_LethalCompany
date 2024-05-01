using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : WeaponBase, IEquipable, IBattler
{
    /// <summary>
    /// 폭발 데미지
    /// </summary>
    float damage;

    /// <summary>
    /// 폭발 딜레이
    /// </summary>
    float delay = 3.0f;
    
    /// <summary>
    /// 폭발 데미지 반경
    /// </summary>
    public float explosionRadius;

    Rigidbody rigid;

    TestInputActions inputActions;

    public float Hp { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void Awake()
    {
        explosionRadius = 5.0f;
        rigid = GetComponent<Rigidbody>();
    }

    public void Equip()
    {
        throw new System.NotImplementedException();
    }

    public void Use()
    {
        StopAllCoroutines();
        rigid.AddForce(transform.forward, ForceMode.Impulse);
        StartCoroutine(Bomb());
    }

    IEnumerator Bomb()
    {
        Debug.Log("발사");
        yield return new WaitForSeconds(delay);

        Collider[] collders = Physics.OverlapSphere(transform.position, explosionRadius, LayerMask.GetMask("Ememy"));
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

            foreach(IBattler battler in enemies)
            {
                battler.Defense(damage);
            }
            foreach(EnemyBase enemy in enemies)
            {
                enemy.onDebuffAttack?.Invoke();
            }
        }

        yield return new WaitForSeconds(delay);
        Debug.Log("오브젝트 비활성화");
        
        gameObject.SetActive(false);
    }
    public void Attack(IBattler target)
    {

    }

    public void Defense(float attackPower)
    {

    }
}
