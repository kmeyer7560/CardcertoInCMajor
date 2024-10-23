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
        stamina -= staminaCost;

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
            if (recharge != null)
            {
                StopCoroutine(recharge);
                recharge = StartCoroutine(RechargeStamia());
            }
        }   
    }

    private IEnumerator RechargeStamia()
    {
        yield return new WaitForSeconds(1f);

        while (stamina < maxStamina)
        {
            stamina += ChargeRate / 10f;
            if (stamina >= maxStamina)
            {
                staminaBar.fillAmount = stamina / maxStamina;
                yield return new WaitForSeconds(.1f); 
            }
        }
    }
}
