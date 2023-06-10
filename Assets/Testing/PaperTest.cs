using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;
using System;

/*
* BUAT TESTING AJA SORRY
*
*/
public class PaperTest : MonoBehaviour
{
    public int CurrentTypedPosition { get; private set; }
    public string Sentence { get; private set; }
    public event Action SentenceFinished;

    [SerializeField] private TMPro.TMP_Text _textSentence;
    [SerializeField] private SentenceSRO _sentenceSRO;

    SentenceSRO sentenceSRO;

    // Start is called before the first frame update
    void Start() {
        Init(_sentenceSRO);
    }

    // To be called when a new sentence is added. Resets everything and 
    // animates entry
    void Init(SentenceSRO s) {
        CurrentTypedPosition = 0;
        sentenceSRO = s;
        Sentence = _textSentence.text = sentenceSRO.sentence;
    }

    // Get the letter and if correct, advance current typing position
    // Invoke next SentenceFinished if last letter is typed
    bool AdvanceNextLetter(char letter) {
        
        Debug.Log(CurrentTypedPosition);
        Debug.Log("Get Letter " + letter);

        if (letter != Sentence[CurrentTypedPosition])
        {
            return false;
        }

        UpdateDisplayText();
        
        if(CurrentTypedPosition == (Sentence.Length-1))
        {
            Debug.Log("Invoked");
            SentenceFinished.Invoke();
            return true;
        }
        
        CurrentTypedPosition++;
        
        return true;
    }

    void UpdateDisplayText()
    {
        Debug.Log("Changed on index" + CurrentTypedPosition);

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
    void AddExclamationMark(bool festive) {

    }

    // Update is called once per frame
    void Update() {
        CheckInput();  
    }

    void CheckInput()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            AdvanceNextLetter('a');
            Debug.Log("Get a");
        }
        else if(Input.GetKeyDown(KeyCode.B))
        {
            AdvanceNextLetter('b');
            Debug.Log("Get b");
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            AdvanceNextLetter('c');
            Debug.Log("Get c");
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            AdvanceNextLetter('d');
            Debug.Log("Get d");
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            AdvanceNextLetter('e');
            Debug.Log("Get e");
        }
    }
}
