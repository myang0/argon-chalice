using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    public enum MenuState {
        Active, Paused, Dialogue
    }
    public MenuState state = MenuState.Dialogue;
    [SerializeField] private Image uiShadow;
    [SerializeField] private GameObject menu;
    [SerializeField] private Text healthValue;
    [SerializeField] private GameObject chatbox;
    [SerializeField] private Text chatboxName;
    [SerializeField] private Text chatboxSpeech;
    private List<Dialogue> _introDialogue = new List<Dialogue>();
    private List<Dialogue> _firstPuzzleDialogue = new List<Dialogue>();
    private List<Dialogue> _firstEnemyDialogue = new List<Dialogue>();
    private List<Dialogue> _actualStartDialogue = new List<Dialogue>();
    private List<Dialogue> _goodEndDialogue = new List<Dialogue>();
    private List<Dialogue> _badEndDialogue = new List<Dialogue>();
    private List<List<Dialogue>> _dialogueList = new List<List<Dialogue>>();
    [SerializeField] private int _dialogueIndex = 0;
    [SerializeField] private int _lineIndex = 0;
    private bool _isEnding = false;
    
    // Start is called before the first frame update
    void Start() {
        AddDialogues();
        chatboxName.text = _dialogueList[_dialogueIndex][_lineIndex].GETName();
        chatboxSpeech.text = _dialogueList[_dialogueIndex][_lineIndex].GETText();
        StartCoroutine(BeginDialogueDelay());
    }

    private IEnumerator BeginDialogueDelay() {
        yield return new WaitForSeconds(0.0001f);
        state = MenuState.Dialogue;
    }

    private void AddDialogues() {
        _introDialogue.Add(new Dialogue("NARRATOR", 
            "A swashbuckling adventurer whose thirst for treasure knows no bounds enters " +
            "an ancient crypt in search of the rumored Argon Chalice."));
        _introDialogue.Add(new Dialogue("NARRATOR",
            "Will this be a tale of tragedy about an adventurer who perished to the denizens " +
            "of the mysterious crypt and its enigmatic puzzles? Or will it be a story of awe and fortune?"));
        _firstPuzzleDialogue.Add(new Dialogue("NARRATOR",
            "In order our beloved adventurer to make their way through the crypt, they must " +
            "solve a series of brain obliterating puzzles to reach and unlock the chamber gates!"));
        _firstEnemyDialogue.Add(new Dialogue("NARRATOR", 
            "However puzzles are not the only dangers of the crypt for monsters corrupted by " +
            "the Argon Chalice's energies also guard the gates. Does our dear adventurer even stand " +
            "a chance against these dreadful otherworldly amalgamations?"));
        _actualStartDialogue.Add(new Dialogue("NARRATOR", 
            "Our adventurer's true journey will begin beyond those gates. The ever shifting " +
            "chambers of the crypt will lead our adventurer into unforeseeable challenges, " +
            "unbelievable monsters, and mindblowingly mindblowing puzzles."));
        
        _goodEndDialogue.Add(new Dialogue("NARRATOR",
            "Our adventurer has obtained the Argon Chalice and defeated the manifestations of its " +
            "corrupt energies. No matter the hardships that our adventurer has endured, their humanity " +
            "still remains whole and pure despite the ravenous dark energies of the Chalice."));
        _goodEndDialogue.Add(new Dialogue("NARRATOR",
            "With the legendary Argon Chalice in hand, our adventurer leaves the dungeon and " +
            "returns home with a priceless treasure that not even the richest nobles can imagine " +
            "possessing. The tale of our fearless adventurer and their triumphs will be forever " +
            "recorded in the archives of history."));
        _badEndDialogue.Add(new Dialogue("NARRATOR",
            "Although our adventurer has obtained the Argon Chalice and defeated the manifestations " +
            "of its corrupt energies, their humanity had been lost to the Chalice. With no human soul left, " +
            "the energy of the Argon Chalice claimed its new host."));
        _badEndDialogue.Add(new Dialogue("NARRATOR",
            "And so, our adventurer's tale meets a tragic end as they become nothing more than one " +
            "of the many victims of the Argon Chalice."));
        _dialogueList.Add(_introDialogue);
        _dialogueList.Add(_firstPuzzleDialogue);
        _dialogueList.Add(_firstEnemyDialogue);
        _dialogueList.Add(_actualStartDialogue);
        _dialogueList.Add(_goodEndDialogue);
        _dialogueList.Add(_badEndDialogue);
    }

    // Update is called once per frame
    void Update()
    {
        if (state != MenuState.Dialogue) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                state = (state == MenuState.Paused) ? MenuState.Active : MenuState.Paused;
            } 
            healthValue.text = "" + GameManager.GetInstance().health;
        } else {
            if (Input.GetKeyDown(KeyCode.Space)) {
                LoadNextLine();
            } else if (Input.GetKeyDown(KeyCode.Return)) {
                _lineIndex += _dialogueList[_dialogueIndex].Count;
                LoadNextLine();
            }
        }
        SetMenu();
    }

    public void GoodEnd() {
        _dialogueIndex = _dialogueList.Count - 2;
        _lineIndex = 0;
        state = MenuState.Dialogue;
        _isEnding = true;
        LoadNextLine();
    }

    public void BadEnd() {
        _dialogueIndex = _dialogueList.Count - 1;
        _lineIndex = 0;
        state = MenuState.Dialogue;
        _isEnding = true;
        LoadNextLine();
    }

    public void LoadNextDialogue() {
        _dialogueIndex++;
        state = MenuState.Dialogue;
    }

    private void LoadNextLine() {
        if (_lineIndex < _dialogueList[_dialogueIndex].Count) {
            chatboxName.text = _dialogueList[_dialogueIndex][_lineIndex].GETName();
            chatboxSpeech.text = _dialogueList[_dialogueIndex][_lineIndex].GETText();
            _lineIndex++;
        } else {
            if (!_isEnding) {
                _lineIndex = 0;
                ResumeApp();  
            } else {
                SceneManager.LoadScene("Scenes/IntroScene");
            }
        }
    }

    private void SetMenu() {
        if (state == MenuState.Paused) {
            Time.timeScale = 0;
            uiShadow.enabled = true;
            menu.SetActive(true);
            chatbox.SetActive(false);
        } else if (state == MenuState.Active) {
            Time.timeScale = 1;
            uiShadow.enabled = false;
            menu.SetActive(false);
            chatbox.SetActive(false);
        } else if (state == MenuState.Dialogue) {
            Time.timeScale = 0;
            uiShadow.enabled = true;
            menu.SetActive(false);
            chatbox.SetActive(true);
        }
    }

    public void QuitApp() {
        Application.Quit();
    }

    public void ResumeApp() {
        state = MenuState.Active;
        SetMenu();
    }
}
