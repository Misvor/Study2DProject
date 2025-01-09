using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{

    public static Main Self;
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;


    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemiesPerSecond = 0.5f;
    public float enemiesDefaultPadding = 1.5f;

    public WeaponDefinition[] weaponDefinitions;

    public GameObject prefabPowerUp;
    public WeaponType[] powerUpFrequency = new WeaponType[]
    {
        WeaponType.blaster, WeaponType.blaster, WeaponType.spread, WeaponType.shield
    };
    private BoundsCheck bounds;

	private void Awake()
	{
        Self = this;

        bounds = GetComponent<BoundsCheck>();

        Invoke("SpawnEnemy", 1f / enemiesPerSecond);

        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (var weapon in weaponDefinitions)
		{
            WEAP_DICT[weapon.type] = weapon;
		}
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

    public void ShipDestroyed(Enemy e)
	{
        if(Random.value <= e.powerUpDropChance)
		{
            var typeOfWeapon = Random.Range(0, powerUpFrequency.Length);
            var puType = powerUpFrequency[typeOfWeapon];

            var go = Instantiate(prefabPowerUp);
            var powerUp = go.GetComponent<PowerUp>();

            powerUp.SetType(puType);
            powerUp.transform.position = e.transform.position;
		}
	}
    public static WeaponDefinition GetWeaponDefinition(WeaponType wt)
	{
        if (WEAP_DICT.TryGetValue(wt, out var result))
            return result;

        return new WeaponDefinition();

    }

    public void DelayedRestart(float delay)
	{
        Invoke("Restart", delay);
	}

    public void Restart()
	{
        //SceneManager.LoadScene("Scene_0");
        SceneManager.LoadScene("_Scene_0");
	}
}
