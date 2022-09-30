using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    [SerializeField] AudioClip CoinCollecting;
    bool wasCollected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && wasCollected == false)
        {
            Debug.Log("Collected");
            wasCollected = true;
            AudioSource.PlayClipAtPoint(CoinCollecting, Camera.main.transform.position, 1f);
            Destroy(gameObject);
        }
    }

}
