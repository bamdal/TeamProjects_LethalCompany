using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
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

    EnemySpawner enemySpawner;

    /// <summary>
    /// 사용할 모듈들 인스펙터에서 넣어두기
    /// </summary>
    public List<Modul> moduls;

    public List<Modul> Moduls
    {
        get
        {
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
    /// enemy 생성 포인트 리스트
    /// </summary>
    List<EnemySpawnPoint> enemySpawnPoints = new List<EnemySpawnPoint>();

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
            if (itemSpawnCount == 0)
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
                    return 1.2f;

                case Difficulty.B:
                    return 1.5f;

                case Difficulty.A:
                    return 1.8f;

                case Difficulty.S:
                    return 2;
                default: return difficultyCorrection;
            }
        }
    }

    /// <summary>
    /// 마무리 단계때 커넥터 정리하는 변수
    /// </summary>
    bool closeConnector = false;

    /// <summary>
    /// 씬생성시 맵생성 완료되었는지 파악하는 변수
    /// </summary>
    bool doMakeComplite = false;




    public void StartGame()
    {
        enemySpawner = GetComponent<EnemySpawner>();
        doMakeComplite = false;
        if (pointNav == null)
        {
            pointNav = FindAnyObjectByType<GenerationPointNav>();
        }
        if (randomSeed > 0)
        {
            Random.InitState(randomSeed);
        }

        // 1번만 소환되는 모듈만 따로 빼두기
        FindUnique();

        // GenerationPoint자식으로 랜덤맵생성
        StartCoroutine( DungeonGeneration());


    }



    /// <summary>
    /// 맵 생성 시작
    /// existConnectors는 이미 깔려 있는 모듈들의 커넥터 모음
    /// newConnectors는 이제 깔릴 모듈들의 커넥터 모음 나중에 existConnectors에 다시 넣어서 반복해 생성
    /// </summary>
    IEnumerator DungeonGeneration()
    {
        index = 0; // 재생성시 초기화용
        Modul Start = Instantiate(startModul, generationStartPoint);   // 맵 시작 지점 스폰

        List<ModulConnector> existConnectors = new List<ModulConnector>(Start.Connectors); // 시작지점의 연결지점 가져오기
        List<ModulConnector> failConnectors = new List<ModulConnector>();   // 실패한 커넥터 가져오기
        

        for (int generation = 0; generation < generationCount; generation++)    // 반복 재생 횟수
        {
            List<ModulConnector> newConnectors = new List<ModulConnector>();    // 생성된모듈들의 연결자 리스트
            for (int exist = 0; exist < existConnectors.Count; exist++)
            {
                // 어쨋든 다음꺼 연결해서 붙임
                Modul newModul = RandomSelectModul();   // 랜덤한 모듈 가져오기
                ModulConnector dc = null;
                Modul currentModul = MatchConntectors(existConnectors[exist], newModul, out dc); // 모듈을 현재 커넥터에 연결
                //yield return new WaitForSeconds(1);
                yield return null;
               
                //newConnectors.AddRange(newModul.Connectors.Where(e => e != 현재 이미 연결된 커넥터));
                if (currentModul != null)
                {
                    itemSpawnPoints.AddRange(currentModul.ItemSpawnPoints);   //아이템 스폰포인터를 목록 넣기
                    enemySpawnPoints.AddRange(currentModul.EnemySpawnPoints);   // 적 스폰포인트를 목록에 넣기
                    newConnectors.AddRange(AddValuesWithoutDuplicates(dc, currentModul));

                }
                else
                {
                    failConnectors.Add(existConnectors[exist]);  // 실패시 커넥터 저장
                }
                // 붙힐때 충돌 감지 되면 endmodul붙힘

            }
            existConnectors = newConnectors; // 현재 깔려있는 커넥터들 갱신

        }
        closeConnector = true;          // 남은 커넥터정리시에는 충돌검사 안하기
        // existConnectors에 endmodul 연결함
        foreach (ModulConnector connector in existConnectors)    // 마무리 빈 커넥터의 입구 막기
        {
            MatchConntectors(connector, endModul,out _);  // 하나는 비상탈출구로 만들어야함,
                                                          // 첫번째connector중 아무나 한개는 비상 탈출구
            //yield return new WaitForSeconds(1);
            yield return null;
        }

        foreach (ModulConnector connector in failConnectors)
        {
            MatchConntectors(connector, endModul, out _);
            yield return null;
        }

        pointNav.CompliteGenerationDungeon();   // 던전 생성 완료후 네비메시를 깔게 함

        ItemGeneration();  // 아이템 스폰 코드 작성
        enemySpawner.OnSpawnEnemy(enemySpawnPoints, difficulty);    // 아이템 스폰 좌표들과 난이도 보내기
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
    private Modul MatchConntectors(ModulConnector oldConnector, Modul newModul, out ModulConnector destroyconnecter)
    {
        Modul currentModul = Instantiate(newModul, generationStartPoint);
        currentModul.name = $"{index}째 모듈";
        currentModul.transform.GetChild(0).name = $"{index}째 모듈";

        ModulConnector newConnector; // 연결될 커넥터
        newConnector = currentModul.Connectors[Random.Range(0, currentModul.ConnectorsCount)];    // 새로 만든 모듈의 연결할 커넥터
        newConnector.name = $"{oldConnector.gameObject.transform.parent.name}과 연결";
        index++;
        currentModul.transform.position = oldConnector.transform.position;

        
        float angle = Vector3.SignedAngle(newConnector.transform.forward, -oldConnector.transform.forward, Vector3.up);
        //float angle = Quaternion.Angle(newConnector.transform.rotation, oldConnector.transform.rotation);
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        currentModul.transform.localRotation *= rotation;

        // 회전이 정상 작동하면 newConnector좌표를 oldConnector좌표로 이동시키는 포지션 값을 구하고 currentModul의 위치를 그만큼 이동
        Vector3 m = oldConnector.transform.position - newConnector.transform.position;
        currentModul.transform.position += m;
        destroyconnecter = newConnector;


        if (!closeConnector && CheckOverlapping(currentModul))
        {
            currentModul.gameObject.SetActive(false); // 겹치는 모듈이 있으면 새로 생성한 모듈을 파괴하고 종료
            oldConnector.UseConnector = false;
            //Modul modul = oldConnector.transform.parent.gameObject.GetComponent<Modul>();   // 연결자 다시 돌려주기
            // 삭제된경우 커넥터가 지워져서 그 부분에 마무리 입구 막기가 제대로 안되는 상황
            return null;
        }

        // ㅁ생김새로 랜덤하게 나오면 조기 종료 해버림
        // 맵이 곂치면 망함

        // 모듈 연결할때 새로 생성한 모듈 커넥터에 있는 문은 삭제한다. TestModul넣으면 에러
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
        //Collider newmoduleCollider = newModul.GetComponent<Collider>();
        //if (newmoduleCollider == null)
        //{
        //    //Debug.LogWarning(newModul.name);
        //    return false; // 콜라이더가 없으면 겹침을 판단하지 않고 바로 false 반환
        //}

        //Modul[] enemyTargetPositions = pointNav.GetComponentsInChildren<Modul>(); // 이미 깔려있는 모듈 가져오기


        // 기존 콜라이더로 비교하기
        // Collider moduleCollider = modul.GetComponent<Collider>();
        //if (Physics.ComputePenetration(newmoduleCollider, newmoduleCollider.transform.position, newmoduleCollider.transform.rotation,
        //                               moduleCollider, moduleCollider.transform.position, moduleCollider.transform.rotation,
        //                               out _, out _))
        //{
        //    Debug.Log(newModul.name);
        //    return true; // 겹치는 모듈이 존재함
        //}


        BoxCollider newmoduleCollider = newModul.GetComponent<BoxCollider>();
        /*        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = newmoduleCollider.transform.position; // 큐브 위치 설정
                cube.transform.localScale = newmoduleCollider.size ; // 큐브 크기 설정
                cube.transform.rotation = newmoduleCollider.transform.rotation;
                cube.GetComponent<Renderer>().material.color = Color.blue; // 큐브 색상 설정*/
                Collider[] moduleCollider = Physics.OverlapBox(newmoduleCollider.transform.position, newmoduleCollider.bounds.extents*0.95f, newmoduleCollider.transform.rotation, LayerMask.GetMask("GenerationMask"));

        
/*        RaycastHit hitInfo;
        Physics.BoxCast(newmoduleCollider.transform.position, newmoduleCollider.size*0.5f, newmoduleCollider.transform.forward, out hitInfo, newmoduleCollider.transform.rotation, 5f, LayerMask.GetMask("GenerationMask"));

        if (hitInfo.transform != null)
        {
            // 충돌한 경우에 수행할 작업
            Debug.Log(hitInfo.transform.gameObject.name);
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = hitInfo.point;
            cube.GetComponent<Renderer>().material.color = Color.blue;
            return true; // 겹치는 모듈이 존재함
        }*/
        foreach (Collider collider in moduleCollider)
            {
                if (collider.gameObject != newModul.gameObject) // 자기 자신은 제외
                {
                    Debug.Log(collider.name);
                    return true; // 겹치는 모듈이 존재함
                }
            }

       // Physics.BoxCast(newmoduleCollider.transform.position, newmoduleCollider.bounds.extents,, newmoduleCollider.transform.rotation)
               

        // 함수 파라미터 : 현재 위치, Box의 절반 사이즈, Ray의 방향, RaycastHit 결과, Box의 회전값, BoxCast를 진행할 거리
        return false;
    }



    /// <summary>
    /// 연결될 모듈을 랜덤으로 꺼내서 정해줌 enemyTargetPositions, uniqueModuls
    /// enemyTargetPositions 90%, uniqueModuls 10%
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
                uniqueModuls.Remove(selectModul);
                //uniqueModuls.RemoveAt(randomUniqueIndex);
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
    private List<ModulConnector> AddValuesWithoutDuplicates(ModulConnector DConnectors, Modul modul)
    {
        List<ModulConnector> newConnecters = new List<ModulConnector>(modul.ConnectorsCount);

        foreach (ModulConnector connecter in modul.Connectors)
        {
            if (connecter != DConnectors)
            {
                newConnecters.Add(connecter);
            }
        }


        return newConnecters;
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
        Shuffle(itemSpawnPoints.Count, out int[] itemcount);
        foreach (int index in itemcount)
        {
            itemSpawnPoints[index].SpawnItem();
        }
        // 아이템 랜덤선택 ItemType에서 Scrap항목만 생성
        //
        doMakeComplite = true; // 던전 생성 끝
    }

    /// <summary>
    /// 셔플용 함수
    /// </summary>
    /// <param name="count">셔플할 숫자 범위 (0 ~ count -1)</param>
    /// <param name="result">셔플 결과</param>
    void Shuffle(int count, out int[] result)
    {
        // count만큼 순서대로 숫자가 들어간 배열 만들기
        result = new int[count];
        for (int i = 0; i < count; i++)
        {
            result[i] = i;
        }
        // 위에서 만든 배열을 섞기 
        int loopCount = result.Length - 1;
        for (int i = 0; i < loopCount; i++) // 8*8일때 63번 반복
        {
            int randomIndex = UnityEngine.Random.Range(0, result.Length - i);   // 처음에는 0 ~ 63 중 랜덤 선택
            int lastIndex = loopCount - i;                                      // 처음엔 63번 인덱스

            (result[lastIndex], result[randomIndex]) = (result[randomIndex], result[lastIndex]);    // 랜덤으로 나온 값과 63번 값 스왑
        }
    }

    public bool IsStartGameDone()
    {
        return doMakeComplite;
    }
}