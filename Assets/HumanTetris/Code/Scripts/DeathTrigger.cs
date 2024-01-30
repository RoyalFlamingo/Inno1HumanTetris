using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    [SerializeField] BoxCollider _collider;
    void Start()
    {

    }

    void Update()
    {
        
    }

	private void OnCollisionEnter(Collision collision)
	{
        if (!collision.gameObject.CompareTag("Player"))
            return;
       
        collision.gameObject.SetActive(false);

        GameManager.Instance.PlayerDeath(collision.gameObject);

	}

}
