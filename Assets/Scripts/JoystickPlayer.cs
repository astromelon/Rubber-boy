using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class JoystickPlayer : MonoBehaviour
{
    /// <summary>
    /// Runner game templete 소스 시작
    /// </summary>
    // public static JoystickPlayer Instance => s_Instance;
    // static JoystickPlayer s_Instance;
    // Transform m_Transform;
    // Vector3 m_StartPosition, m_DefaultScale, m_Scale, m_TargetScale;
    // float m_StartHeight, m_Speed, m_TargetSpeed;
    // public Vector3 Scale => m_Scale;
    // [SerializeField]
    // SkinnedMeshRenderer m_SkinnedMeshRenderer;
    // [SerializeField]
    // PlayerSpeedPreset m_PlayerSpeed = PlayerSpeedPreset.Medium;
    // [SerializeField]
    // float m_CustomPlayerSpeed = 10.0f;
    
    // enum PlayerSpeedPreset
    // {
    //     Slow,
    //     Medium,
    //     Fast,
    //     Custom
    // }
    // void Awake()
    // {
    //     if (s_Instance != null && s_Instance != this)
    //     {
    //         Destroy(gameObject);
    //         return;
    //     }

    //     s_Instance = this;

    //     Initialize();
    // }

    // /// <summary>
    // /// Set up all necessary values for the PlayerController.
    // /// </summary>
    // public void Initialize()
    // {
    //     m_Transform = transform;
    //     m_StartPosition = m_Transform.position;
    //     m_DefaultScale = m_Transform.localScale;
    //     m_Scale = m_DefaultScale;
    //     m_TargetScale = m_Scale;

    //     if (m_SkinnedMeshRenderer != null)
    //     {
    //         m_StartHeight = m_SkinnedMeshRenderer.bounds.size.y;
    //     }
    //     else 
    //     {
    //         m_StartHeight = 1.0f;
    //     }

    //     ResetSpeed();
    // }

    // public void ResetSpeed()
    // {
    //     m_Speed = 0.0f;
    //     m_TargetSpeed = GetDefaultSpeed();
    // }

    // public float GetDefaultSpeed()
    // {
    //     switch (m_PlayerSpeed)
    //     {
    //         case PlayerSpeedPreset.Slow:
    //             return 5.0f;

    //         case PlayerSpeedPreset.Medium:
    //             return 10.0f;

    //         case PlayerSpeedPreset.Fast:
    //             return 20.0f;
    //     }

    //     return m_CustomPlayerSpeed;
    // }

    /// <summary>
    /// Runner game templete 소스 끝
    /// </summary>

    // Player 상태 정보
    public enum State
    {
        IDLE,       // 가만히 있을때
        ATTACK,     // 일반 공격
        SPECIAL,    // 특수 공격
        STUN,       // 스턴 상태
        DIE,        // 사망
    }

    

    // Player 현재 상태
    public State state;
    // Player 공격 사정거리
    public float attackDist;
    // 공격하는데 걸리는 시간
    public float timeToAttack;
    // Player 사망 여부
    public bool isDie = false;

    public float speed;
    public VariableJoystick variableJoystick;
    public Rigidbody rb;
    private Vector3 initialVelocity;

    private float lastTapTime;
    private float doubleTapTimeThreshold;  // 더블 탭 간격을 조절할 수 있는 임계값

    private Animator anim;     // Animation 컴포넌트를 지정할 변수
    // private readonly int hashPunch = Animator.StringToHash("IsPunch");      // Animator 파라미터의 해시값 추출
    private bool touchInputEnabled;  // 모바일 터치 입력 활성화 유무

    public float leftLimit, rightLimit, upLimit, downLimit;    // 플레이어 가동범위 제약

    Vector3 myPos;
    Vector3 direction;

    public float decelerationTime; // 감속 시간 (조절 가능)
    // private Coroutine decelerateCoroutine;   // 감속 코루틴
    public float dragFactor = 0.5f;     // 감속 계수
    
    
    public void Start()
    {
        state = State.IDLE;     // 플레이어 기본상태(대기)
        speed = 15.0f;          // 플레이어 기본 스피드
        attackDist = 5.0f;      // 플레이어 공격거리
        timeToAttack = 2.8f;    // 플레이어 공격하는데 걸리는 시간

        touchInputEnabled = true;   // 모바일 터치 활성화
        doubleTapTimeThreshold = 0.2f;  // 더블 탭 간격을 조절할 수 있는 임계값
        
        anim = GetComponent<Animator>();    // Animator 컴포넌트 할당
        
        StartCoroutine(CheckPlayerState());     // Player 상태 체크를 위한 코루틴
        
        StartCoroutine(PlayerAction());     // Player 상태따라 수행해야 할 코루틴

        // 플레이어 가동범위 제약
        leftLimit = -2.7f;
        rightLimit = 2.6f;
        downLimit = -4.8f;
        upLimit = 4.5f;

        // 감속시간
        // decelerationTime = 1.0f;
       
    }

    // 일정 간격으로 Player 행동 상태 체크 및 상태 변경
    IEnumerator CheckPlayerState()
    {
        while(!isDie)
        {
            // 0.3초 동안 중지(대기)하는 동안 제어권을 메시지 루프에 양보
            yield return new WaitForSeconds(0.3f);

            // 적과의 거리 측정

            // 공격 사정거리에 들어왔는지 확인
            
            // 특수공격 체크
            
            // 스턴 체크
        }
    }

    // Player 상태변경에 따른 행동 수행
    IEnumerator PlayerAction()
    {
        while(!isDie)
        {
            switch (state)
            {
                case State.IDLE:
                    anim.SetBool("IsPunch", false);
                    break;

                case State.ATTACK:
                    anim.SetBool("IsPunch", true);
                    
                    break;
                
                case State.SPECIAL:

                    break;
                
                case State.STUN:

                    break;
                
                case State.DIE:

                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator DisableAttack(float sec)
    {
        yield return new WaitForSeconds(sec);
        state = State.IDLE;
    }

    public void Update()
    {
        // Debug.Log(state);

        // 손가락이 터치패드에서 떼 졌을때 속도감소
        if (touchInputEnabled is false)
        {
            rb.drag = dragFactor;   // 감속 적용
            Debug.Log("감속");
        }
        else
        {
            rb.drag = 0f;   // 감속 해제
            Debug.Log("감속해제");
        }

        MtouchEvent();    // 공격상태(더블텝) 체크
        ClampedPosition(); // 이동 제약
    }

    public void FixedUpdate()
    {
        

        Joystick();     // 움직임상태(가상 조이스틱) 체크
    }

    void Joystick()
    {   
        switch (state)
        {
            // 대기 상태 일 때만 움직임
            case State.IDLE:
                // 가상 조이스틱
                direction = Vector3.up * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
                rb.AddRelativeForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
                
                break;

            // 공격상태 일땐 핸드폰 터치 못함
            case State.ATTACK:
                rb.velocity = Vector3.zero;
                break;
        }
    }

    
    void MtouchEvent()
    {
        // Debug.Log(rb.velocity);

        
        // 모바일 디바이스에서 터치 이벤트를 감지
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                
                switch (touch.phase)
                {
                    // 손가락이 화면에 처음 닿았을 때
                    case TouchPhase.Began:
                        touchInputEnabled = true;  // 터치 됨

                        // 더블 탭 감지
                        if (Time.time - lastTapTime < doubleTapTimeThreshold)
                        {
                            if (Time.timeScale != 0)
                            {
                                // 공격 상태
                                state = State.ATTACK;
                                // Attack 상태 Idle로 변경 코루틴(n초 동안 정지해서 공격)
                                StartCoroutine(DisableAttack(timeToAttack));
                            }
                        }
                        lastTapTime = Time.time;
                        break;

                    case TouchPhase.Ended:

                    // 손가락이 화면에서 떼었을 때 또는 터치 입력이 취소되었을 때
                    case TouchPhase.Canceled:
                        touchInputEnabled = false; 
                        Debug.Log("터치 이벤트 종료!");

                        // 터치 끝날 때 감속 코루틴 시작
                        // if (decelerateCoroutine == null)
                        // {
                        //     Debug.Log("감속 코루틴!");
                        //     decelerateCoroutine = StartCoroutine(Decelerate());
                        // }
                        
                        break;
                        

                }
            }
        }
    // private IEnumerator Decelerate()
    // {
    //     float elapsedTime = 0f;
    //     while (elapsedTime < decelerationTime)
    //     {
    //         // 시간에 따라 속도를 서서히 감소
    //         rb.velocity = Vector3.Lerp(initialVelocity, Vector3.zero, elapsedTime / decelerationTime);

    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }

    //     // 감속이 완료되면 코루틴 종료
    //     decelerateCoroutine = null;
    //     Debug.Log("감속 코루틴 종료");
    // }

        
    // 플레이어 이동범위 제약
    void ClampedPosition()
    {
        myPos = transform.position;
        
        myPos.x = Mathf.Clamp(myPos.x,leftLimit, rightLimit);    // 좌우 범위
        myPos.y = Mathf.Clamp(myPos.y, downLimit, upLimit);    // 위아래 범위
            
        transform.position = myPos;
        
        // Debug.Log("현재위치: " + myPos);
    }
        
}
