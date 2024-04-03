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

    public Difficulty difficulty = Difficulty.D;

    public Transform Prototype;

    /// <summary>
    /// 사용할 모듈들 인스펙터에서 넣어두기
    /// </summary>
    List<Modul> moduls;

    public List<Modul> Moduls
    {
        get
        {
            if (moduls == null)
            {
                moduls = new List<Modul>();
                moduls.AddRange(Prototype.GetComponentsInChildren<Modul>());
            }
            return moduls;
        }
    }

    /// <summary>
    /// 시작 모듈
    /// </summary>
    public Modul startModul;

    /// <summary>
    /// 종료시 열려있는 모든 커넥터를 막을 문
    /// </summary>
    public Modul endModul;

    /// <summary>
    /// 던전생성 시작 지점
    /// </summary>
    public GenerationPointNav pointNav;

    /// <summary>
    /// 던전 생성 시작 지점 트랜스폼 컴포넌트
    /// </summary>
    Transform generationStartPoint => pointNav.transform;

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

    /// <summary>
    /// 디버그용 모듈순서 검출기
    /// </summary>
    int index = 0;

    /// <summary>
    /// 아이템 생성 포인트 리스트
    /// </summary>
    List<ItemSpawnPoint> itemSpawnPoints = new List<ItemSpawnPoint>();

    /// <summary>
    /// 스폰될 아이템 개수
    /// </summary>
    uint itemSpawnCount = 0;

    /// <summary>
    /// 스폰될 아이템 개수 프로퍼티 (한번 값이 들어오면 변경되지 않음)
    /// </summary>
    public uint ItemSpawnCount
    {
        get
        {
            if(itemSpawnCount == 0)
            {
                itemSpawnCount = (uint)(Random.Range(itemSpawnMinCount, itemSpawnMaxCount) * DifficultyCorrection);
            }
            return itemSpawnCount;
        }

    }

    /// <summary>
    /// 아이템 랜덤스폰 최대치
    /// </summary>
    float itemSpawnMaxCount = 14;

    /// <summary>
    /// 아이템 랜덤스폰 최소치
    /// </summary>
    float itemSpawnMinCount = 10;

    /// <summary>
    /// 현재 난이도에 따른 보정치
    /// </summary>
    float difficultyCorrection = 1;

    /// <summary>
    /// 아이템 최종소환 개수에 곱해서 개수 조정용 프로퍼티
    /// </summary>
    public float DifficultyCorrection
    {
        get
        {
            switch (difficulty)
            {
                case Difficulty.D:
                   return 1;
           
                case Difficulty.C:
                    return  1.2f;
             
                case Difficulty.B:
                    return 1.5f;
          
                case Difficulty.A:
                    return  1.8f;
       
                case Difficulty.S:
                    return 2;
                default: return difficultyCorrection;
            }
        }
    }

    private void Start()
    {
        if (randomSeed > 0)
        {
            Random.InitState(randomSeed);
        }

        // 1번만 소환되는 모듈만 따로 빼두기
        FindUnique();

        // GenerationPoint자식으로 랜덤맵생성
        DungeonGeneration();

        // 아이템 스폰 코드 작성
        ItemGeneration();

    }

    /// <summary>
    /// 아이템 스픈포인트에 아이템을 소환한다. 
    /// </summary>
    private void ItemGeneration()
    {
        // 아이템 스폰포인트 찾기
        // itemSpawnPoints 는 DungeonGeneration 에서 넣어주고 있음
        Debug.Log($"아이템의 소환 개수 :{ItemSpawnCount}");
        //ItemSpawnCount 만큼 itemSpawnPoints에 랜덤위치에 랜덤스크럽 소환 
        // 소환한 위치에 포인트는 리스트에서 삭제
        foreach (ItemSpawnPoint p in itemSpawnPoints)
        {
            // 포인트들 중에서 아이템 최대 선택개수에 맞춰서 생성
        }
        // 아이템 랜덤선택 ItemType에서 Scrap항목만 생성
        //

    }

    /// <summary>
    /// 맵 생성 시작
    /// existConnectors는 이미 깔려 있는 모듈들의 커넥터 모음
    /// newConnectors는 이제 깔릴 모듈들의 커넥터 모음 나중에 existConnectors에 다시 넣어서 반복해 생성
    /// </summary>
    void DungeonGeneration()
    {
        Modul Start = Instantiate(startModul, generationStartPoint);   // 맵 시작 지점 스폰

        List<ModulConnector> existConnectors = new List<ModulConnector>(Start.Connectors); // 시작지점의 연결지점 가져오기

        for (int generation = 0; generation < generationCount; generation++)    // 반복 재생 횟수
        {
            List<ModulConnector> newConnectors = new List<ModulConnector>();    // 생성된모듈들의 연결자 리스트
            for (int exist = 0; exist < existConnectors.Count; exist++)
            {
                // 어쨋든 다음꺼 연결해서 붙임
                Modul newModul = RandomSelectModul();   // 랜덤한 모듈 가져오기
                Modul currentModul = MatchConntectors(existConnectors[exist], newModul); // 모듈을 현재 커넥터에 연결

                itemSpawnPoints.AddRange(currentModul.GetComponentsInChildren<ItemSpawnPoint>());   //아이템 스폰포인터를 목록 넣기

                //newConnectors.AddRange(newModul.Connectors.Where(e => e != 현재 이미 연결된 커넥터));
                if (currentModul != null)
                {
                    newConnectors.AddRange(AddValuesWithoutDuplicates(existConnectors[exist], currentModul));

                }
                // 붙힐때 충돌 감지 되면 endmodul붙힘

            }
            existConnectors = newConnectors; // 현재 깔려있는 커넥터들 갱신

        }
        // existConnectors에 endmodul 연결함
        foreach (ModulConnector connector in existConnectors)    // 마무리 빈 커넥터의 입구 막기
        {
            MatchConntectors(connector, endModul);  // 하나는 비상탈출구로 만들어야함,
            // 첫번째connector중 아무나 한개는 비상 탈출구
        }

        pointNav.CompliteGenerationDungeon();   // 던전 생성 완료후 네비메시를 깔게 함
    }

    /// <summary>
    /// 맵생성시 유니크 모듈만 따로 빼서 소환해둠
    /// </summary>
    private void FindUnique()
    {
        uniqueModuls = new List<Modul>(Moduls.Count);
        for (int i = 0; i < Moduls.Count; i++)// 모듈에서 유니크모듈만 빼서 넣어두기
        {
            if (Moduls[i].thisUnique)
            {
                uniqueModuls.Add(Moduls[i]);
                Moduls.RemoveAt(i);
            }
        }
    }



    /// <summary>
    /// 연결될 위치와 새 모듈을 입력하면 자동으로 연결하고 배치하는 함수
    /// </summary>
    /// <param name="oldConnector">연결할 커넥터</param>
    /// <param name="newModul">새로 들어갈 모듈</param>
    /// <returns>새로 생성되어 연결된 모듈</returns>
    private Modul MatchConntectors(ModulConnector oldConnector, Modul newModul)
    {
        Modul currentModul = Instantiate(newModul, generationStartPoint);
        currentModul.name = $"{index}째 모듈";

        ModulConnector newConnector; // 연결될 커넥터
        newConnector = currentModul.Connectors[Random.Range(1, 100) % currentModul.ConnectorsCount];    // 새로 만든 모듈의 연결할 커넥터
        newConnector.name = $"{index}째 연결자";
        index++;
        currentModul.transform.position = oldConnector.transform.position;


        float angle = Vector3.SignedAngle(newConnector.transform.forward, -oldConnector.transform.forward, Vector3.up);
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        currentModul.transform.rotation = rotation;

        // 회전이 정상 작동하면 newConnector좌표를 oldConnector좌표로 이동시키는 포지션 값을 구하고 currentModul의 위치를 그만큼 이동
        Vector3 m = oldConnector.transform.position - newConnector.transform.position;
        currentModul.transform.position += m;

        bool overlapping = CheckOverlapping(currentModul);
        if (overlapping)
        {
            Destroy(currentModul.gameObject); // 겹치는 모듈이 있으면 새로 생성한 모듈을 파괴하고 종료
            Modul modul = oldConnector.transform.parent.gameObject.GetComponent<Modul>();   // 연결자 다시 돌려주기

            return modul;
        }

        // ㅁ생김새로 랜덤하게 나오면 조기 종료 해버림
        // 맵이 곂치면 망함

        // 모듈 연결할때 새로 생성한 모듈 커넥터에 있는 문은 삭제한다.
        oldConnector.transform.GetChild(1).gameObject.SetActive(false);
        oldConnector.transform.GetChild(0).gameObject.SetActive(false);

        return currentModul;
    }

    /// <summary>
    /// 현재 새로 생성된 모듈이 있는 위치에 기존 맵이 있는지 아닌지 판별하는 함수
    /// </summary>
    /// <param name="newModul"></param>
    /// <returns>true면 겹치는 상태 false면 정상 생성</returns>
    private bool CheckOverlapping(Modul newModul)
    {
        Collider newmoduleCollider = newModul.GetComponent<Collider>();
        if (newmoduleCollider == null)
        {
            //Debug.LogWarning(newModul.name);
            return false; // 콜라이더가 없으면 겹침을 판단하지 않고 바로 false 반환
        }

        Modul[] moduls = pointNav.GetComponentsInChildren<Modul>(); // 이미 깔려있는 모듈 가져오기
        foreach (Modul modul in moduls)
        {
            if (modul == newModul)  // 자기자신 제외
            {
                continue;
            }
            Collider moduleCollider = modul.GetComponent<Collider>();
            if (Physics.ComputePenetration(newmoduleCollider, newmoduleCollider.transform.position, newmoduleCollider.transform.rotation,
                                           moduleCollider, moduleCollider.transform.position, moduleCollider.transform.rotation,
                                           out _, out _))
            {
                Debug.Log(newModul.name);
                return true; // 겹치는 모듈이 존재함
            }
        }

        return false; // 겹치는 모듈이 없음
    }

    /// <summary>
    /// 연결될 모듈을 랜덤으로 꺼내서 정해줌 moduls, uniqueModuls
    /// moduls 90%, uniqueModuls 10%
    /// uniqueModuls이 이미 소환되면 소환에서 제외
    /// </summary>
    private Modul RandomSelectModul()
    {
        Modul selectModul;
        int randomIndex = Random.Range(0, Moduls.Count);
        selectModul = Moduls[randomIndex];


        float num = Random.value;
        if (num < 0.05f)// 5% 유니크맵
        {
            if (uniqueModuls.Count > 0)
            {
                int randomUniqueIndex = Random.Range(1, 100) % uniqueModuls.Count;
                selectModul = uniqueModuls[randomUniqueIndex];  // 인덱스 아웃
                uniqueModuls.RemoveAt(randomUniqueIndex);
            }
        }
        return selectModul;
    }

    /// <summary>
    /// 새로 연결된 모듈에서 이미 겹쳐진 커넥터는 연결될 커넥터에서 제외 시키는 함수
    /// </summary>
    /// <param name="existConnectors">이미 연결중인 커넥터</param>
    /// <param name="modul">연결한 모듈</param>
    /// <returns>새로 연결될 커넥터들</returns>
    private List<ModulConnector> AddValuesWithoutDuplicates(ModulConnector existConnectors, Modul modul)
    {
        List<ModulConnector> newConnecters = new List<ModulConnector>(modul.ConnectorsCount - 1);

        foreach (ModulConnector connecter in modul.Connectors)
        {
            if (connecter.transform.position != existConnectors.transform.position)
            {
                newConnecters.Add(connecter);
            }
        }


        return newConnecters;
    }
}
