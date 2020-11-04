using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class OverWorldManager : MonoBehaviour
{
    [SerializeField] private List<Transform> tutorialSpawns = new List<Transform>();
    [SerializeField] private List<Transform> stageOneSpawns = new List<Transform>();
    [SerializeField] private List<Transform> stageTwoSpawns = new List<Transform>();
    [SerializeField] private List<Transform> stageThreeSpawns = new List<Transform>();
    [SerializeField] private Transform stageOneBossSpawn;
    [SerializeField] private Transform stageTwoBossSpawn;
    [SerializeField] private Transform stageThreeBossSpawn;
    [SerializeField] private List<GameObject> disableList = new List<GameObject>();
    public int stageCount = 0;
    public bool _overworldIsActive = true;
    // Start is called before the first frame update
    void Start() {
        Assert.IsTrue(tutorialSpawns.Count == tutorialSpawns.Distinct().Count(), 
            "Tutorial spawnpoints List, Unique spawnpoints = " + tutorialSpawns.Distinct().Count());
        Assert.IsTrue(stageOneSpawns.Count == stageOneSpawns.Distinct().Count(),
            "Stage One spawnpoints List, Unique spawnpoints = " + stageOneSpawns.Distinct().Count());
        Assert.IsTrue(stageTwoSpawns.Count == stageTwoSpawns.Distinct().Count(),
            "Stage Two spawnpoints List, Unique spawnpoints = " + stageTwoSpawns.Distinct().Count());
        Assert.IsTrue(stageThreeSpawns.Count == stageThreeSpawns.Distinct().Count(),
            "Stage Three spawnpoints List, Unique spawnpoints = " + stageThreeSpawns.Distinct().Count());

        Assert.IsTrue(stageOneSpawns.Count == GameObject.FindWithTag("StageOne").transform.childCount-1,
            "Stage One spawnpoints List, List is missing spawnpoint(s)");
        Assert.IsTrue(stageTwoSpawns.Count == GameObject.FindWithTag("StageTwo").transform.childCount-1,
            "Stage Two spawnpoints List, List is missing spawnpoint(s)");
        Assert.IsTrue(stageThreeSpawns.Count == GameObject.FindWithTag("StageThree").transform.childCount,
            "Stage Three spawnpoints List, List is missing spawnpoint(s)");

        foreach (Transform t in tutorialSpawns) {
            Assert.IsNotNull(t, "Tutorial spawnpoints List, spawnpoint is null");
        }
        foreach (Transform t in stageOneSpawns) {
            Assert.IsNotNull(t, "Stage One spawnpoints List, spawnpoint is null");
        }
        foreach (Transform t in stageTwoSpawns) {
            Assert.IsNotNull(t, "Stage Two spawnpoints List, spawnpoint is null");
        }
        foreach (Transform t in stageThreeSpawns) {
            Assert.IsNotNull(t, "Stage Three spawnpoints List, spawnpoint is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OverworldSetState(bool state) {
        _overworldIsActive = state;
        foreach (GameObject g in disableList) {
            g.SetActive(state);
        }
    }

    public Vector3 GetNextSpawn(int stage) {
        Transform spawnPoint = null;
        if (stageCount < 2) {
            switch (stage) {
                case 0: {
                    // int random = Random.Range(0, tutorialSpawns.Count);
                    spawnPoint = tutorialSpawns[0];
                    tutorialSpawns.RemoveAt(0);
                    break;
                }
                case 1: {
                    int random = Random.Range(0, stageOneSpawns.Count);
                    spawnPoint = stageOneSpawns[random];
                    stageOneSpawns.RemoveAt(random);
                    break;
                }
                case 2: {
                    int random = Random.Range(0, stageTwoSpawns.Count);
                    spawnPoint = stageTwoSpawns[random];
                    stageTwoSpawns.RemoveAt(random);
                    break;
                }
                case 3: {
                    int random = Random.Range(0, stageThreeSpawns.Count);
                    spawnPoint = stageThreeSpawns[random];
                    stageThreeSpawns.RemoveAt(random);
                    break;
                }
            }

            stageCount++;
        } else {
            stageCount = 0;
            switch (stage) {
                case 0: {
                    int random = Random.Range(0, stageOneSpawns.Count);
                    spawnPoint = stageOneSpawns[random];
                    stageOneSpawns.RemoveAt(random);
                    break;
                }
                case 1: {
                    spawnPoint = stageOneBossSpawn;
                    break;
                }
                case 2: {
                    spawnPoint = stageTwoBossSpawn;
                    break;
                }
                case 3: {
                    spawnPoint = stageThreeBossSpawn;
                    break;
                }
            }
        }
        Assert.IsNotNull(spawnPoint, "OverWorldManager: Next spawnpoint is null!");
        return new Vector3(spawnPoint.position.x, spawnPoint.position.y, -5);
    }
}
