using System.Collections;
using UnityEngine;
using TMPro;

public class StaminaManager : MonoBehaviour
{
    public GameObject staminaNum;
    public float stamina = 10f;
    public float maxStamina = 10f;
    public float ChargeRate = 1f; // Set a default charge rate
    private Coroutine rechargeCoroutine; // Store the reference to the coroutine
    private float lastCardUsedTime; // Track the last time a card was used
    private float rechargeDelay = 1.5f; // Delay before stamina starts recharging

    public void useCard(float staminaCost)
    {
        if (staminaCost <= stamina)
        {
            Debug.Log("Card actually used");
            stamina -= staminaCost;
            lastCardUsedTime = Time.time; // Update the last card used time

            // Stop the current recharge coroutine if it's running
            if (rechargeCoroutine != null)
            {
                StopCoroutine(rechargeCoroutine);
            }

            // Start the recharge coroutine
            rechargeCoroutine = StartCoroutine(RechargeStamina());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (stamina >= 0)
        {
            staminaNum.GetComponent<TMP_Text>().SetText("{}", stamina);
        }
    }

    private IEnumerator RechargeStamina()
    {
        // Wait until 1.5 seconds have passed since the last card was used
        while (Time.time - lastCardUsedTime < rechargeDelay)
        {
            yield return null; // Wait for the next frame
        }

        // Start recharging stamina
        while (stamina < maxStamina)
        {
            stamina += ChargeRate;
            if (stamina > maxStamina)
            {
                stamina = maxStamina; // Ensure stamina does not exceed max
            }

            // Update stamina bar if needed
            yield return new WaitForSeconds(0.5f); 
        }

        rechargeCoroutine = null; // Reset the coroutine reference when done
    }
}
