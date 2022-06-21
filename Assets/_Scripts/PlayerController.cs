using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Salto")]
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float gravityValue = -9.81f;

    [Header("Movimiento Horizontal")]
    public float speed = 15f;

    [Header("Componentes")]
    private CharacterController controller;
    private Vector3 playerVelocity;

    [Header("Ground")]
    private bool groundedPlayer;

    [Header("Inputs")]
    private float x;
    private float z;
    private bool jumping;
    private bool dodging;
    private bool parrying;
    private bool attack;

    [Header("Dodge Parameters")]
    private float dogdeImpulseForce = 20f;

    [Header("Parry")]
    private bool isParry; // Esta en modo defensa?
    private float refresh = 3f; // Cooldown del parry

    [Header("Stamina Parameters")]
    private float parryStaminaCost= 80f;
    public StaminaBar _staminaBar;
    private float maxStamina = 100;
    private float currentStamina;


    [Header("Stamina Regen Parameters")]
    private float staminaRegen = 10f;

    [Header("Attack Parameters")]
    private bool canAttack;

    [Header("Animator Parameter")]
    private Animator _animator;

    [Header("Collider")]
    Collider _collider;
    

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        _collider = GetComponent<Collider>();
        _animator = GameObject.FindGameObjectWithTag("Escudo").GetComponent<Animator>();
        isParry = false;
        currentStamina = maxStamina; // stamina inicial es la maxima
        _staminaBar.SetMaxStamina(maxStamina); 
        
    }

    // Update is called once per frame
    void Update()
    {
        groundedPlayer = controller.isGrounded;

        MyInput();
        Movement();
        Dodge();
        Parry();
        
    }

    private void MyInput(){
        // Inputs
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        jumping = Input.GetButtonDown("Jump");
        parrying = Input.GetKey(KeyCode.F);
        dodging = Input.GetKeyDown(KeyCode.LeftShift);
    }

    private void Movement(){
        Vector3 move = transform.right * x + transform.forward * z; // Establecemos las direcciones,cogiendo la dirección donde está mirando el player,y le aplicamos los inputs 
        controller.Move(move * speed * Time.deltaTime); // movemos el player en x y z

        // Reposicionamos el jugador en caso de bajar de -2 en y
        if(groundedPlayer && playerVelocity.y < -2){
            playerVelocity.y = 0f;
        }

        // Salto
        if(jumping && groundedPlayer){
            // Saltamos dandole valor a y
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        // Gestionamos la gravedad
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Movemos el player en y(Saltar)
        controller.Move(playerVelocity * Time.deltaTime);
    }



    // TODO IEnumerator que haga la accion de esquivar
    private void Dodge(){
        float startTime = Time.time;
        Vector3 move = transform.right * x * 50  + transform.forward * z * 50;

        // TODO Accion de esquivar con tecla shift
        if(dodging){
            controller.Move(move * dogdeImpulseForce * Time.deltaTime);
            
        }
        
    }


    // TODO Mecánica de Parry

    private void Parry(){

        // TODO Detectar enemigos en el rango de ataque

        // TODO Modo defensa,donde el si el jugador es tocado, repele a los enemigos

        // Si toco tecla F, y stamina distinto de 0
        if(parrying && currentStamina != 0){
            
            DrainStamina(parryStaminaCost);
            _animator.SetBool("Parry",true);

            // Para que la animación y la accion de parry no salte de un estado a otro,con esto establecemos que la stamina que este entre 0 y 1 no se aplique
            if(currentStamina < 1){
                _animator.SetBool("Parry",false);
            }
            
        } else{ 
            // Regeneramos Stamina
            RegenerateStamina();
            _animator.SetBool("Parry",false);
            
        }

    }

    // TODO Mecánica ataque si cumple condicion de Parry

    private void Attack(){
        
    }

    /// <summary>
    /// Drena la Stamina
    /// </summary>
    /// <param name="staminaDrain">cantidad de Stamina a drenar</param>
    public void DrainStamina(float staminaDrain){
        currentStamina -= staminaDrain * Time.deltaTime;
        _staminaBar.SetStamina(currentStamina);

        if(currentStamina < 0){
            currentStamina = 0;
        }
        
    }

    /// <summary>
    /// Regenera Stamina
    /// </summary>
    public void RegenerateStamina(){
        if(currentStamina < maxStamina){
            currentStamina += staminaRegen * Time.deltaTime;
            _staminaBar.SetStamina(currentStamina);
        }
    }


}
