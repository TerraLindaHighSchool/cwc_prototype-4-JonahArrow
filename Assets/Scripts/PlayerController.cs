using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public GameObject powerUpIndicator;
    public float speed = 5.0f;
    public bool hasPowerUp;
    private float powerUpStrength = 15.0f;

    // Start is called before the first frame update
    void Start()
    {
        powerUpIndicator.gameObject.SetActive(false);
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
        powerUpIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.CompareTag("PowerUp"))
       {
            hasPowerUp = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerUpCountdownRoutine());
            powerUpIndicator.gameObject.SetActive(true);

            IEnumerator PowerUpCountdownRoutine()
            {
                yield return new WaitForSeconds(7);
                hasPowerUp = false;
                powerUpIndicator.gameObject.SetActive(false);
            }
       }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && hasPowerUp)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);
            Debug.Log("Collided with" + collision.gameObject.name + "with powerup set to" + hasPowerUp);
            enemyRigidbody.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);
        }
    }
}
