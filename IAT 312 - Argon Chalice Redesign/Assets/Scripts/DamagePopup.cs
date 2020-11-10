using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    private Rigidbody2D _rb;
    private TextMesh _tm;

    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _tm = GetComponent<TextMesh>();

        _rb.velocity = new Vector2(Random.Range(-2.5f, 2.5f), Random.Range(5f, 10f));

        StartCoroutine(DestroyDelay());
    }

    void Update() {
        if (transform.localScale.x > 0) {
            transform.localScale = new Vector3(
                transform.localScale.x - 0.0035f,
                transform.localScale.y - 0.0035f,
                transform.localScale.z
            );
        }

        if (_tm.color.a > 0) {
            _tm.color = new Color(
                _tm.color.r,
                _tm.color.g,
                _tm.color.b,
                _tm.color.a - 0.0035f
            );
        }
    }

    public void SetText(string newText) {
        GetComponent<TextMesh>().text = newText;
    }

    IEnumerator DestroyDelay() {
        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }
}
