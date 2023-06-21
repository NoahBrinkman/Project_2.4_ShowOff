using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DyingState : PlayerState
{
    
    [SerializeField] private bool enableReviving = true;
    [SerializeField] private int reviveTime = 30;
    [SerializeField] private Slider slider;
    [SerializeField] private KeyCode leftKey = KeyCode.Z;
    [SerializeField] private KeyCode rightKey = KeyCode.X;
    private int _value;
    [HideInInspector] public PlayerState ReturnToState;
    [SerializeField] private TMP_Text timerTexr;
    [SerializeField] private float _secondsUntilTrueDeath;
    [SerializeField] private float _timer;
    [SerializeField] private UnityEvent _onDeath;
    public override void Enter()
    {
        base.Enter();
        _value = 0;
        enableReviving = true;
    }

    public override void Run()
    {
        base.Run();
        _timer += Time.deltaTime;
        if (_timer > _secondsUntilTrueDeath)
        {
            _onDeath?.Invoke();
        }
        if (enableReviving)
        {
            if (Input.GetKeyUp(leftKey) && _value % 2 == 0)
            {
                _value++;

            } else if (Input.GetKeyUp(rightKey) && _value % 2 == 1)
            {
                _value++;
            }
        }

        slider.value = _value;
        if (_value >= reviveTime)
        {
            Debug.LogWarning("YOU REVIVED!!!");
            enableReviving = false;
           StateMachine.Revive();
        }
    }

}
