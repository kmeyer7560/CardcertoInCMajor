using UnityEngine;
using System.Collections;
using System;
using System.Numerics;

public class ChestInteraction : MonoBehaviour
{
    public float interactionRange = 2f;
    public KeyCode interactionKey = KeyCode.Space;
    public RouletteManager rouletteManager;
    private bool notSpinning = true;

    private bool playerInRange = false;
    public GameObject[] cardPrefabs;
    //private int randomCard = Random.Range(0,cardPrefabs.Length);

    private void Start()
    {
        if (rouletteManager == null)
        {
            rouletteManager = FindObjectOfType<RouletteManager>();
        }
    }

    private void Update()
    {
        //playerInRange = Vector3.Distance(transform.position, Camera.main.transform.position) <= interactionRange; 

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

    public void GiveReward(int value, string suit)
    {
        if(suit == "Clubs")
        {
            for(int i=0; i<value; i++)
            {

                //Instantiate(cardPrefabs[randomCard], new Vector3(0, 0, 0), Quaternion.Identity);
            }
        }
    }
}
