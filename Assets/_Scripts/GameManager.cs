using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemy;
    public List<Transform> SpawnPositions;
    private float startInvoke;
    private float repeatRate;

    // Start is called before the first frame update
    void Start()
    {
        startInvoke = Random.Range(3,6);
        StartCoroutine(RandomEnemySpawn());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    //Spawnea Enemigos
    IEnumerator RandomEnemySpawn(){
        while(true){
            yield return new WaitForSeconds(Random.Range(2,6));

            Instantiate(enemy, SpawnPositions[Random.Range(0,SpawnPositions.Count)]);
            Debug.Log("Nuevo enemigo");
        }
    }

    private Transform GenerateSpawnPos(){
        Transform randomPos = SpawnPositions[Random.Range(0,SpawnPositions.Count)];
        return randomPos;
    }
}
