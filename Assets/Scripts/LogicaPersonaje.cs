using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LogicaPersonaje : MonoBehaviour
{
    //Correr
    public int velCorrer;
    public float velocidadMovimiento = 5.0f;
    public float velocidadRotacion = 200.0f;
    private Animator anim;
    public float x,y;

    public Rigidbody rb;
    public float fuerzaDeSalto = 8f;
    public bool puedoSaltar;

    public bool estoyAgachado;

    public float velocidadInicial;
    public float velocidadAgachado;

    private CapsuleCollider capsuleCollider;
    private Vector3 colliderCenterOriginal;
    private float colliderHeightOriginal;

    // Start is called before the first frame update
    void Start()
    {
        puedoSaltar = false;
        anim = GetComponent<Animator>();

        velocidadInicial = velocidadMovimiento;
        velocidadAgachado = velocidadMovimiento * 0.5f;

        capsuleCollider = GetComponent<CapsuleCollider>();
        if (capsuleCollider != null)
        {
            colliderCenterOriginal = capsuleCollider.center;
            colliderHeightOriginal = capsuleCollider.height;
        }
    }

    void FixedUpdate()
    {
        transform.Rotate(0,x * Time.deltaTime * velocidadRotacion, 0);
        transform.Translate(0,0,y * Time.deltaTime * velocidadMovimiento);
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        anim.SetFloat("VelX", x);
        anim.SetFloat("VelY", y);

        //Control para Correr
        if(Input.GetKey(KeyCode.LeftShift) && !estoyAgachado)
        {
            velocidadMovimiento = velCorrer;
            if(y > 0)
            {
                anim.SetBool("Correr", true);
            }
            else
            {
                anim.SetBool("Correr", false);
            }
        }
        else
        {
            anim.SetBool("Correr", false);

            if(estoyAgachado)
            {
                velocidadMovimiento = velocidadAgachado;
            }
            else
            {
                velocidadMovimiento = velocidadInicial;
            }
        }

        //Control para Salto
        if(puedoSaltar)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetBool("Salto", true);
                rb.AddForce(new Vector3(0, fuerzaDeSalto, 0), ForceMode.Impulse);
            }

            if(Input.GetKey(KeyCode.LeftControl))
            {
                Agacharse();
                //anim.SetBool("Agachado", true);
                //velocidadMovimiento = velocidadAgachado;
            }
            else
            {
                Levantarse();
                //anim.SetBool("Agachado", false);
                //velocidadMovimiento = velocidadInicial;
            }
            anim.SetBool("TocarSuelo", true);
        }
        else
        {
            EstoyCayendo();
        }
    }

    private void Agacharse()
    {
        anim.SetBool("Agachado", true);
        estoyAgachado = true;
        if (capsuleCollider != null)
        {
            capsuleCollider.height = colliderHeightOriginal * 0.5f;
            capsuleCollider.center = new Vector3(colliderCenterOriginal.x, colliderCenterOriginal.y * 0.5f, colliderCenterOriginal.z);
        }
    }

    private void Levantarse()
    {
        anim.SetBool("Agachado", false);
        estoyAgachado = false;
        if (capsuleCollider != null)
        {
            capsuleCollider.height = colliderHeightOriginal;
            capsuleCollider.center = colliderCenterOriginal;
        }
    }

    public void EstoyCayendo()
    {
        anim.SetBool("TocarSuelo", false);
        anim.SetBool("Salto", false);
    }
}
