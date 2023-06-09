using UnityEngine;

[CreateAssetMenu(fileName = "Sentence", menuName = "SRO/Sentence")]
public class SentenceSRO : ScriptableObject {
  public enum Tone {
    AGGRESIVE,
    PERSUASIVE
  }

  public string sentence;
  public Tone tone;
}