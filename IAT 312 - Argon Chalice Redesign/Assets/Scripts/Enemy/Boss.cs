using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private BattleSystem battleSys;
    private EventText eText;

    [SerializeField] private GameObject ballProjectile;

    [SerializeField] private float health;

    void Start()
    {
        battleSys = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();
        eText = GameObject.FindGameObjectWithTag("EventText").GetComponent<EventText>();
    }

    void Update()
    {
        transform.position += new Vector3(0, 0.00025f * Mathf.Sin(Time.time), 0);
    }

    void FixedUpdate() {
        
    }

    public void InflictDamage(float damage) {
        eText.SetText(string.Format("Enemy took {0} damage!", damage));
        health -= damage;
    }

    public void EnemyPhaseAction() {
        // TODO: add more attacks and actions
        StartCoroutine(ProjectileWave());
    }

    IEnumerator ProjectileWave() {
        for (int i = 0; i < 3; i++) {
            yield return new WaitForSeconds(1.5f);

            Instantiate(ballProjectile, transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(2);

        battleSys.StartPlayerPhase();
    }
}
