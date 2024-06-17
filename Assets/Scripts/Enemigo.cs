using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Enemigo : MonoBehaviour
{
    public Estados estado;
    public float distanciaSeguir;
    public float distanciaAtacar;
    public float distanciaEscapar;

    public bool autoseleccionarTarget = true;
    public Transform target;
    public float distancia;
    public bool vivo = true;

    public void Awake()
    {
        StartCoroutine(CalcularDistancia());
    }

    private void Start()
    {
        if(autoseleccionarTarget)
            target = Personaje1.singleton.transform;
    }

    private void LateUpdate()
    {
        ChequearEstado();
    }

    public void ChequearEstado()
    {
        switch (estado)
        {
            case Estados.idle:
                EstadoIdle();
                break;
            case Estados.seguir:
                transform.LookAt(target,Vector3.up);
                EstadoSeguir();
                break;
            case Estados.atacar:
                EstadoAtacar();
                break;
            case Estados.muerto:
                EstadoMuerto();
                break;
        }
    }
    public void CambiarEstado(Estados e)
    {
        switch (e)
        {
            case Estados.idle:
                break;
            case Estados.seguir:
                break;
            case Estados.atacar:
                break;
            case Estados.muerto:
                vivo = false;
                break;
        }
        estado = e;
    }

    public virtual void EstadoIdle()
    {
        if(distancia < distanciaSeguir)
        {
            CambiarEstado(Estados.seguir);
        }
    }
    public virtual void EstadoSeguir()
    {
        if(distancia < distanciaAtacar)
        {
            CambiarEstado(Estados.atacar);
        }
        else if (distancia > distanciaEscapar)
        {
            CambiarEstado(Estados.idle);
        }
    }
    public virtual void EstadoAtacar()
    {
        if(distancia > distanciaAtacar + 0.1f)
        {
            CambiarEstado(Estados.seguir);
        }  
    }
    public virtual void EstadoMuerto()
    {
         
    }

    IEnumerator CalcularDistancia()
    {
        while (vivo)
        {
            yield return new WaitForSeconds(0.2f);
            if (target != null)
            {
                distancia = Vector3.Distance(transform.position, target.position);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, distanciaAtacar);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, distanciaSeguir);
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, Vector3.up, distanciaEscapar);
    }
#endif

    private void OnDrawGizmos()
    {
        int icono = (int)estado;
        icono++;
        Gizmos.DrawIcon(transform.position + Vector3.up * 0.6f, "0" + icono +".png",false);
    }
}
public enum Estados
{
    idle = 0,
    seguir = 1,
    atacar = 2,
    muerto = 3

}
