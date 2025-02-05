using UnityEngine;

public class ClaimReward : MonoBehaviour
{
    public float interactionRange = 2f;
    public KeyCode interactionKey = KeyCode.Space;
    private float separationForce = 5f;
    private bool playerInRange = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        playerInRange = Vector3.Distance(transform.position, Camera.main.transform.position) <= interactionRange; 

        if (playerInRange && Input.GetKeyDown(interactionKey))
        {
            Debug.Log("card claimed");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("ChestReward"))
        {
            Vector3 away = transform.position - collision.contacts[0].point;
            away.y = 0;
            away = away.normalized * separationForce;
            
            rb.AddForce(away, ForceMode.Impulse);
        }
    }
}
