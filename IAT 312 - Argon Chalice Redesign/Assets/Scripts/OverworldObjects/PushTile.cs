using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushTile : MonoBehaviour {
    private Vector3 _playerStartPos;
    private bool _isPushing = false;
    private const float PullDuration = 0.15f;
    private float _time;
    private void FixedUpdate() {
        if (!_isPushing) return;
        CharacterBehavior player =
            GameObject.FindGameObjectWithTag("PlayerCharacter").GetComponent<CharacterBehavior>();
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, -5);
        PullPlayer(player, targetPos);
        PushPlayer(player, targetPos);
    }

    private void PullPlayer(CharacterBehavior player, Vector3 targetPos) {
        _time += Time.deltaTime / PullDuration;
        player.transform.position = Vector3.Lerp(_playerStartPos, targetPos, _time);
    }

    private void PushPlayer(CharacterBehavior player, Vector3 targetPos) {
        if (player.transform.position != targetPos) return;
        _isPushing = false;
        player.state = CharacterBehavior.State.Pushed;
        player.movementDirection = transform.right;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("PlayerCharacter")) return;
        _playerStartPos = other.transform.position;
        other.GetComponent<CharacterBehavior>().state = CharacterBehavior.State.Pulled;
        _isPushing = true;
        _time = 0;
    }
}
