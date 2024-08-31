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
            //�÷��̾ ������� AI�� �÷��̾� �������� �����̼ǽ�����
            if (isGrab)
            {
                this.transform.DOLookAt(targetCharacter.transform.position, 0.1f);
            }
            //�׺�޽����� �̲������� �����ϱ�����
            //�����̳� ��ֹ��� ���� �޶��� �ӵ��� �����ӵ��� ����ϱ�����
            //navMeshAgent.desiredVelocity �� ���� �������� ���ϴ� ��ǥ�ӵ��� ��Ÿ����
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
        //Ʈ������ �켱������ üũ�ؾ��ϴ� ������ �ڽĿ� �߰����ش�.
        SelectNode selectNode = new SelectNode();
        behaviorTree.rootNode = selectNode;

        //�÷��̾ ���� �� �ִ��� Ȯ���� �����ϴٸ� ��� ������//
        SequenceNode GrabNode = new SequenceNode();
        selectNode.childList.Add(GrabNode);

        DecoratorNode checkDistanceToGrabNode = new DecoratorNode(CheckDistanceToGrab);
        GrabNode.childList.Add(checkDistanceToGrabNode);

        TaskNode GrabTaskNode = new TaskNode(GrabPlayer);
        GrabNode.childList.Add(GrabTaskNode);

        //�÷��̾ �þ߹������� ���°�� �߰��ϴ� ������//
        SequenceNode sequenceNode = new SequenceNode();
        selectNode.childList.Add(sequenceNode);

        DecoratorNode decoratorNode = new DecoratorNode(CheckDistance);
        sequenceNode.childList.Add(decoratorNode);

        TaskNode taskNode = new TaskNode(MoveToPlayer);
        sequenceNode.childList.Add(taskNode);


        //�÷��̾ �þ߿� ���°�� �����ϴ� ������//
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
        //�÷��̾ �����Ÿ����� ������ ��´�.
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
        //���� �÷��̾��� ��ǲ�� �ߴ�
        targetCharacter.CharacterStop = true;

        //���� �÷��̾��� ������ AI �� �����̼�
        targerCam.transform.DOLookAt(new Vector3(this.transform.position.x,
                                                  this.transform.position.y + 4,
                                                  this.transform.position.z), 0.5f);
        targetCharacter.transform.transform.DOLookAt(this.transform.position, 0.5f);

        //ȭ���� ���� ���������� �ٲٰ� ai�� ������ ������Ŵ
        fadeOutScene.FadeOut();
        isGrab = true;
        navMeshAgent.speed = 0;
        visiblePlayer = false;
        behaviorTree.ChangeTreeState();

        animator.SetBool("Grab", true);
        //���� �����
        Invoke("ResetGame", 2.0f);

        return ReturnCode.SUCCESS;

    }
    public ReturnCode MoveToPlayer()
    {
        //�÷��̾ �þ߿� �ִ°�� �÷��̾ �߰�
        //�þ߿��� �������� ������ ��ġ���� �߰�
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
    //�÷��̾ �ֱ������� ��ġ
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

        //  forward�� target�� �̷�� ���� ������ ���� �����
        if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
        {
            float dstToTarget = Vector3.Distance(transform.position, targetCharacter.transform.transform.position);

            // Ÿ������ ���� ����ĳ��Ʈ�� obstacleMask�� �ɸ��� ������
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

        //�÷��̾� ���� ��������
        //if (RandomPoint(targetCharacter.transform.position , patrolRange, out patrolPoint))
        // {
        //     patrolLocation.position = patrolPoint;
        // }

        //���� ������
        if (RandomPoint(transform.position, patrolRange, out patrolPoint))
        {
            patrolLocation.position = patrolPoint;
        }
    }
    //ai�� ���� �� ������ ��ġ�� �����ϴ� �Լ�
    //������ �о� ��ġ�� ������ �ʴ´ٸ� ����Ƚ���� �÷����ų� SamplePosition �Լ����� maxdistance�� �÷�����
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
    //�÷��̾ ai���� ������� ������ �� �����Ѵ�.
    //���� �� �ε� �ϴ°��� �ƴ� �ܼ� �Ա��� �̵����� ����� �ϴ� ������ �ְ�����
    //���� �� �ε��ϰԵǸ� ��й�ȣ�� ���� �ٲ�� ������ �Ʒ��Ͱ��� ����
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
    //���� ���� �Ǵ� ���µǾ������ ai�� �������ִ� �Լ�
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
    //�߼Ҹ� �� �����Լ�
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
