using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour, IUpdateable
{
    [SerializeField]
    private Camera targerCam;
    [SerializeField]
    private MovePlayer targetCharacter;
    [SerializeField]
    private Transform patrolLocation;
    [SerializeField]
    private float patrolRange;
    [SerializeField]
    private float viewRadius;
    [SerializeField]
    [Range(0, 360)]
    private float viewAngle;
    [SerializeField]
    private float ecoInterval;
    [SerializeField]
    private LayerMask obstacleMask;
    [SerializeField]
    private FadeScene fadeOutScene;
    [SerializeField]
    private Transform spawnLocation;

    private bool visiblePlayer;
    private NavMeshAgent navMeshAgent;
    private BehaviorTree behaviorTree;
    private Vector3 patrolPoint;
    private Vector3 lastSeenPos;
    private float WaitTime;
    private Animator animator;
    private bool isGrab = false;

    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, true, false, false);
    }

    private void OnDisable()
    {
        UpdateManager.OffSubscribe(this, true, false, false);
    }

    public void FixedUpdateWork() { }
    public void UpdateWork()
    {
        if (behaviorTree.GetRunState())
        {
            behaviorTree.RunTree();

            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
            //플레이어를 잡은경우 AI를 플레이어 방향으로 로테이션시켜줌
            if (isGrab)
            {
                this.transform.DOLookAt(targetCharacter.transform.position, 0.1f);
            }
            //네비메쉬에서 미끄러짐을 방지하기위함
            //관성이나 장애물에 의해 달라진 속도를 실제속도로 사용하기위함
            //navMeshAgent.desiredVelocity 는 다음 목적지로 향하는 목표속도를 나타낸다
            navMeshAgent.velocity = navMeshAgent.desiredVelocity;
        }
    }
    public void LateUpdateWork() { }


    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        behaviorTree = GetComponent<BehaviorTree>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {

        InitBehaviorTree();
        behaviorTree.gameObject.SetActive(false);
        behaviorTree.ChangeTreeState();
        gameObject.SetActive(false);
    }

    void InitBehaviorTree()
    {
        //트리에서 우선적으로 체크해야하는 노드부터 자식에 추가해준다.
        SelectNode selectNode = new SelectNode();
        behaviorTree.rootNode = selectNode;

        //플레이어를 잡을 수 있는지 확인후 간능하다면 잡는 시퀀스//
        SequenceNode GrabNode = new SequenceNode();
        selectNode.childList.Add(GrabNode);

        DecoratorNode checkDistanceToGrabNode = new DecoratorNode(CheckDistanceToGrab);
        GrabNode.childList.Add(checkDistanceToGrabNode);

        TaskNode GrabTaskNode = new TaskNode(GrabPlayer);
        GrabNode.childList.Add(GrabTaskNode);

        //플레이어가 시야범위내에 들어온경우 추격하는 시퀀스//
        SequenceNode sequenceNode = new SequenceNode();
        selectNode.childList.Add(sequenceNode);

        DecoratorNode decoratorNode = new DecoratorNode(CheckDistance);
        sequenceNode.childList.Add(decoratorNode);

        TaskNode taskNode = new TaskNode(MoveToPlayer);
        sequenceNode.childList.Add(taskNode);


        //플레이어가 시야에 없는경우 순찰하는 시퀀스//
        SequenceNode patsqeNode = new SequenceNode();
        selectNode.childList.Add(patsqeNode);

        TaskNode WaitNode = new TaskNode(Wait);
        patsqeNode.childList.Add(WaitNode);

        TaskNode patposNode = new TaskNode(SetPatrolPos);
        patsqeNode.childList.Add(patposNode);

        TaskNode patrolNode = new TaskNode(patrol);
        patsqeNode.childList.Add(patrolNode);

    }


    public ReturnCode CheckDistance()
    {
        if (visiblePlayer)
        {
            return ReturnCode.SUCCESS;
        }
        else
        {
            return ReturnCode.FAILURE;
        }
    }
    public ReturnCode CheckDistanceToGrab()
    {
        //플레이어가 일정거리내에 들어오면 잡는다.
        float dist = Vector3.Distance(targetCharacter.transform.position, this.transform.position);
        if (dist <= 2.8)
        {
            return ReturnCode.SUCCESS;
        }
        else
        {
            return ReturnCode.FAILURE;
        }

    }

    public ReturnCode GrabPlayer()
    {
        //잡힌 플레이어의 인풋을 중단
        targetCharacter.CharacterStop = true;

        //잡은 플레이어의 방향을 AI 로 로테이션
        targerCam.transform.DOLookAt(new Vector3(this.transform.position.x,
                                                  this.transform.position.y + 4,
                                                  this.transform.position.z), 0.5f);
        targetCharacter.transform.transform.DOLookAt(this.transform.position, 0.5f);

        //화면이 점점 검은색으로 바꾸고 ai의 동작을 정지시킴
        fadeOutScene.FadeOut();
        isGrab = true;
        navMeshAgent.speed = 0;
        visiblePlayer = false;
        behaviorTree.ChangeTreeState();

        animator.SetBool("Grab", true);
        //게임 재시작
        Invoke("ResetGame", 2.0f);

        return ReturnCode.SUCCESS;

    }
    public ReturnCode MoveToPlayer()
    {
        //플레이어가 시야에 있는경우 플레이어를 추격
        //시야에서 사라진경우 마지막 위치까지 추격
        navMeshAgent.speed = 6.5f;
        float dist = Vector3.Distance(targetCharacter.transform.position, this.transform.position);
        float last = Vector3.Distance(lastSeenPos, this.transform.position);
        if (dist <= 2.6 || last <= 2.6)
        {

            return ReturnCode.SUCCESS;
        }
        else if (!visiblePlayer)
        {
            navMeshAgent.SetDestination(lastSeenPos);
            return ReturnCode.RUNNING;
        }
        else
        {
            navMeshAgent.SetDestination(targetCharacter.transform.position);
            return ReturnCode.RUNNING;
        }
    }
    public ReturnCode patrol()
    {
        navMeshAgent.speed = 5;
        float dist = Vector3.Distance(patrolLocation.position, this.transform.position);
        if (dist < 1 || visiblePlayer)
        {
            return ReturnCode.SUCCESS;
        }
        else
        {

            return ReturnCode.RUNNING;
        }
    }
    public ReturnCode SetPatrolPos()
    {
        FindPatrolLocation();

        navMeshAgent.SetDestination(patrolLocation.position);
        return ReturnCode.SUCCESS;
    }
    public Vector3 DirFromAngle(float angleDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Cos((-angleDegrees + 90) * Mathf.Deg2Rad), 0, Mathf.Sin((-angleDegrees + 90) * Mathf.Deg2Rad));
    }
    //플레이어를 주기적으로 서치
    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }
    void FindVisibleTargets()
    {

        Vector3 dirToTarget = (targetCharacter.transform.position - transform.position).normalized;

        //  forward와 target이 이루는 각이 설정한 각도 내라면
        if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
        {
            float dstToTarget = Vector3.Distance(transform.position, targetCharacter.transform.transform.position);

            // 타겟으로 가는 레이캐스트에 obstacleMask가 걸리지 않으면
            if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask) && dstToTarget <= viewRadius)
            {
                lastSeenPos = targetCharacter.transform.position;
                visiblePlayer = true;
            }
            else
            {
                visiblePlayer = false;
            }
        }

    }
    void FindPatrolLocation()
    {

        //플레이어 기준 랜덤생성
        //if (RandomPoint(targetCharacter.transform.position , patrolRange, out patrolPoint))
        // {
        //     patrolLocation.position = patrolPoint;
        // }

        //완전 무작위
        if (RandomPoint(transform.position, patrolRange, out patrolPoint))
        {
            patrolLocation.position = patrolPoint;
        }
    }
    //ai가 순찰 시 랜덤한 위치를 선택하는 함수
    //범위가 넓어 위치가 찍히지 않는다면 루프횟수를 늘려보거나 SamplePosition 함수에서 maxdistance를 늘려보자
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 50; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 5.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = targetCharacter.transform.position;
        return true;
    }
    public ReturnCode Wait()
    {
        WaitTime += Time.deltaTime;
        if (WaitTime >= 2)
        {
            WaitTime = 0;
            return ReturnCode.SUCCESS;
        }
        else if (visiblePlayer)
        {
            return ReturnCode.FAILURE;
        }
        else
        {
            return ReturnCode.RUNNING;
        }
    }
    //플레이어가 ai에게 잡힌경우 게임을 재 시작한다.
    //씬을 재 로딩 하는것이 아닌 단순 입구로 이동시켜 재시작 하는 느낌을 주고자함
    //씬을 재 로딩하게되면 비밀번호가 전부 바뀌기 때문에 아래와같이 구성
    void ResetGame()
    {
        StopCoroutine(FindTargetsWithDelay(0.2f));
        gameObject.SetActive(false);
        targetCharacter.CharacterStop = false;
        targetCharacter.transform.position = spawnLocation.position;

        fadeOutScene.FadeIn();
        animator.SetBool("Grab", false);
        //Debug.Log("Restart");
        isGrab = false;
        behaviorTree.gameObject.SetActive(false);
        Invoke("SpawnEnemy", 15.0f);

        targetCharacter.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        targetCharacter.CharacterStop = false;
        targetCharacter.VisibleMousePointer(false);

        targetCharacter.InitStamina();

        var t1 = GameManager.Instance.ObjectPool.Get(PoolingType.PlayerEcho);
        t1.Activate(spawnLocation);
        GameManager.Instance.ObjectPool.Return(t1);
    }
    //게임 시작 또는 리셋되었을경우 ai를 스폰해주는 함수
    public void SpawnEnemy()
    {
        transform.position = spawnLocation.position;
        navMeshAgent.speed = 0;
        behaviorTree.gameObject.SetActive(true);
        gameObject.SetActive(true);
        StartCoroutine(FindTargetsWithDelay(0.2f));
        behaviorTree.ChangeTreeState();
        lastSeenPos = this.transform.position;
        visiblePlayer = false;
    }
    //발소리 및 에코함수
    public void FootSound()
    {

        GameManager.Instance.SoundManager.PlaySound3D("Sound/walk", transform);

    }
    public void EcoWithSound()
    {
        var t1 = GameManager.Instance.ObjectPool.Get(PoolingType.NPCEcho);

        t1.Activate(transform);
        GameManager.Instance.ObjectPool.Return(t1);
        GameManager.Instance.SoundManager.PlaySound3D("Sound/walk", transform);
    }

    public void RunSound()
    {

        GameManager.Instance.SoundManager.PlaySound3D("Sound/walk", transform);

    }
    public void RunEcoWithSound()
    {
        var t1 = GameManager.Instance.ObjectPool.Get(PoolingType.NPCRunEcho);

        t1.Activate(transform);
        GameManager.Instance.ObjectPool.Return(t1);
        GameManager.Instance.SoundManager.PlaySound3D("Sound/walk", transform);
    }

    public float ViewAngle
    {
        set { viewAngle = value; }
        get { return viewAngle; }
    }
    public float ViewRadius
    {
        set { viewRadius = value; }
        get { return viewRadius; }
    }

    public MovePlayer TargetPlayer
    {
        set { targetCharacter = value; }
        get { return targetCharacter; }

    }
}
