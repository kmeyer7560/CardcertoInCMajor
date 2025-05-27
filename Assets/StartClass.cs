using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartClass : MonoBehaviour
{
    public GameObject violinPickup;
    public GameObject drumPickup;
    public GameObject guitarPickup;
    public GameObject flutePickup;
    public GameObject startClassObj;

    private Timer timer;
    public GameObject player;

    void Awake()
    {
        violinPickup.SetActive(false);
        guitarPickup.SetActive(false);
        flutePickup.SetActive(false);
        drumPickup.SetActive(false);
        timer = GameObject.Find("TimerManager").GetComponent<Timer>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void SelectInstrument(GameObject instrumentPickup)
    {
        violinPickup.SetActive(false);
        drumPickup.SetActive(false);
        guitarPickup.SetActive(false);
        flutePickup.SetActive(false);

        instrumentPickup.SetActive(true);
        startClassObj.SetActive(false);
        timer.startTimer = true;
        player.transform.position = new Vector3(0, 0, 0);
    }

    public void Violin()
    {
        SelectInstrument(violinPickup);
    }

    public void Drum()
    {
        SelectInstrument(drumPickup);
    }

    public void Guitar()
    {
        SelectInstrument(guitarPickup);
    }

    public void Flute()
    {
        SelectInstrument(flutePickup);
    }
}
