using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;
using System;

public class Paper : MonoBehaviour {
  public int CurrentTypedPosition { get; private set; }
  public string Sentence { get; private set; }
  public SentenceSRO SentenceSRO { get; private set; }

  public event Action SentenceFinish;

  TMPro.TMP_Text _textSentence;

  void Awake() {
    _textSentence = GetComponentInChildren<TMPro.TMP_Text>();
  }

  // Start is called before the first frame update
  void Start() {

  }

  // To be called when a new sentence is added. Resets everything and 
  // animates entry
  public void Init(SentenceSRO s) {
    CurrentTypedPosition = 0;
    SentenceSRO = s;
    Sentence = _textSentence.text = SentenceSRO.sentence;
  }

  // Get the letter and if correct, advance current typing position
  // Invoke next SentenceFinished if last letter is typed
  public bool AdvanceNextLetter(char letter) {
    if ((CurrentTypedPosition < Sentence.Length) && letter.Equals(Sentence[CurrentTypedPosition])) {
      if (letter != ' ') {
        UpdateDisplayText();
      }

      if (CurrentTypedPosition == (Sentence.Length - 1)) {
        SentenceFinish?.Invoke();
      }

      CurrentTypedPosition++;
      return true;
    }

    return false;
  }

  void UpdateDisplayText() {
    Debug.Log("Current changed char" + _textSentence.textInfo.characterInfo[CurrentTypedPosition].character);
    Color32 _activeColor = Color.green;
    int meshIndex = _textSentence.textInfo.characterInfo[CurrentTypedPosition].materialReferenceIndex;
    int vertexIndex = _textSentence.textInfo.characterInfo[CurrentTypedPosition].vertexIndex;
    Color32[] vertexColors = _textSentence.textInfo.meshInfo[meshIndex].colors32;
    vertexColors[vertexIndex + 0] = _activeColor;
    vertexColors[vertexIndex + 1] = _activeColor;
    vertexColors[vertexIndex + 2] = _activeColor;
    vertexColors[vertexIndex + 3] = _activeColor;

    _textSentence.UpdateVertexData(TMPro.TMP_VertexDataUpdateFlags.All);
  }

  // Add exclamation mark to the end of sentence
  // bool festive: extra visual feedback
  public void AddExclamationMark(bool festive) {

  }

  // Update is called once per frame
  void Update() {

  }
}
