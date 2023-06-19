using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleAnimationOnMachChanged : MonoBehaviour
{
    Animator _animator;
    private int _currentMachLevel;
    private float _lockedTill;
    private int _currentState;
    private static readonly int Mach1 = Animator.StringToHash("Mach1");
    private static readonly int Mach2 = Animator.StringToHash("Mach2");
    private static readonly int Mach3 = Animator.StringToHash("Mach3");
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        var state = GetState();
        if (state == _currentState) return;
        _animator.CrossFade(state, 0, 0);
        _currentState = state;
        
    }

    private int GetState()
    {
        if (Time.time < _lockedTill) return _currentState;

        if (_currentMachLevel == 0) return Mach1;
        if (_currentMachLevel == 1) return Mach2;
        if (_currentMachLevel == 2) return Mach3;

        int LockState(int s, float t)
        {
            _lockedTill = Time.time + t;
            return s;
        }

        return Mach1;
    }

    public void SetMachLevel(int machLevel)
    {
        _currentMachLevel = machLevel;
    }
}
