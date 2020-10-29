using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OverWorldManager : MonoBehaviour
{
    [SerializeField] private List<Transform> stageOneSpawns = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform GetNextSpawn() {
        int random = Random.Range(0, stageOneSpawns.Count);
        Transform spawnPoint = stageOneSpawns[random];
        stageOneSpawns.RemoveAt(random);
        return spawnPoint;
    }
}
