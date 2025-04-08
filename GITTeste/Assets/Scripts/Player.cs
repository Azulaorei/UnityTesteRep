using System;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{
    public CharacterController player;
    public Animator anim;
    public float vel;
    public float velRun;
    private Vector3 Direcao;

    // Camera:
    public Transform cam;
    public float suavizarCamera;
    private float velocidadeRotacao;



    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }



    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Direcao = new Vector3(x, 0, z);

        Direcao = cam.transform.TransformDirection(Direcao);

        Movimento();
        Correr();
    }



    private void Correr()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Direcao.magnitude >= 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Direcao), Time.deltaTime * 10);

            player.Move(Direcao * velRun * Time.deltaTime);
            anim.SetBool("Correr", true);
        }

        else
        {
            anim.SetBool("Correr", false);
        }
    }



    private void Movimento()
    {
        if (Direcao.magnitude >= 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Direcao), Time.deltaTime * 10);

            player.Move(Direcao * vel * Time.deltaTime);
            anim.SetBool("Andar", true);
        }

        else
        {
            player.Move(Direcao * vel * Time.deltaTime);
            anim.SetBool("Andar", false);
        }
    }
}
