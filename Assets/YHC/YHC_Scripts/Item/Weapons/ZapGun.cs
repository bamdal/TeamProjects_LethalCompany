using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZapGun : WeaponBase, IEquipable
{
    /// <summary>
    /// 총이 릴리즈중인지 아닌지 확인하는 변수
    /// </summary>
    bool isRelease = false;

    /// <summary>
    /// 스캔한 장소에 적이 있는지 없는지 확인하는 변수, true면 적이 있다, false면 적이 없다.
    /// </summary>
    bool isTargetOn = false;

    float scanRadius = 5.0f;

    float damage = 10.0f;

    float damageTick = 1.0f;
    float battery = 100.0f;
    
    Transform bullet;
    SphereCollider bulletCollider;
    Rigidbody bulletRigid;

    EnemyBase targetEnemy;

    private void Awake()
    {
        bullet = transform.GetChild(3);
        bulletCollider = bullet.GetComponent<SphereCollider>();
        bulletRigid = bullet.GetComponent<Rigidbody>();

        bulletRigid.useGravity = false;
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        ObjectScan();
    }

    public void Equip()
    {

    }

    public void Use()
    {
        Shot();
    }

    void ObjectScan()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, scanRadius);
        targetEnemy = null;

        foreach(Collider collider in colliders)
        {
            targetEnemy = collider.GetComponent<EnemyBase>();
        }

        if(targetEnemy != null)
        {
            isTargetOn = true;
        }
        else
        {
            isTargetOn = false;
        }
    }

    void Shot()
    {
        bulletRigid.AddForce(transform.forward * 5.0f, ForceMode.Impulse);
    }

    void Release()
    {
        damageTick -= Time.deltaTime;

        if (isTargetOn && targetEnemy != null)
        {
            IBattler attackTarget = targetEnemy.GetComponent<IBattler>();
            if(damageTick < 0.0f && battery > 0.0f)
            {
                attackTarget.Defense(damage);
                damageTick = 1.0f;
                battery -= 20.0f;
                targetEnemy.onDebuffAttack?.Invoke();
            }
        }
    }
}
