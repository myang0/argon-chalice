using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour {
    [SerializeField] private Image redScreenImage;
    [SerializeField] private float _opacityValue = 0;
    [SerializeField] private Image blackTextBoxImage;
    [SerializeField] private Text blackTextBoxText;
    [SerializeField] private CanvasGroup group;
    [SerializeField] private Button refuseButton;
    [SerializeField] private Button acceptButton;
    
    // Start is called before the first frame update
    public enum State {
        Disabled, Heartbeat, StartDeath, ReverseDeath, Suspended, GameOver
    }

    public bool _opacityRising = false;
    public State state;
    private bool _disableButtons = false;

    private readonly string _firstDeath =
        "It seems you've fallen to the terrors of the dungeon. For a small price, you" +
        " can once again roam the land of the living. Do you accept?";

    private readonly string _secondDeath =
        "And so you have died yet once again. The price remains the same. A fragment of your" +
        " humanity for another life. Do you accept?";

    private readonly string _thirdDeath =
        "I see that you've become rather well acquainted with Death. Will you continue to " +
        " forsake your humanity? My offer still stands.";

    private readonly string _respawnText = "An excellent choice. It is one you shall not regret. Let the " +
                                           "energy of the Argon Chalice flow through your veins, mortal.";
    
    private readonly string _refuseText = "Very well. If you're that desperate to hold onto your humanity," +
                                          " then perish.";
    void Start()
    {
        // refuseButton.onClick.AddListener(RefuseDeal);
        // acceptButton.onClick.AddListener(AcceptDeal);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state) {
            case State.Heartbeat:
                HeartbeatEffect();
                break;
            case State.Disabled:
                _opacityValue = 0;
                _opacityRising = false;
                break;
            case State.StartDeath:
                StartRed();
                SetBlackBoxOpacity();
                break;
            case State.ReverseDeath:
                EndRed();
                break;
            case State.GameOver:
                GameOverRed();
                break;
        }
        SetRedScreenOpacity();
        group.alpha = _opacityValue;

        if ((state == State.StartDeath && _opacityValue > 0.5f) || state == State.Suspended
            || state == State.GameOver) {
            GameManager gameManager = GameManager.GetInstance();
            switch (gameManager.deathCount) {
                case 0:
                    if (blackTextBoxText.text != _respawnText
                        && blackTextBoxText.text != _refuseText) blackTextBoxText.text = _firstDeath;
                    break;
                case 1:
                    if (blackTextBoxText.text != _respawnText
                        && blackTextBoxText.text != _refuseText) blackTextBoxText.text = _secondDeath;
                    break;
                default: {
                    if (gameManager.deathCount > 1) {
                        if (blackTextBoxText.text != _respawnText
                        && blackTextBoxText.text != _refuseText) blackTextBoxText.text = _thirdDeath;
                    }
                    break;
                }
            }
            
            blackTextBoxImage.gameObject.SetActive(true);
            if (_disableButtons) return;
            acceptButton.gameObject.SetActive(true);
            refuseButton.gameObject.SetActive(true);
        } else {
            blackTextBoxImage.gameObject.SetActive(false);
            acceptButton.gameObject.SetActive(false);
            refuseButton.gameObject.SetActive(false);
        }
    }

    private void GameOverRed() {
        _opacityValue += 0.00075f;
    }

    public void RefuseDeal() {
        Debug.Log("Refused");
        acceptButton.gameObject.SetActive(false);
        refuseButton.gameObject.SetActive(false);
        blackTextBoxText.text = _refuseText;
        _disableButtons = true;
        state = State.GameOver;
        StartCoroutine(LoadGameOver());
    }

    private IEnumerator LoadGameOver() {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Scenes/IntroScene");
    }

    public void AcceptDeal() {
        Debug.Log("Accepted");
        acceptButton.gameObject.SetActive(false);
        refuseButton.gameObject.SetActive(false);
        blackTextBoxText.text = _respawnText;
        _disableButtons = true;
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn() {
        yield return new WaitForSeconds(3);
        state = State.ReverseDeath;
    }

    private void SetBlackBoxOpacity() {
        Color color;
        color = blackTextBoxImage.color;
        color = new Color(color.r, color.g, color.b, _opacityValue);
        blackTextBoxImage.color = color;
    }

    private void SetRedScreenOpacity() {
        var color = redScreenImage.color;
        color = new Color(color.r, color.g, color.b, _opacityValue);
        redScreenImage.color = color;
    }

    private void EndRed() {
        if (!(_opacityValue > 0f)) return;
        _opacityValue -= 0.001f;
        if (_opacityValue < 0f) {
            _opacityValue = 0f;
            state = State.Disabled;
            _disableButtons = false;
            blackTextBoxText.text = "";
            GameManager.GetInstance().deathCount++;
            GameManager.GetInstance().humanityValue -= 50;
            GameObject.FindWithTag("Player").GetComponent<BattlePlayer>().Respawn();
        }
    }

    private void StartRed() {
        float max = 0.85f;
        if (!(_opacityValue < max)) return;
        _opacityValue += 0.001f;
        if (_opacityValue > max) {
            _opacityValue = max;
            state = State.Suspended;
        }
    }

    private void HeartbeatEffect() {
        if (_opacityRising) {
            _opacityValue += 0.002f;
            if (_opacityValue > 0.4f) {
                _opacityRising = false;
            }
        } else {
            _opacityValue -= 0.002f;
            if (_opacityValue < 0f) {
                _opacityRising = true;
            }
        }
    }
}
