using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class EnemyControllerAI : MonoBehaviour
{
    // Variables que guardan referencias a GameObjects
    public GameObject canon;
    public GameObject turret;
    public GameObject blastPrefab;
    public GameObject healthBarUI;
    public GameObject damageTextPrefab;

    private GameObject player;
    private NavMeshAgent enemyAgent;
    private AudioSource audioSourceEnemy;
    private Animator canonAnimator;

    // Vida del enemy
    private float enemyHealth = 1f;

    // Cooldown del disparo
    private float shootCooldown = 2f;
    private bool shootTrigger = true;

    // Distancia del RayCast
    private float playerDetectionDistance = 2000f;

    // Rango de valores para definir el damage aleatorio recibido
    private float[] damageHealthRange = { 0.20f, 0.36f };

    // Variable fix para la colision de los disparos
    private bool isBlastColliding = false;

    private Vector3 directionToPlayer, newPosition;
    private float damage, distance;

    void Start()
    {
        // Obtiene las referencias que necesita
        player = GameObject.Find("Player");
        audioSourceEnemy = GetComponent<AudioSource>();
        enemyAgent = GetComponent<NavMeshAgent>();

        // Obtiene el componente de la animacion del canon
        canonAnimator = canon.GetComponent<Animator>();

        // Restablece la barra de vida
        healthBarUI.GetComponentInChildren<Slider>().value = enemyHealth;
    }

    void Update()
    {
        // Obtiene la distancia entre el enemy y el player
        distance = Vector3.Distance(transform.position, player.transform.position);

        // Si la distancia es mayor o igual 100
        if (distance >= 100)
        {
            // Obtiene la dirección al player
            directionToPlayer = transform.position - player.transform.position;

            // Obtiene una nueva posición en relación a la dirección al player
            newPosition = transform.position - directionToPlayer;

            // Setea el destino del enemyAgent
            enemyAgent.SetDestination(newPosition);

            // Rota el cuerpo del tanque de forma más natural
            // (No puedo explicar lo que hace este trozo de código, fue extraido de Internet)
            if (enemyAgent.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                transform.rotation = Quaternion.LookRotation(enemyAgent.velocity.normalized);
            }
        }

        // Rota la torreta con rotacion bloqueada en X, Z
        turret.transform.LookAt(player.transform.position);
        turret.transform.localEulerAngles = new Vector3(0, turret.transform.localEulerAngles.y, 0);

        // Rota el canon con rotacion bloqueada en Y, Z
        canon.transform.LookAt(player.transform.position);
        canon.transform.localEulerAngles = new Vector3(canon.transform.localEulerAngles.x, 0, 0);

        // Si shootTrigger es True y el player está a tiro
        if (shootTrigger && IsPlayerOnSight())
        {
            // Dispara
            WeaponShoot();
        }
    }

    // Disparo del enemy
    private void WeaponShoot()
    {
        // Reproduce el sonido de disparo
        audioSourceEnemy.Play();

        // Ejecuta la animación de retroceso del canon
        canonAnimator.SetTrigger("Shoot");

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

    // Comprueba que el player en el punto de mira
    private bool IsPlayerOnSight()
    {
        // Información del hit del Raycast
        RaycastHit hitData;

        // Ray desde la posición del canon en dirección hacia adelante
        Ray ray = new Ray(canon.transform.position, canon.transform.forward);

        // RAYCAST VISUAL EN LA SCENE DE UNITY
        Debug.DrawRay(canon.transform.position, canon.transform.forward * playerDetectionDistance, Color.yellow);

        // Si Physics.Raycast con la distancia devuelve True
        if (Physics.Raycast(ray, out hitData, playerDetectionDistance))
        {
            // Devuelve True or False dependiendo de si está tocando Player
            return hitData.collider.CompareTag("Player");
        }
        else
        {
            // Devuelve False
            return false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si recibe un disparo del player
        if (other.gameObject.CompareTag("PlayerBlast"))
        {
            // Comprueba que solo esté colisionando un disparo (FIX)
            // Evita que se ejecute más de una vez saliendo de la función
            if (isBlastColliding) return;
            isBlastColliding = true;

            // Guarda un valor aleatorio del rango que va a obtener como daño
            damage = Mathf.Round(Random.Range(damageHealthRange[0], damageHealthRange[1]) * 100f) / 100f;

            // Resta el daño a enemyHealth
            enemyHealth -= damage;

            // Actualiza la barra de vida
            healthBarUI.GetComponentInChildren<Slider>().value = enemyHealth;

            // Instancia un objeto textDamage en la posición de impacto y con el daño recibido como texto
            GameObject textDamage = Instantiate(damageTextPrefab, other.transform.position, damageTextPrefab.transform.rotation);
            textDamage.transform.GetChild(0).GetComponent<TextMeshPro>().text = "-" + damage.ToString("F2");

            // Si enemyHealth es menor a 1
            if (enemyHealth < 1)
            {
                // Muestra la barra de vida
                healthBarUI.SetActive(true);
            }

            // Si enemyHealth es menor o igual a 0
            if (enemyHealth <= 0)
            {
                // Destruye el enemy
                Destroy(gameObject);
            }

            // Reproduce el sonido de impacto
            player.GetComponent<PlayerController>().audioSourcePlayer[1].Play();

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
}