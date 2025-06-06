using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml.Serialization;

public class PlayerHealthBar : MonoBehaviour
{
    public GameObject guitubble;
    public GameObject vioubble;
    public GameObject drubble;
    public Slider healthSlider;
    public float defense;
    private bool deflectActive;
    public int deflectedNum;
    public GameObject player;
    private SpriteRenderer renderer;
    public GameObject healthBall;
    Transform healthBallTransform;
    public GameObject deathScreenObj;
    [SerializeField] public RawImage deathScreen;
    [SerializeField] private float fadeDuration = 5f;
    double currHealth;

    public Animator anim;

    public PlayerMovement playerMovement;
    bool playingDeathSequence;
    public Timer timer;

    void Awake()
    {
        currHealth = 100f;
    }
    void Start()
    {
        deathScreen = GameObject.Find("DSBG").GetComponent<RawImage>();
        Color fadecolor = deathScreen.color;
        fadecolor.a = 0f;
        deathScreen.color = fadecolor;

        healthBallTransform = healthBall.GetComponent<RectTransform>();
        vioubble.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        renderer = player.GetComponentInChildren<SpriteRenderer>();
        anim = player.GetComponentInChildren<Animator>();
        deathScreenObj.SetActive(false);
        playerMovement = player.GetComponent<PlayerMovement>();

        playerMovement.canMove = true;
        timer = GameObject.Find("TimerManager").GetComponent<Timer>();
    }

    void Update()
    {
        //if(deathScreenObj == null){Debug.Log("null");}
        if(healthSlider.value<=0 && !playingDeathSequence)
        {
            DeathSequence();
            Debug.Log("playerdeath");

            playingDeathSequence = true;
        }
    }

    public void SetSlider(float amount)
    {
        healthSlider.value = amount;
    }

    public void SetSliderMax(float amount)
    {
        healthSlider.maxValue = amount;
        SetSlider(amount);
    }

    public void TakeDamage(float amount)
    {
        if (deflectActive)
        {
            amount = 0;
            deflectedNum++; 
        }
        healthBallTransform.position += new Vector3(0f, -amount/110,0f);
        currHealth -= amount*.6;
        healthSlider.value -= (amount - defense);
        StartCoroutine(DmgFlash());
    }

    IEnumerator DmgFlash()
    {
        renderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        renderer.color = Color.white;
    }

    public void Heal(float amount, int i)
    {
        Debug.Log("player healed");
        healthSlider.value += (amount * i);
        healthBallTransform.position += new Vector3(0f, 1-amount/110,0f);
        currHealth += amount*.6;
    }

    public void setDefense(float value, int i)
    {
        if (i == 1)
        {
            GameObject instantiateGuitubble = Instantiate(guitubble, new Vector3(player.transform.position.x-.3f, player.transform.position.y, player.transform.position.z), transform.rotation);
            instantiateGuitubble.transform.SetParent(player.transform);
        }
        else if (i == 2)
        {
            GameObject instantiateDrubble = Instantiate(drubble, new Vector3(player.transform.position.x-.3f, player.transform.position.y, player.transform.position.z), transform.rotation);
            instantiateDrubble.transform.SetParent(player.transform);
        }
        defense = value;
        StartCoroutine(defenseReset());
    }

    public void deflect(Action<int> callback)
    {
        GameObject instantiateVioubble = Instantiate(vioubble, new Vector3(player.transform.position.x-.3f, player.transform.position.y, player.transform.position.z), transform.rotation);
        instantiateVioubble.transform.SetParent(player.transform);
        deflectActive = true;
        StartCoroutine(reflectRoutine(callback));
    }

    IEnumerator defenseReset()
    {
        yield return new WaitForSeconds(1f);
        defense = 0;
        drubble.SetActive(false);
    }

    IEnumerator reflectRoutine(Action<int> callback)
    {
        yield return new WaitForSeconds(1f);
        deflectActive = false;
        vioubble.SetActive(false);
        callback(deflectedNum); // Call the callback with the deflectedNum
    }
    
    void DeathSequence()
    {
        playerMovement.canMove = false;
        anim.SetTrigger("dead");
        timer.StopTimer();
        StartCoroutine(FadeIn());
        deathScreenObj.SetActive(true);
    }
    IEnumerator FadeIn()
{
    deathScreen.enabled = true;
    float elapsed = 0f;
    Color fadeColor = deathScreen.color;
    
    while (elapsed < fadeDuration)
    {
        elapsed += Time.deltaTime;
        fadeColor.a = Mathf.Clamp01(elapsed / fadeDuration) * 0.75f;
        deathScreen.color = fadeColor;
        yield return null;
    }
    fadeColor.a = 0.75f;
    deathScreen.color = fadeColor;
}

    public void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    
}
