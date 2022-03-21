using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Variables que guardan referencias a GameObjects y Componentes
    private GameManager gameManager;
    private SettingsManager settingsManager;

    public GameObject canon;
    public GameObject turret;
    public GameObject blastPrefab;
    public GameObject healthBarUI, shieldBarUI;
    public AudioSource[] audioSourcePlayer;

    private Rigidbody rigidbodyPlayer;
    private Animator canonAnimator;

    // Variables que guardan los Input Axis de teclado y ratón
    private float horizontalInput, verticalInput, mouseInputX, mouseInputY, xRotation;

    // Sensibilidad de movimiento del canon
    private float mouseSensitivity = 0;

    // Vida y escudo del player
    private float healthPlayer = 1f;
    private float shieldPlayer = 1f;

    // Movimiento, rotación y velocidad máxima
    private float speedMovement = 50f;
    private float speedRotation = 80f;
    private float maxVelocity = 50f;

    // Cooldown del disparo
    private bool shootTrigger = true;
    private float shootCooldown = 0.25f;

    // Distancia límite con el suelo
    private float groundDistance = 7f;

    // Rango de valores para definir el damage aleatorio recibido
    private float[] damageHealthRange = { 0.20f, 0.36f };
    private float[] damageShieldRange = { 0.30f, 0.36f };

    // Valores de los items de vida y escudo
    private float healthItemValue = 0.5f;
    private float shieldItemValue = 0.75f;

    // Variable fix para la colision de los disparos
    private bool isBlastColliding = false;

    private void Start()
    {
        // Obtienes las componentes que necesita
        gameManager = FindObjectOfType<GameManager>();
        settingsManager = FindObjectOfType<SettingsManager>();

        rigidbodyPlayer = GetComponent<Rigidbody>();
        audioSourcePlayer = GetComponents<AudioSource>();
        canonAnimator = canon.GetComponent<Animator>();

        // Restablece las barras de escudo y vida
        shieldBarUI.GetComponentInChildren<Slider>().value = shieldPlayer;
        healthBarUI.GetComponentInChildren<Slider>().value = healthPlayer;

        // Modificación del centro de la masa del player
        rigidbodyPlayer.centerOfMass = new Vector3(0, -1, 0);
    }

    void Update()
    {
        // Si pulsas la tecla Espacio o Click Izquierdo y shootTrigger es True
        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0)) && shootTrigger)
        {
            // Dispara
            WeaponShoot();
        }

        // Si pulsas la tecla R resetea totalmente la rotación del player
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.rotation = Quaternion.identity;
        }

        // Actualiza la sensibilidad de movimiento del canon
        mouseSensitivity = settingsManager.mouseSensitivity;
    }

    private void FixedUpdate()
    {
        // Obtiene los axis del teclado
        horizontalInput = Input.GetAxis("Horizontal") * Time.deltaTime;
        verticalInput = Input.GetAxis("Vertical");

        // Obtiene los axis del ratón
        mouseInputX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseInputY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rota la torreta según mouseInputX
        turret.transform.Rotate(Vector3.up * speedRotation * mouseInputX);

        // Limita la rotación en X del canon según mouseInputY
        // Mathf.Clamp - Devuelve un valor limitado como Output
        xRotation -= mouseInputY * speedRotation;
        xRotation = Mathf.Clamp(xRotation, -15f, 15f);
        canon.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // Si el player está tocando el suelo
        if (IsGrounded())
        {
            // Rota en el eje vertical según horizontalInput
            transform.Rotate(Vector3.up * speedRotation * horizontalInput);

            // Acelera en el eje horizontal según verticalInput
            rigidbodyPlayer.AddRelativeForce(Vector3.forward * speedMovement * verticalInput, ForceMode.VelocityChange);

            // Si la magnitud de la velocidad del player es mayor a la velocidad máxima determinada
            if (rigidbodyPlayer.velocity.magnitude > maxVelocity)
            {
                // La velocidad del player recibe esa velocidad normalidada por la velocidad máxima
                rigidbodyPlayer.velocity = rigidbodyPlayer.velocity.normalized * maxVelocity;
            }
        }

        // Añade una fuerza vertical para mantener el player en el suelo
        rigidbodyPlayer.AddForce(Vector3.up * -100);
    }

    // Disparo del player
    private void WeaponShoot()
    {
        // Ejecuta la animación de retroceso del canon
        canonAnimator.SetTrigger("Shoot");

        // Reproduce el sonido de disparo
        audioSourcePlayer[0].Play();

        // Instancia el disparo en la punta del canon
        Instantiate(blastPrefab, canon.transform.GetChild(0).position, canon.transform.GetChild(0).rotation);

        // Llama a WeaponCooldown
        StartCoroutine(WeaponCooldown());
    }

    // Cooldown entre cada disparo
    private IEnumerator WeaponCooldown()
    {
        // shootTrigger es False y espera un tiempo para que vuelva a ser True
        shootTrigger = false;
        yield return new WaitForSeconds(shootCooldown);
        shootTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si obtiene un HealthItem con el player
        if (other.gameObject.CompareTag("HealthItem"))
        {
            // Si healthPlayer es menor a 1
            if (healthPlayer < 1f)
            {
                // Si healthPlayer más healthItem es mayor o igual a 1
                if (healthPlayer + healthItemValue >= 1f)
                {
                    // healthPlayer ahora vale 1
                    healthPlayer = 1f;
                }
                else
                {
                    // healthPlayer más healthItem
                    healthPlayer += healthItemValue;
                }

                // Actualiza la barra de vida del player
                healthBarUI.GetComponentInChildren<Slider>().value = healthPlayer;

                // Reproduce el sonido del item
                audioSourcePlayer[2].Play();

                // Destruye el item
                Destroy(other.gameObject);
            }
        }

        // Si obtiene un ShieldItem con el player
        if (other.gameObject.CompareTag("ShieldItem"))
        {
            // Si shieldPlayer es menor a 1
            if (shieldPlayer < 1f)
            {
                // Si shieldPlayer más shieldItemValue es mayor o igual a 1
                if (shieldPlayer + shieldItemValue >= 1f)
                {
                    // shieldPlayer ahora vale 1
                    shieldPlayer = 1f;
                }
                else
                {
                    //shieldPlayer más shieldItemValue
                    shieldPlayer += shieldItemValue;
                }

                // Actualiza la barra de escudo del player
                shieldBarUI.GetComponentInChildren<Slider>().value = shieldPlayer;

                // Reproduce el sonido del item
                audioSourcePlayer[2].Play();

                // Destruye el item
                Destroy(other.gameObject);
            }
        }

        // Si recibe un disparo del enemy
        if (other.gameObject.CompareTag("EnemyBlast"))
        {
            // Comprueba que solo esté colisionando un disparo (FIX)
            // Evita que se ejecute más de una vez saliendo de la función
            if (isBlastColliding) return;
            isBlastColliding = true;

            // Si shieldPlayer es mayor a 0
            if (shieldPlayer > 0)
            {
                // Resta a shieldPlayer un valor aleatorio entre el rango de damageShieldRange
                shieldPlayer -= Mathf.Round(Random.Range(damageShieldRange[0], damageShieldRange[1]) * 100f) / 100f;

                // Actualiza la barra de escudo del player
                shieldBarUI.GetComponentInChildren<Slider>().value = shieldPlayer;

            }
            // Si healthPlayer es mayor a 0
            else if (healthPlayer > 0)
            {
                // Resta a healthPlayer un valor aleatorio entre el rango de damageHealthRange
                healthPlayer -= Mathf.Round(Random.Range(damageHealthRange[0], damageHealthRange[1]) * 100f) / 100f;

                // Actualiza la barra de escudo del player
                healthBarUI.GetComponentInChildren<Slider>().value = healthPlayer;
            }

            // Si healthPlayer es menor o igual a 0
            if (healthPlayer <= 0)
            {
                // GAME OVER
                gameManager.GameOver();
            }

            // Reproduce el sonido de impacto
            audioSourcePlayer[3].Play();

            // Llama a TriggerEnterOn
            StartCoroutine(TriggerEnterOn());
        }
    }

    // Fix para que solo compruebe una sola colisión de un disparo
    private IEnumerator TriggerEnterOn()
    {
        // Espera al final del frame y isBlastColliding vuelve a False
        yield return new WaitForEndOfFrame();
        isBlastColliding = false;
    }

    // Comprueba que el player esté pegado al suelo a cierta distancia
    private bool IsGrounded()
    {
        // Información del hit del Raycast
        RaycastHit hitData;

        // Ray desde la posición del player en dirección hacia abajo
        Ray ray = new Ray(transform.position, -transform.up);

        // RAYCAST VISUAL EN LA SCENE DE UNITY
        Debug.DrawRay(transform.position, -transform.up * groundDistance, Color.cyan);

        // Si Physics.Raycast con la distancia devuelve True
        if (Physics.Raycast(ray, out hitData, groundDistance))
        {
            // Devuelve True or False dependiendo de si está tocando suelo
            return hitData.collider.CompareTag("Ground");
        }
        else
        {
            // Devuelve False
            return false;
        }
    }
}
