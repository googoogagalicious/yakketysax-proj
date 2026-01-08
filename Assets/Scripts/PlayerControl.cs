using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class PlayerControl : MonoBehaviour
{
    public float walkSpeed;
    public float maxWalk;
    public float turnSpeed;
    private Vector2 moveInput;
    private Vector2 rotateInput;
    private Rigidbody rigi;
    private IA_PlayerControl ctrl;
    public float jumpForce;
    public float probeLen; 
    public bool grounded;
    public LayerMask whatIsGround;
    private int keyCount;
    public int minKeys;
    public TextMeshProUGUI displayKeys;
    public TextMeshProUGUI displayTime;
    private float timer;
    public bool isAlive;
    public GameObject restartBtn;

    
    void jump(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (grounded & isAlive)
        {
            rigi.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    void UpdateKeys()
    {
        displayKeys.text = keyCount.ToString("00") + "/" + minKeys.ToString("00") + "Keys";
    }
    
    void UpdateTimer()
    {
        displayTime.text = timer.ToString("000.00");
    }
    
    void Awake()
    {
        rigi = GetComponent<Rigidbody>();
        ctrl = new IA_PlayerControl();
        ctrl.Enable();
        ctrl.Player.Jump.started += jump;
        keyCount = 0;
        UpdateKeys();
        timer = 0f;
        UpdateTimer();
        isAlive = true;
        restartBtn.SetActive(false);

    }

    private void OnDisable()
    {
        ctrl.Disable();
    }

    void FixedUpdate()
    {
        if(isAlive)
        {
        //reads input from the user
        moveInput = ctrl.Player.Move.ReadValue<Vector2>();
        rotateInput = ctrl.Player.Rotate.ReadValue<Vector2>();

        timer += Time.deltaTime;
        UpdateTimer();
        
        if (moveInput.magnitude > 0.1f)
        {
            Vector3 moveForward = moveInput.y * this.transform.forward;
            Vector3 moveRight = moveInput.x * this.transform.right;
            Vector3 moveVector = moveForward + moveRight;
            rigi.AddForce(moveVector * walkSpeed * Time.deltaTime);

            rigi.linearVelocity = Vector3.ClampMagnitude(rigi.linearVelocity, maxWalk);
        }
        grounded = Physics.Raycast(this.transform.position, Vector3.down, probeLen,
                                            whatIsGround);
        if (rotateInput.magnitude > 0.1f)
        {
            Vector3 anglevelocity = new Vector3(0f, rotateInput.x * turnSpeed, 0f);
            Quaternion deltarot = Quaternion.Euler(anglevelocity * Time.deltaTime);
            rigi.MoveRotation(rigi.rotation * deltarot);

        }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Key")
        {
            keyCount++;
            Debug.Log(keyCount);
            UpdateKeys();
            Destroy(other.gameObject);
        }
        else if (other.transform.tag == "Finish")
        {
            if (keyCount < minKeys)
            {
                Debug.Log("Collect more keys to exit");
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

        void OnCollisionEnter(Collision other)
        {
            if(other.transform.tag == "Enemy")
            {
                isAlive = false;
                restartBtn.SetActive(true);
            }
        }

}
