using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{

    public static Main Self;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemiesPerSecond = 0.5f;
    public float enemiesDefaultPadding = 1.5f;

    private BoundsCheck bounds;

	private void Awake()
	{
        Self = this;

        bounds = GetComponent<BoundsCheck>();

        Invoke("SpawnEnemy", 1f / enemiesPerSecond);
	}
	
    public void SpawnEnemy()
	{
        int type = Random.Range(0, prefabEnemies.Length);

        GameObject newBorn = Instantiate<GameObject>(prefabEnemies[type]);

        float enemyPadding = enemiesDefaultPadding;
        if(newBorn.GetComponent<BoundsCheck>() != null)
		{
            enemyPadding = Mathf.Abs(newBorn.GetComponent<BoundsCheck>().radius);
		}

        var position = Vector3.zero;

        float xMin = -bounds.camWidth + enemyPadding;
        float xMax = bounds.camWidth - enemyPadding;

        position.x = Random.Range(xMin, xMax);
        position.y = bounds.camHeight + enemyPadding;

        newBorn.transform.position = position;

        var enemySpawnRate = Random.Range(0.7f * enemiesPerSecond, 1.5f * enemiesPerSecond);

        Invoke("SpawnEnemy", 1f / enemySpawnRate);
    
    }   

    public void DelayedRestart(float delay)
	{
        Invoke("Restart", delay);
	}

    public void Restart()
	{
        //SceneManager.LoadScene("Scene_0");
        SceneManager.LoadScene("SampleScene");
	}
}
