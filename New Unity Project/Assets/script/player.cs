using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    private static player instance;

    public static player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<player>();
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    private IUseable useable;

    [SerializeField]//필드를 직렬화 하도록 설정
    private float movementSpeed;
    private bool facingRight;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    private bool isGrounded;

    private bool jump;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float climbSpeed;

    public Rigidbody2D MyRigidbody;
    public Animator myAnimator;

    public bool OnLadder { get; set; }
    // Use this for initialization
    void Start()
    {
        OnLadder = false;
        facingRight = true;
        MyRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleInput();
    }


    // Update is called once per frame
    void FixedUpdate()//FixedUpdate()에서 이동 계산을 적용 할 때 Time.deltaTime 값을 곱할 필요는 없습니다. 
                      //이것은 프레임 속도와는 독립적으로 FixedUpdate()가 신뢰할 수있는 타이머에서 호출되기 때문 입니다.
    {
        float horizontal = Input.GetAxis("Horizontal");//Input.GetAxis( "Horizontal" ) 함수를 사용하면, 플레이어가 왼쪽 키 또는 오른쪽 키를 눌렀는지 알 수 있다.
        float vertical = Input.GetAxis("Vertical"); //Input.GetAxis("Horizontal") 의 리턴 값이 음수, 양수, 0인지만 확인하면 어떤 키를 눌렀는지 알 수 있다.
        isGrounded = IsGrounded();

        HandleMovement(horizontal, vertical);
        flip(horizontal);
        HandleLayers();
        ResetValue();

    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&!OnLadder)
        {
            myAnimator.SetTrigger("jump");
            jump = true;
        }
    }

    private void HandleMovement(float horizontal, float vertical)
    {
        MyRigidbody.velocity = new Vector2(horizontal * movementSpeed, MyRigidbody.velocity.y);//매개변수 horizontal(-1왼쪽 1오른쪽 0가만히)에 속도곱해서 나감
        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));//스피드라는 에니메이터파라미터에 절대값을 반환(0은 가만히 1은 움직임)

        if (isGrounded && jump)//점프하거나(1반환)땅에없을때
        {
            isGrounded = false;
            MyRigidbody.AddForce(new Vector2(0, jumpForce));//점프할때 jumpForce만큼 더함
             //트리거 매개 변수를 활성으로 설정합니다.트리거 매개 변수는 전환에 사용될 때 false로 재설정되는 bool 매개 변수입니다.
        }
        if (MyRigidbody.velocity.y < 0)//땅에 내려왔을때
        {
            myAnimator.SetBool("land", true);
        }
        if (OnLadder)
        {
            MyRigidbody.velocity = new Vector2(horizontal * climbSpeed, vertical * climbSpeed);
            myAnimator.speed = vertical != 0 ? Mathf.Abs(vertical) : Mathf.Abs(horizontal);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Use();
        }
    }

    private void flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)//왼쪽으로 돌아도 스타트에서 이미 페이싱라이트가 트루이기때문에 이프문으로 들어옴
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    private void Use()
    {
       if (useable != null)
        {
            useable.Use();
        }
    }

    private void ResetValue()
    {
        jump = false;
    }

    private bool IsGrounded()
    {
        if (MyRigidbody.velocity.y <= 0)//땅에 내려왔을때
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        myAnimator.ResetTrigger("jump"); //trigger 매개 변수를 false로 다시 설정합니다.
                        myAnimator.SetBool("land", false);//land는 false
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HandleLayers()
    {
        if (!isGrounded)
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Useable")
        {
            useable = collision.GetComponent<IUseable>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Useable")
        {
            useable = null;
        }
    }
}