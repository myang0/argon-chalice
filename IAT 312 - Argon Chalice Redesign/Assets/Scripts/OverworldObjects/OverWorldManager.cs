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
    [SerializeField] public List<BaseChamber> chambers = new List<BaseChamber>();
    [SerializeField] public EnemyOverworld boss;
    public AudioSource audioSource;
    [SerializeField] private AudioClip tutorialBgm;
    [SerializeField] private AudioClip stageOneBgm;
    [SerializeField] private AudioClip stageTwoBgm;
    [SerializeField] private AudioClip stageThreeBgm;
    public int stageCount = 0;
    public int stageNumber = 0;
    public bool isBossStage = false;
    public bool _overworldIsActive = true;

    private bool _ended = false;
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
        Assert.IsTrue(stageThreeSpawns.Count == GameObject.FindWithTag("StageThree").transform.childCount-1,
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
        audioSource.clip = tutorialBgm;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (boss.isCompleted && !_ended) {
            if (GameManager.GetInstance().humanityValue > -1) {
                GameObject.FindWithTag("Menu").GetComponent<Menu>().GoodEnd();
            } else {
                GameObject.FindWithTag("Menu").GetComponent<Menu>().BadEnd();
            }

            _ended = true;
        }
    }

    public void ResetChamber() {
        foreach (BaseChamber c in chambers) {
            if (c.GetChamberIsActive()) {
                c.ResetChamberObjects();
            }
        }
    }

    public BaseChamber GetCurrentChamber() {
        foreach (BaseChamber c in chambers) {
            if (c.GetChamberIsActive()) {
                return c;
            }
        }

        return null;
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
                    PlayIfNewStage(stageTwoBgm);
                    break;
                }
                case 3: {
                    int random = Random.Range(0, stageThreeSpawns.Count);
                    spawnPoint = stageThreeSpawns[random];
                    stageThreeSpawns.RemoveAt(random);
                    PlayIfNewStage(stageThreeBgm);
                    break;
                }
            }

            isBossStage = false;
            stageCount++;
            // if (stageCount == -1) stageCount = 0;
            // if (stageCount == 0) stageCount = 1;
            // if (stageCount == 1) stageCount = 2;
            // if (stageCount == 2) stageCount = 3;
        } else {
            switch (stage) {
                case 0: {
                    int random = Random.Range(0, stageOneSpawns.Count);
                    spawnPoint = stageOneSpawns[random];
                    stageOneSpawns.RemoveAt(random);
                    stageNumber = 1;
                    stageCount = 0;
                    PlayIfNewStage(stageOneBgm);
                    break;
                }
                case 1: {
                    spawnPoint = stageOneBossSpawn;
                    isBossStage = true;
                    stageNumber = 2;
                    stageCount = -1;
                    break;
                }
                case 2: {
                    spawnPoint = stageTwoBossSpawn;
                    isBossStage = true;
                    stageNumber = 3;
                    stageCount = -1;
                    break;
                }
                case 3: {
                    spawnPoint = stageThreeBossSpawn;
                    stageNumber = 4;
                    isBossStage = true;
                    break;
                }
            }
        }
        Assert.IsNotNull(spawnPoint, "OverWorldManager: Next spawnpoint is null!");
        Debug.Log("STAGE: " + stageNumber + "---" + "CHAMBER: " + stageCount);
        return new Vector3(spawnPoint.position.x, spawnPoint.position.y, -5);
    }

    private void PlayIfNewStage(AudioClip clip) {
        if (stageCount == -1 || (stageCount == 0 && stageNumber == 1)) {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
