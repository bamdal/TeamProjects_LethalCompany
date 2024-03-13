using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    // 생성될때 스타트 포인트 모듈 가지기
    // 모듈들이 생성될수 있는 조각들 전부 가져오기
    // 묘듈 커넥터 를 보면서 계속 연결해주기
    // 모듈의 충돌이 발생할시 다른 모듈로 교체
    // 초기에 정해놓은 횟수대로 반복
    // 핵심 소환 장소모듈(thisUnique = true)이 소환되지 않았을 경우 남은 커넥터 위치에 끼워넣기
    // 반복 횟수 마무리후 남은 커넥터문 잠그기

    // 네비메시를 바닥에 깔기 - 던전을 하나로 합치든 그냥 깔든 빈틈생기면 그정도는 미리 지나가게하든 네비메시가 깔릴 위치에 빈 메시 만들든

    // 모듈에 미리 만들어둔 아이템 소환위치에 아이템 랜덤배치하기

    /// <summary>
    /// 사용할 모듈들 인스펙터에서 넣어두기
    /// </summary>
    public List<Modul> moduls;

    /// <summary>
    /// 시작 모듈
    /// </summary>
    public Modul startModul;

    /// <summary>
    /// 종료시 열려있는 모든 커넥터를 막을 문
    /// </summary>
    public Modul endModul;

    /// <summary>
    /// 던전 생성 시작 지점
    /// </summary>
    public Transform generationStartPoint;

    /// <summary>
    /// 반복시행할 맵 생성 횟수
    /// </summary>
    public int generationCount = 5;

    /// <summary>
    /// 랜덤 시드 0 이상 값 쓰면 시드 고정
    /// </summary>
    public int randomSeed = -1;

    /// <summary>
    /// 단 한번만 생성되어야 할 모듈 모음
    /// </summary>
    List<Modul> uniqueModuls;


    private void Awake()
    {
        if (randomSeed < 0)
        {
            Random.InitState(randomSeed);
        }

        FindUnique();

        Generation();
    }

    /// <summary>
    /// 맵 생성 시작
    /// existConnectors는 이미 깔려 있는 모듈들의 커넥터 모음
    /// newConnectors는 이제 깔릴 모듈들의 커넥터 모음 나중에 existConnectors에 다시 넣어서 반복해 생성
    /// </summary>
    void Generation()
    {
        Instantiate(startModul,generationStartPoint);   // 맵 시작 지점 스폰
        
        List<ModulConnector> existConnectors = new List<ModulConnector>(startModul.Connectors); // 시작지점의 연결지점 가져오기

        for (int generation = 0; generation < generationCount; generation++)    // 반복 재생 횟수
        { 
            List<ModulConnector> newConnectors = new List<ModulConnector>();    // 생성된모듈들의 연결자 리스트
            for (int exist = 0; exist < existConnectors.Count; exist++)
            {
                // 어쨋든 다음꺼 연결해서 붙임
                Modul newModul = RandomSelectModul();
                MatchConntectors(existConnectors[exist], newModul);
                
                //newConnectors.AddRange(newModul.Connectors.Where(e => e != 현재 이미 연결된 커넥터));
                newConnectors.Add(AddValuesWithoutDuplicates(newConnectors, existConnectors[exist],newModul));
                // 붙힐때 충돌 감지 되면 endmodul붙힘

            }
            existConnectors = newConnectors; // 현재 깔려있는 커넥터들 갱신

        }
        // existConnectors에 endmodul 연결함

    }

    /// <summary>
    /// 맵생성시 유니크 모듈만 따로 빼서 소환해둠
    /// </summary>
    private void FindUnique()
    {
        uniqueModuls = new List<Modul>(moduls.Count);
        for (int i = 0; i < moduls.Count; i++)// 모듈에서 유니크모듈만 빼서 넣어두기
        {
            if (moduls[i].thisUnique)
            {
                uniqueModuls.Add(moduls[i]);
                moduls.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 연결될 위치와 새 모듈을 입력하면 자동으로 연결하고 배치하는 함수
    /// </summary>
    /// <param name="oldConnector">연결할 커넥터</param>
    /// <param name="newModul">새로 들어갈 모듈</param>
    private void MatchConntectors(ModulConnector oldConnector, Modul newModul)
    {

    }

    /// <summary>
    /// 연결될 모듈을 랜덤으로 꺼내서 정해줌 moduls, uniqueModuls
    /// moduls 90%, uniqueModuls 10%
    /// uniqueModuls이 이미 소환되면 소환에서 제외
    /// </summary>
    private Modul RandomSelectModul()
    {
        Modul selectModul = new Modul();
        int randomIndex = Random.Range(0, moduls.Count - 1);
        selectModul = moduls[randomIndex];


        float num = Random.value;
        if (num < 0.10f)// 10% 유니크맵
        {
            if (uniqueModuls != null)
            {
                int randomUniqueIndex = Random.Range(0, uniqueModuls.Count - 1);
                selectModul = uniqueModuls[randomUniqueIndex];
                uniqueModuls.RemoveAt(randomUniqueIndex);
            }
        }
        return selectModul; 
    }

    /// <summary>
    /// 새로 연결된 모듈에서 이미 겹쳐진 커넥터는 연결될 커넥터에서 제외 시키는 함수
    /// </summary>
    /// <param name="newConnectors">새로 연결되야 하는 커넥터</param>
    /// <param name="existConnectors">이미 연결중인 커넥터</param>
    /// <param name="modul">연결한 모듈</param>
    /// <returns></returns>
    private ModulConnector AddValuesWithoutDuplicates(List<ModulConnector> newConnectors, ModulConnector existConnectors,Modul modul)
    {
        return new ModulConnector();
    }
}
