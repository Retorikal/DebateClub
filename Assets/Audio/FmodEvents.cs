using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FmodEvents : MonoBehaviour
{
    [field: Header("SFX")]
    [field: SerializeField] public EventReference TypeSound { get; private set; }
    [field: SerializeField] public EventReference TypoSound { get; private set; }
    [field: SerializeField] public EventReference SubmitSound { get; private set; }
    [field: SerializeField] public EventReference PunctuationSound { get; private set; }

    [field: Header("Musics")]
    [field: SerializeField] public EventReference MainBGM { get; private set; }
    public static FmodEvents instance
    {
        get;private set;
    }

    private void Awake()
    {
        if (instance !=null )
        { Debug.LogError("Found more than one FMOD Events instance in this scene awikow"); }
        instance = this;
    }
    
}
