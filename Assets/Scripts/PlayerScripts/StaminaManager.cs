using System.Collections;
using UnityEngine;
using TMPro;

public class StaminaManager : MonoBehaviour
{
    public GameObject staminaNum;
    public int stamina = 10; // Change to int
    public int maxStamina = 10; // Change to int
    public int ChargeRate = 1; // Change to int
    private Coroutine rechargeCoroutine; // Store the reference to the coroutine
    private float lastCardUsedTime; // Track the last time a card was used
    private float rechargeDelay = 1.5f; // Delay before stamina starts recharging
    private bool isRecharging = false; // Flag to indicate if stamina is recharging

    public void useCard(int staminaCost)
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
    else
    {
        Debug.Log("Not enough stamina to use the card.");
    }
}

    // Start is called before the first frame update
    void Start()
    {
        UpdateStaminaDisplay(); // Initialize the display
    }

    // Update is called once per frame
    void Update()
    {
        // Update stamina display
        UpdateStaminaDisplay();
    }

    private void UpdateStaminaDisplay()
    {
        staminaNum.GetComponent<TMP_Text>().SetText("{}", stamina);
    }

    private IEnumerator RechargeStamina()
{
    isRecharging = true; // Set the recharging flag

    // Wait until 1.5 seconds have passed since the last card was used
    yield return new WaitForSeconds(rechargeDelay);

    // Start recharging stamina
    while (stamina < maxStamina)
    {
        stamina += ChargeRate; // Add whole number
        stamina = Mathf.Clamp(stamina, 0, maxStamina); // Ensure stamina does not exceed max

        Debug.Log("Recharging stamina: " + stamina); // Debug log to track stamina

        // Wait for a short duration before the next recharge increment
        yield return new WaitForSeconds(0.5f); 

        // Check if a card has been used again during the recharge
        if (Time.time - lastCardUsedTime < rechargeDelay)
        {
            // If a card was used, stop recharging and restart the coroutine
            yield break; // Exit the coroutine
        }
    }

    isRecharging = false; // Reset the recharging flag
    rechargeCoroutine = null; // Reset the coroutine reference when done
}

}
