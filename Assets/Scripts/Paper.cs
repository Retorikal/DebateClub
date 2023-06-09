using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;
using System;

public class Paper : MonoBehaviour {
  public int CurrentTypedPosition { get; private set; }
  public string Sentence { get; private set; }
  public event Action SentenceFinished;

  SentenceSRO sentenceSRO;

  // Start is called before the first frame update
  void Start() {

  }

  // To be called when a new sentence is added. Resets everything and 
  // animates entry
  void Init(SentenceSRO s) {
    sentenceSRO = s;

  }

  // Get the letter and if correct, advance current typing position
  // Invoke next SentenceFinished if last letter is typed
  bool AdvanceNextLetter(char letter) {
    return true;
  }

  // Add exclamation mark to the end of sentence
  // bool festive: extra visual feedback
  void AddExclamationMark(bool festive) {

  }

  // Update is called once per frame
  void Update() {

  }
}
