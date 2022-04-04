    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    private Vector3 _inputs;
    [SerializeField]
    private float _speed = 5f, _trunSpeed = 360f;

    private bool isGrounded = false;
    [SerializeField]
    private float jumpHeight = 8f, jumpSpeed = 10f;

    public Transform feetPos;
    [SerializeField]
    private float checkRadius, checkRadiusWall;

    public LayerMask groundMask;
    private bool _jumpInput;

    public Transform CheckWallsPos;
    private bool wallDetection;

    private Animator _animator;

    private int hashRunParam;
    private int hashJumpParam;

    private string runParam = "Speed";
    private string jumpParam = "Jump";

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = transform.GetChild(0).transform.GetChild(0).transform.GetComponent<Animator>();
        hashRunParam = Animator.StringToHash("Speed");
        hashJumpParam = Animator.StringToHash("Jump");
    }

    private void FixedUpdate()
    {
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        Gatherinputs();
        Look();

        
        
        isGrounded = Physics.CheckSphere(feetPos.position, checkRadius, groundMask);
        wallDetection = Physics.CheckSphere(CheckWallsPos.position, checkRadiusWall, groundMask);
        if (isGrounded && _jumpInput)
        {
            Jump();
        }
        
    }

    private void Gatherinputs()
    {
        _inputs = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        _jumpInput = Input.GetKeyDown(KeyCode.Space);
        _animator.SetFloat(hashRunParam, Mathf.Abs(Input.GetAxisRaw("Horizontal")));
        _animator.SetFloat(hashRunParam, Mathf.Abs(Input.GetAxisRaw("Vertical")));
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
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _trunSpeed * Time.deltaTime);
        }
    }

    private void Jump()
    {
        _rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);              
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(feetPos.position, checkRadius);
        Gizmos.DrawWireSphere(CheckWallsPos.position, checkRadiusWall);
    }

    private void OnParticleCollision(GameObject other)
    {
        Destroy(this.gameObject);
    }
}
