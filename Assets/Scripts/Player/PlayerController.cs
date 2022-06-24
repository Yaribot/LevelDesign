using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    private Vector3 _inputs;
    [SerializeField]
    private float _speed = 5f, _turnSpeed = 360f;

    private bool isGrounded = false;
    [SerializeField]
    private float jumpHeight = 8f, jumpSpeed = 10f;

    public Transform feetPos;
    [SerializeField]
    private float checkRadius, checkRadiusWall;

    public LayerMask groundMask;
    private bool _jumpInput;
    private bool _pauseInput;
    private bool _agentInput;

    public Transform CheckWallsPos;
    private bool wallDetection;

    private Animator _animator;

    private int hashRunParam;
    private int hashJumpParam;

    public bool isEnable, isPaused, agentGoToGoal;

    private Collider col;
    private GameObject gfx;

    public GameManager gm;

    public GameObject UiPauseMenu;

    //private string runParam = "Speed";
    //private string jumpParam = "Jump";

    // Start is called before the first frame update
    void Start()
    {
        isEnable = true;
        _rb = GetComponent<Rigidbody>();
        _animator = transform.GetChild(0).transform.GetChild(0).transform.GetComponent<Animator>();
        hashRunParam = Animator.StringToHash("Speed");
        hashJumpParam = Animator.StringToHash("Jump");
        col = GetComponent<Collider>();
        gfx = transform.GetChild(0).transform.gameObject;
        _turnSpeed = 1000f;
        agentGoToGoal = false;
    }

    private void FixedUpdate()
    {
        Move();
    }

    // Update is called once per frame
    void Update()
    {

        if (!isEnable)
        {
            _rb.constraints = RigidbodyConstraints.FreezePosition;
            col.enabled = false;
            gfx.SetActive(false);
        }

        if (isEnable)
        {
            _rb.constraints = RigidbodyConstraints.None;
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
            Gatherinputs();
            col.enabled = true;
            gfx.SetActive(true);
        }
        Look();

        
        
        isGrounded = Physics.CheckSphere(feetPos.position, checkRadius, groundMask);
        wallDetection = Physics.CheckSphere(CheckWallsPos.position, checkRadiusWall, groundMask);
        if (isGrounded && _jumpInput)
        {
            Jump();
        }

        if (_pauseInput)
        {
            Pause();
        }

        if (isPaused)
        {
            UiPauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            UiPauseMenu.SetActive(false);
            Time.timeScale = 1;
        }

        if (_agentInput)
        {
            gm.once = true;
            gm.activatePathAgent = !gm.activatePathAgent;
        }


        
    }

    private void Gatherinputs()
    {
        _inputs = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        _jumpInput = Input.GetKeyDown(KeyCode.Space);
        _pauseInput = Input.GetKeyDown(KeyCode.Escape);
        _agentInput = Input.GetKeyDown(KeyCode.F);
        _animator.SetFloat(hashRunParam, Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Vertical")));
        //_animator.SetFloat(hashRunParam, Mathf.Abs(Input.GetAxisRaw("Vertical")));
        _animator.SetBool(hashJumpParam, _jumpInput);
    }

    private void Move()
    {
        if (wallDetection)
        {
            _rb.MovePosition(transform.position + Vector3.zero * _speed * Time.deltaTime);
        }
        else
        {
            //if (!isGrounded)
            //{
            //    _rb.AddForce(transform.position + (transform.forward * _inputs.normalized.magnitude));
            //}
            //else
            //{
                //_rb.MovePosition(transform.position + _inputs * _speed * Time.deltaTime);
               _rb.MovePosition(transform.position + (transform.forward * _inputs.normalized.magnitude) * _speed * Time.deltaTime);
            //}            
        }
    }

    private void Look()
    {
        if(_inputs != Vector3.zero)
        {

                                                     // _inputs <-- to get the control back iso movement
            Quaternion rot = Quaternion.LookRotation(_inputs.ToIso(), Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _turnSpeed * Time.deltaTime);
        }
    }

    private void Jump()
    {
        _rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);              
    }

    private void Pause()
    {
        isPaused = !isPaused;       
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(feetPos.position, checkRadius);
        Gizmos.DrawWireSphere(CheckWallsPos.position, checkRadiusWall);
    }

    private void OnParticleCollision(GameObject other) // Detection collision with particles
    {
        isEnable = false;
        //Destroy(this.gameObject);
        gm.deathCount++;
        gm.totalDeath.Value++;
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach(ContactPoint hitPos in collision.contacts)
        {
            if(collision.gameObject.layer == 7)
            {
                Debug.Log("player hit pos is : " + hitPos.normal);
                if(hitPos.normal.y < 1)
                {
                    _rb.AddForce(transform.up * 3f, ForceMode.Impulse);
                }
            }
        }
    }
}
