using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue {
    private string _name;
    private string _text;

    public Dialogue(string name, string text) {
        _name = name;
        _text = text;
    }

    public string GETName() {
        return _name;
    }

    public string GETText() {
        return _text;
    }
}
