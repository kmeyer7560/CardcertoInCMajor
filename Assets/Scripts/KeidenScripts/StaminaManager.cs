using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
public class StaminaManager : MonoBehaviour
{
    public Image staminaBar;
    public float stamina = 100f;
    public float maxStamina = 100f;
    public float ChargeRate;
    private Coroutine recharge;
    



    public void useCard(float staminaCost)
    {
        ChargeRate = 0;
        StopCoroutine(RechargeStamia());
        if (staminaCost <= stamina)
        {
            Debug.Log("card atually usesd");
            stamina -= staminaCost;
            StartCoroutine(RechargeStamia());
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
            staminaBar.fillAmount = stamina / maxStamina;

        }
         
    }

    private IEnumerator RechargeStamia()
    {

        yield return new WaitForSeconds(1.5f);
        ChargeRate = 10;
        while (stamina < maxStamina && ChargeRate == 10)
        {
            stamina += ChargeRate / 10f;
            if (stamina <= maxStamina)
            {
                staminaBar.fillAmount = stamina / maxStamina;
                yield return new WaitForSeconds(0.2f); 
            }
        }
        recharge = null;
    }
}
