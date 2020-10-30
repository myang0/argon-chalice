using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    [SerializeField] private GameObject minigameBg;
    private Boss boss;

    [SerializeField] private GameObject shootingStar;
    [SerializeField] private float maxStarDamage;
    [SerializeField] private float numSpawnedStars;
    private float numHitStars = 0;

    public Vector3 size;
    private Vector3 pos;
    private Vector2 halfSize;
    private (float fst, float snd) xBounds;
    private (float fst, float snd) yBounds;

    [SerializeField] private GameObject movingBar;
    [SerializeField] private GameObject barMinigameBg;
    [SerializeField] private float maxBarDamage;
    private bool isBarMinigameActive = false;

    private BattleSystem battleSys;

    void Start() {
        pos = transform.position;

        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();

        halfSize = new Vector2(size.x / 2, size.y / 2);

        xBounds = (pos.x - halfSize.x, pos.x + halfSize.x);
        yBounds = (pos.y - halfSize.y, pos.y + halfSize.y);

        battleSys = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();
    }

    void Update() {
        if (isBarMinigameActive && Input.GetMouseButtonDown(0)) {
            EndBarMinigame();
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.DrawCube(transform.position, size);
    }

    public void StartStarMinigame() {
        minigameBg.SetActive(true);
        StartCoroutine(StarWave());
    }

    private void SpawnStar() {
        Vector3 newPos = new Vector3(
            pos.x + Random.Range(-halfSize.x, halfSize.x),
            pos.y + Random.Range(-halfSize.y, halfSize.y),
            0  
        );

        GameObject s = Instantiate(shootingStar, newPos, Quaternion.identity);
        MinigameStar ms = s.GetComponent<MinigameStar>();

        ms.SetXBounds(xBounds);
    }

    public void HitStar(GameObject star) {
        numHitStars++;
        Destroy(star);
    }

    private void DestroyAllStars() {
        foreach (GameObject s in GameObject.FindGameObjectsWithTag("MinigameStar")) {
            Destroy(s);
        }
    }

    private void EndStarMinigame() {
        minigameBg.SetActive(false);
        DestroyAllStars();
        boss.InflictDamage(Mathf.Floor((numHitStars / numSpawnedStars) * maxStarDamage));
        numHitStars = 0;

        StartCoroutine(EnemyPhaseTransition());
    }

    public void StartBarMinigame() {
        minigameBg.SetActive(true);
        barMinigameBg.SetActive(true);

        GameObject b = Instantiate(movingBar, pos, Quaternion.identity);
        MinigameBar mb = b.GetComponent<MinigameBar>();

        mb.SetXBounds(xBounds);
        mb.SetTarget(pos.x);

        isBarMinigameActive = true;
    }

    private void EndBarMinigame() {
        minigameBg.SetActive(false);
        MinigameBar bar = GameObject.FindGameObjectWithTag("MinigameBar").GetComponent<MinigameBar>();
        float dist = bar.GetDistance();
        Destroy(bar.gameObject);

        float dmg = Mathf.Floor((Mathf.Abs(dist - xBounds.snd) / xBounds.snd) * maxBarDamage);
        boss.InflictDamage(dmg);

        barMinigameBg.SetActive(false);
        isBarMinigameActive = false;

        StartCoroutine(EnemyPhaseTransition());
    }

    IEnumerator StarWave() {
        for (int i = 0; i < numSpawnedStars; i++) {
            SpawnStar();
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(2f);
        EndStarMinigame();
    }

    IEnumerator EnemyPhaseTransition() {
        yield return new WaitForSeconds(2);

        battleSys.StartEnemyPhase();
    }
}
