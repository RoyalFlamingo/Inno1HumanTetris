using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	public void Start()
	{
		OnPlayerJoined();
	}

	public void OnPlayerJoined()
	{
		GameManager.Instance.OnPlayerJoined(gameObject);
    }
}
