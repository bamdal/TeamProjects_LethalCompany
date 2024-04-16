using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : WeaponBase, IEquipable
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

    TestInputActions inputActions;

    private void Awake()
    {
        explosionRadius = 5.0f;
    }

    public void Equip()
    {
        throw new System.NotImplementedException();
    }

    public void Use()
    {
        StopAllCoroutines();
        StartCoroutine(Bomb());
    }

    IEnumerator Bomb()
    {
        Debug.Log("발사");
        yield return new WaitForSeconds(delay);

        Collider[] collders = Physics.OverlapSphere(transform.position, explosionRadius, LayerMask.GetMask("Ememy"));
        IHealth[] enemies;
        int index = 0;
        foreach (Collider coll in collders)
        {
            Debug.Log("적 충돌");
            enemies = new IHealth[collders.Length];
            if(coll.GetComponent<IHealth>() != null)
            {
                enemies[index] = coll.GetComponent<IHealth>();
                index++;
            }
        }

        yield return new WaitForSeconds(delay);
        Debug.Log("오브젝트 비활성화");
        
        gameObject.SetActive(false);
        // IHealth를 통해 데미지
    }
}
