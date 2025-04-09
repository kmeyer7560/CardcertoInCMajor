using UnityEngine;
using System.Collections;
using System;

public class ChestInteraction : MonoBehaviour
{
    public float interactionRange = 2f;
    public KeyCode interactionKey = KeyCode.Space;
    public RouletteManager rouletteManager;
    private bool notSpinning = true;
    private int intValue;

    private bool playerInRange = false;
    public GameObject[] guitarCardPrefabs;
    public GameObject[] fluteCardPrefabs;
    public GameObject[] violinCardPrefabs;
    public GameObject[] drumCardPrefabs;
    public GameObject[] specialCardPrefabs;
    public GameObject player;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (rouletteManager == null)
        {
            rouletteManager = FindObjectOfType<RouletteManager>();
        }
    }

    private void Update()
    {   
        playerInRange = Vector2.Distance(transform.position, player.transform.position) <= interactionRange; 
        if (playerInRange && Input.GetKeyDown(interactionKey) && notSpinning)
        {
            StartCoroutine(Spinning(7f));
            rouletteManager.StartSpin();
            Debug.Log("Spin started");
        }
    }
    

    private IEnumerator Spinning(float duration)
    {
        notSpinning = false;
        yield return new WaitForSeconds(duration);
        notSpinning = true;
    }

    public void GiveReward(string value, string suit)
    {
        int intValue = 0;
        if (value != "Ace")
        {
            intValue = int.Parse(value);
            GameObject[] prefabArray = null;

            switch (suit)
            {
                case "Clubs":
                    prefabArray = guitarCardPrefabs;
                    break;
                case "Hearts":
                    prefabArray = fluteCardPrefabs;
                    break;
                case "Spades":
                    prefabArray = violinCardPrefabs;
                    break;
                case "Diamonds":
                    prefabArray = drumCardPrefabs;
                    break;
            }

            for (int i = 0; i < intValue; i++)
            {
                int randomCard = UnityEngine.Random.Range(0, prefabArray.Length);
                GameObject instantiatedCard = Instantiate(prefabArray[randomCard], this.transform.position, Quaternion.identity);
            }
        }
        else
        {
            int randomCard = UnityEngine.Random.Range(0, specialCardPrefabs.Length);
            GameObject instantiatedCard = Instantiate(specialCardPrefabs[randomCard], this.transform.position, Quaternion.identity);
        }
    }
    public void DestroyChest()
    {
        Debug.Log("turnoff chest");
        Debug.Log(gameObject);
        gameObject.SetActive(false);
    }
}
