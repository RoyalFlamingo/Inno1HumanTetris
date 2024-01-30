using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public static ObstacleSpawner Instance { get; private set; }
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

	public int SpawnMediumOnWave = 5;
	public int SpawnHardOnWave = 10;
	public GameObject obstacleMaster;
    private GameObject[] _obstaclePrefabs;
    private List<GameObject> _currentObstacles;
    private List<GameObject> _easyObstacles = new List<GameObject>();
    private List<GameObject> _mediumObstacles = new List<GameObject>();
    private List<GameObject> _hardObstacles = new List<GameObject>();
    public float spawnPosX = 0f;
    public float spawnPosY = 0f;
    public float spawnPosZ = 0f;
    public float obstacleSpeed = 25f;
    public float spawnInterval=3f;
    private int waveNumber = 0;
    public bool Runs { get; set; }

    void Start()
    {
        _obstaclePrefabs = Resources.LoadAll<GameObject>("Prefabs/Obstacles/Obstacles");

        foreach(GameObject obj in _obstaclePrefabs)
        {
            if (obj.CompareTag("EasyObstacle"))
            {
                _easyObstacles.Add(obj);
            }
            else if(obj.CompareTag("MediumObstacle"))
            {
                _mediumObstacles.Add(obj);
            }
            else if(obj.CompareTag("HardObstacle"))
            {
                _hardObstacles.Add(obj);
            }
        }
        _currentObstacles = _easyObstacles;
    }

    public void SpawnRandomObstacle()
    {
        if (!Runs)
            return;

        if(waveNumber == SpawnMediumOnWave)
        {
            _currentObstacles.AddRange(_mediumObstacles);
			Debug.Log("Medium obstacles added!");
		}
        else if(waveNumber == SpawnHardOnWave)
        {
			_currentObstacles.AddRange(_hardObstacles);
			Debug.Log("Hard obstacles added!");
		}

        int index = UnityEngine.Random.Range(0, _currentObstacles.Count);

        //instantiate master object and set speed
        GameObject masterObstacle = Instantiate(obstacleMaster, new Vector3(spawnPosX, spawnPosY, spawnPosZ), Quaternion.identity);
        masterObstacle.gameObject.GetComponent<MoveObstacle>().speed = obstacleSpeed;

        Transform transform = masterObstacle.transform;

		if (UnityEngine.Random.value > 0.5f)
        {
            Debug.Log("Obstacle Flip!");
            transform.transform.localScale = new Vector3(transform.transform.localScale.x, transform.transform.localScale.y, transform.transform.localScale.z*-1);
            transform.transform.localPosition = new Vector3(transform.transform.localPosition.x, transform.transform.localPosition.y, transform.transform.localPosition.z+7);
		}

        Instantiate(_currentObstacles[index], transform);

        //increase obstacle speed in fixed wave intervals
        if(waveNumber%3 == 0)
        {
            obstacleSpeed = Mathf.RoundToInt(obstacleSpeed*1.33f);
            spawnInterval = Mathf.Clamp(spawnInterval-0.25f, 1.5f, float.MaxValue);
        }
        waveNumber++;
        Invoke("SpawnRandomObstacle", spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
