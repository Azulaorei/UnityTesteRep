using System;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController player;
    public Animator anim;
    public LayerMask cenario;
    public float vel;
    public float velRun;
    private Vector3 Direcao;

    // Camera:
    public Transform camera;
    public float suavizarCamera;
    private float velocidadeRotacao;

    // Pulo:
    public float alturaPulo;
    public Transform groundCheck;
    public float gravidade;
    private Vector3 forcaY;
    public bool tempoPulo;



    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Direcao = new Vector3(x, 0, z);

        float visao = Mathf.Atan2(Direcao.x, Direcao.z) * Mathf.Rad2Deg + camera.eulerAngles.y; // Calcula o ângulo de direção:
        float anglo = Mathf.SmoothDampAngle(transform.eulerAngles.y, visao, ref velocidadeRotacao, suavizarCamera); // Suaviza a rotação para transição suave

        Movimento(visao, anglo);
        Correr(visao, anglo);
        Controle();
    }



    public void TempoPulo(bool liberarPulo)
    {
        tempoPulo = liberarPulo;
    }


    private void Controle()
    {
        bool isGrounded = Physics.CheckSphere(groundCheck.position, 0.3f, cenario);

        if (Input.GetKeyDown(KeyCode.Space) & isGrounded & tempoPulo == false)
        {
            anim.SetTrigger("Pular");
            forcaY.y = MathF.Sqrt(-2f * alturaPulo * gravidade);
        }

        // Gravidade no Player:
        forcaY.y += gravidade * Time.deltaTime;
        player.Move(forcaY * Time.deltaTime);
    }

    private void Correr(float visao, float anglo)
    {
        if (Input.GetKey(KeyCode.LeftShift) && Direcao.magnitude >= 0.1f)
        {
            transform.rotation = Quaternion.Euler(0, anglo, 0);
            Vector3 novaDirecao = Quaternion.Euler(0, visao, 0) * Vector3.forward;

            player.Move(novaDirecao * velRun * Time.deltaTime);
            anim.SetBool("Correr", true);
        }

        else
        {
            anim.SetBool("Correr", false);
        }
    }

    private void Movimento(float visao, float anglo)
    {
        if (Direcao.magnitude >= 0.1f)
        {
            // Aplica a rotação suavizada ao personagem:
            transform.rotation = Quaternion.Euler(0, anglo, 0);

            // Calcula nova direção com rotação aplicada.
            Vector3 novaDirecao = Quaternion.Euler(0, visao, 0) * Vector3.forward;

            player.Move(novaDirecao * vel * Time.deltaTime);
            anim.SetBool("Andar", true);
        }

        else
        {
            player.Move(Direcao * vel * Time.deltaTime);
            anim.SetBool("Andar", false);
        }
    }
}
