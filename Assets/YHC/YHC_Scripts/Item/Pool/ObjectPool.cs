using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : Recycle
{
    public GameObject recycleObject;    

    public int poolBaseCapacity = 8;

    /// <summary>
    /// 풀에 사용 가능한 모든 오브젝트 배열
    /// </summary>
    T[] pool;

    /// <summary>
    /// 풀의 오브젝트들을 관리하는 큐
    /// </summary>
    Queue<T> poolQueue;

    public void Initialize()
    {
        if(pool == null)
        {
            // 풀이 없으면 풀 생성
            pool = new T[poolBaseCapacity];
            poolQueue =  new Queue<T>(poolBaseCapacity);

            GenerateRecycleObjects(0, poolBaseCapacity, pool);      // 풀 제작
        }
        else
        {
            foreach(T item in pool)
            {
                item.gameObject.SetActive(false);   // 만들어진 아이템들 비활성화
            }
        }
    }

    /// <summary>
    /// 풀에서 오브젝트 한개를 리턴하는 함수
    /// </summary>
    /// <param name="position"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public T GetObject(Vector3? position = null, Vector3? angle = null)
    {
        if(poolQueue.Count > 0)
        {
            T newComp = poolQueue.Dequeue();
            newComp.transform.position = position.GetValueOrDefault();  // 파라메터값이 없으면 defalut값 리턴해서 obj의 스폰 위치 지정
            newComp.transform.rotation = Quaternion.Euler(angle.GetValueOrDefault());   // 위와 같음
            
            newComp.gameObject.SetActive(true);     // 오브젝트 활성화
            
            return newComp;
        }
        else
        {
            // 큐에 빈공간 없으면 큐 확장
            PoolExpand();
            return GetObject(position, angle);
        }
    }

    private void PoolExpand()
    {
        int expandCapacity = poolBaseCapacity * 2;  // 2배수 확장
        T[] expandedPool = new T[expandCapacity];   // 확장된 크기로 새 풀 제작

        for(int i = 0; i < poolBaseCapacity; i++)
        {
            // 기존 풀 내용 새 풀로 옮기기
            expandedPool[i] = pool[i];
        }

        // 기존에서 새로 확장된 부분에 게임오브젝트 생성
        GenerateRecycleObjects(poolBaseCapacity, expandCapacity, expandedPool);

        pool = expandedPool;
        poolBaseCapacity = expandCapacity;
    }

    /// <summary>
    /// 오브젝트 풀이 확장될 때 새로운 게임 오브젝트를 제작하는 함수
    /// </summary>
    /// <param name="startIndex">시작 인덱스</param>
    /// <param name="endIndex">마지막 인덱스 + 1(for문에서 사용)</param>
    /// <param name="expandedPool">확장할 풀</param>
    void GenerateRecycleObjects(int startIndex, int endIndex, T[] expandedPool)
    {
        for(int i = startIndex; i < endIndex; i++)
        {
            GameObject obj = Instantiate(recycleObject, transform); // 오브젝트 새로 생성
            obj.name = $"{recycleObject.name}_{i}";     // 새로 만들어진 오브젝트들 이름 변경

            T newObjectComp = obj.GetComponent<T>();    // 새로 만들어진 오브젝트와 같은 오브젝트 찾아서
            newObjectComp.onDisable += () => poolQueue.Enqueue(newObjectComp);       // 오브젝트 큐에 추가

            expandedPool[i] = newObjectComp;
            obj.gameObject.SetActive(false);
        }
    }
}
