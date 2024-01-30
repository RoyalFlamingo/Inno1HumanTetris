using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<GameObject> Players = new List<GameObject>();
    public GameObject StartScreen;
    public TextMeshProUGUI StartTimer;
    public GameObject PlayerJoinManager;
    private Vector3 FirstPlayerPosition = new Vector3(-16, 0.5f, 14.5f);
    private float PlayerSpacing = 4f;

    public int GameStartTime = 5;

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        StartScreen.SetActive(true);
    }

    private void CheckForGameStart()
    {
        if (Players.Count == 1)
        {
            StartTimer.gameObject.SetActive(true);
            StartCoroutine(GameStartTimer());
        }
    }

    private IEnumerator GameStartTimer()
    {
        while (true)
        {
            Debug.Log($"Game starts in {GameStartTime}");
            GameStartTime--;
            StartTimer.text = $"Game starts in {GameStartTime}";
            if (GameStartTime <= 0)
            {
                StartGame();
                break;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private void StartGame()
    {
        PlayerJoinManager.SetActive(false);
        StartScreen.SetActive(false);
        Players.ForEach(p => { p.GetComponent<PlayerMovement>().ActivateInput(); });
        Debug.Log("Game started!");
        ObstacleSpawner.Instance.Runs = true;
        ObstacleSpawner.Instance.SpawnRandomObstacle();
    }

    public void OnPlayerJoined(GameObject player)
    {
        Players.Add(player);
        player.GetComponent<Rigidbody>().MovePosition(FirstPlayerPosition + (Players.Count - 1) * PlayerSpacing * Vector3.back);
        Debug.Log($"Player {Players.Count} joined.");
        CheckForGameStart();
    }

    public void PlayerDeath(GameObject player)
    {
        Debug.Log($"{player.name} fell into the water.");

        Destroy(player);

        //easy solution to restart the game is to reload the scene
        var players = FindObjectsOfType<PlayerMovement>();
        if(players.Length <= 1)
            StartCoroutine(RestartGame(players.Length));
    }

    private IEnumerator RestartGame(int playerCount)
    {
        if(!ObstacleSpawner.Instance.Runs)
            yield break;

        ObstacleSpawner.Instance.Runs = false;

        var obstacles = FindObjectsOfType<MoveObstacle>();
        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }

        var msg = Instantiate(Resources.Load("Prefabs/UI/CenterMessagePrefab")) as GameObject;
        if(playerCount == 1)
            msg.GetComponent<CenterMessageScript>().ShowText("Winner! Game restarting...", 0f);
        else
            msg.GetComponent<CenterMessageScript>().ShowText("Game Over! Game restarting...", 0f);

        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
