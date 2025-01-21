using UnityEngine;
using System.Collections;

public class ChestInteraction : MonoBehaviour
{
    public float interactionRange = 2f;
    public KeyCode interactionKey = KeyCode.Space;
    public RouletteManager rouletteManager;
    private bool notSpinning = true;

    private bool playerInRange = false;

    private void Start()
    {
        if (rouletteManager == null)
        {
            rouletteManager = FindObjectOfType<RouletteManager>();
        }
    }

    private void Update()
    {
        playerInRange = Vector3.Distance(transform.position, Camera.main.transform.position) <= interactionRange; 

        if (playerInRange && Input.GetKeyDown(interactionKey) && notSpinning)
        {
            if (rouletteManager != null)
            {
                rouletteManager.StartSpin();
                StartCoroutine(Spinning(7f));
                Debug.Log("Spin started");
            }
        }
    }

    private IEnumerator Spinning(float duration)
    {
        notSpinning = false;
        yield return new WaitForSeconds(duration);
        notSpinning = true;
    }
}
