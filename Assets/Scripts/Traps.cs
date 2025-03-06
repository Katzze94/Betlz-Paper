using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Traps : MonoBehaviour
{
    public static event Action OnTrapContact;
    private bool isTriggerActive = true;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("PlayerHitBox") && isTriggerActive)
        {
            isTriggerActive = false;  // Desactiva la trampa para evitar que se dispare nuevamente hasta que el jugador respawnee.
            Debug.Log("Jugador tocó una trampa.");
            OnTrapContact?.Invoke(); // 🔹 Llamamos al evento solo si hay suscriptores
        }
    }
    public void ResetTrap()
    {
        isTriggerActive = true;  // Reactivamos la trampa para que se pueda activar nuevamente
    }
}
