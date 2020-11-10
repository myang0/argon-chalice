using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    [SerializeField] private GameObject minigameBg;
    private Boss boss;

    private BattlePlayer _player;

    [SerializeField] private GameObject shootingStar;
    [SerializeField] private float maxStarDamage;
    [SerializeField] private float maxStars;
    private float numHitStars = 0;

    public Vector3 size;
    private Vector3 pos;
    private Vector3 _bottomPos;
    private Vector2 halfSize;
    private (float fst, float snd) xBounds;
    private (float fst, float snd) yBounds;

    [SerializeField] private GameObject movingBar;
    [SerializeField] private GameObject barMinigameBg;
    [SerializeField] private float maxBarDamage;
    private bool isBarMinigameActive = false;

    [SerializeField] private GameObject clickHammer;
    [SerializeField] private GameObject targetZone;
    [SerializeField] private float maxClickDamage = 60;
    private float maxClicks = 12;
    private float numClicks = 0;

    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _maxShootDamage;
    [SerializeField] private float _numTargets;
    private bool _isShooting = false;
    private bool _canShoot = true;
    private float _numHitTargets = 0;

    [SerializeField] private GameObject _iPanel;
    [SerializeField] private Text _iText;

    private BattleSystem battleSys;

    void Start() {
        pos = transform.position;

        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<BattlePlayer>();

        halfSize = new Vector2(size.x / 2, size.y / 2);

        xBounds = (pos.x - halfSize.x, pos.x + halfSize.x);
        yBounds = (pos.y - halfSize.y, pos.y + halfSize.y);

        _bottomPos = new Vector3(pos.x, yBounds.fst, pos.z);

        battleSys = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();
    }

    void Update() {
        if (isBarMinigameActive && Input.GetMouseButtonDown(0)) EndBarMinigame();

        if (Input.GetMouseButtonDown(0) && _isShooting && _canShoot) {
            ShootBullet();
            StartCoroutine(ShootCooldown());
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.DrawCube(transform.position, size);
    }

    public void StartStarMinigame() {
        _iPanel.SetActive(true);
        _iText.text = "Click the stars!";
        minigameBg.SetActive(true);
        targetZone.SetActive(false);
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
        _iPanel.SetActive(false);
        minigameBg.SetActive(false);
        DestroyAllStars();
        float dmg = Mathf.Floor((numHitStars / maxStars) * maxStarDamage);
        CalcDamage(dmg);
        numHitStars = 0;

        StartCoroutine(EnemyPhaseTransition());
    }

    public void StartBarMinigame() {
        _iPanel.SetActive(true);
        _iText.text = "Time your click!";
        minigameBg.SetActive(true);
        barMinigameBg.SetActive(true);
        targetZone.SetActive(false);

        GameObject b = Instantiate(movingBar, pos, Quaternion.identity);
        MinigameBar mb = b.GetComponent<MinigameBar>();

        mb.SetXBounds(xBounds);
        mb.SetTarget(pos.x);

        isBarMinigameActive = true;
    }

    private void EndBarMinigame() {
        _iPanel.SetActive(false);
        minigameBg.SetActive(false);
        MinigameBar bar = GameObject.FindGameObjectWithTag("MinigameBar").GetComponent<MinigameBar>();
        float dist = bar.GetDistance();
        Destroy(bar.gameObject);

        float dmg = Mathf.Floor((Mathf.Abs(dist - xBounds.snd) / xBounds.snd) * maxBarDamage);
        CalcDamage(dmg);

        barMinigameBg.SetActive(false);
        isBarMinigameActive = false;

        StartCoroutine(EnemyPhaseTransition());
    }

    public void StartClickMinigame() {
        _iPanel.SetActive(true);
        _iText.text = "Time your clicks!";
        minigameBg.SetActive(true);
        targetZone.SetActive(true);

        StartCoroutine(ClickWave());
    }

    public void ClickSuccess() {
        numClicks++;
    }

    private void EndClickMinigame() {
        _iPanel.SetActive(false);
        minigameBg.SetActive(false);
        targetZone.SetActive(false);

        foreach (GameObject h in GameObject.FindGameObjectsWithTag("MinigameHammer")) {
            Destroy(h);
        }

        if (numClicks > maxClicks) numClicks = maxClicks;

        float dmg = Mathf.Floor((numClicks / maxClicks) * maxClickDamage);
        numClicks = 0;

        CalcDamage(dmg);
        StartCoroutine(EnemyPhaseTransition());
    }

    public void StartShootMinigame() {
        _iPanel.SetActive(true);
        _iText.text = "Shoot the targets!";
        minigameBg.SetActive(true);
        targetZone.SetActive(false);

        _isShooting = true;

        StartCoroutine(TargetWave());
    }

    IEnumerator TargetWave() {
        for (int i = 0; i < _numTargets; i++) {
            SpawnTarget();
        }

        yield return new WaitForSeconds(5);
        EndShootMinigame();
    }

    IEnumerator ShootCooldown() {
        _canShoot = false;
        yield return new WaitForSeconds(0.5f);
        _canShoot = true;
    }

    private void SpawnTarget() {
        Vector3 newPos = new Vector3(
            pos.x + Random.Range(-halfSize.x, halfSize.x),
            pos.y + Random.Range(0, halfSize.y),
            0  
        );

        GameObject t = Instantiate(_target, newPos, Quaternion.identity);
        MinigameTarget mt = t.GetComponent<MinigameTarget>();

        mt.SetXBounds(xBounds);
    }

    private Quaternion GetAngleToMouse() {
        Camera cam = Camera.main;
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 vectorToMouse = (_bottomPos - mousePos).normalized;
        float angle = Mathf.Atan2(vectorToMouse.y, vectorToMouse.x) * Mathf.Rad2Deg;
        Quaternion angleToMouse = Quaternion.AngleAxis(angle + 90, Vector3.forward);
        return angleToMouse;
    }

    private void ShootBullet() {
        GameObject bObject = Instantiate(_bullet, _bottomPos, Quaternion.identity);

        Rigidbody2D rb = bObject.GetComponent<Rigidbody2D>();
        rb.velocity = GetAngleToMouse() * new Vector2(0, 25f);

        MinigameBullet mb = bObject.GetComponent<MinigameBullet>();
        mb.SetBounds(xBounds, yBounds);
    }

    public void HitTarget() {
        _numHitTargets++;
    }

    private void EndShootMinigame() {
        _iPanel.SetActive(false);
        minigameBg.SetActive(false);

        _isShooting = false;

        foreach (GameObject t in GameObject.FindGameObjectsWithTag("Target")) {
            Destroy(t);
        }

        float dmg = (_numHitTargets / _numTargets) * _maxShootDamage;
        CalcDamage(dmg);

        _numHitTargets = 0;

        StartCoroutine(EnemyPhaseTransition());
    }

    private void CalcDamage(float damage) {
        float dmg;
        if (_player.hasRage) {
            float fracLost = Mathf.Abs(_player.health - _player.maxHealth) / _player.maxHealth;
            dmg = Mathf.Floor(damage + ((fracLost) * damage));
        } else {
            dmg = damage;
        }
        
        boss.InflictDamage(dmg);
    }

    IEnumerator StarWave() {
        for (int i = 0; i < maxStars; i++) {
            SpawnStar();
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(2f);
        EndStarMinigame();
    }

    IEnumerator ClickWave() {
        for (int i = 0; i < 8; i++) {
            Instantiate(clickHammer, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.615f);
        }

        yield return new WaitForSeconds(5f);
        EndClickMinigame();
    }

    IEnumerator EnemyPhaseTransition() {
        yield return new WaitForSeconds(2);
        GameObject.FindWithTag("Player").GetComponent<BattlePlayer>()._isAttacking = false;
        battleSys.StartEnemyPhase();
    }
}
