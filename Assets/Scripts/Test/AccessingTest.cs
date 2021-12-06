using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SO_Scripts.Managers;
using UnityEngine;
using Random = UnityEngine.Random;


public class AccessingTest : MonoBehaviour {

    [SerializeField] private Transform t;
    private Sequence seq;

    enum State {
        Input,
        Anim,
        Process,
    };

    private void Start() {
        DOTween.Init();
        startseq();
    }

    private int animationCounter;
    private State _currentState;

    private void Update() {
        
        switch (_currentState) {
            //////////////////////////////////////////
            //////////////////////////////////////////
            case State.Input:
                if(Input.GetKeyDown(KeyCode.Space))
                   startseq();
                break;
            //////////////////////////////////////////
            //////////////////////////////////////////
            case State.Anim:
                
                break;
            //////////////////////////////////////////
            //////////////////////////////////////////
            case State.Process:
                CheckExp();
                break;
        }
        
    }

    void Anim_1() {
        seq.Join(t.DOMoveX(4f, 1f));
    }
    
    void Anim_2() {
        seq.Join(t.DOMoveY(4f, 1f));
    }

    void YouCanGetInput() {
        _currentState = State.Input;
    }

    void CheckExp() {
        Debug.Log("cheking");
        int rand = Random.Range(0, 100);

        if (rand < 50) {
            Debug.Log("small");
            YouCanGetInput();
        } else {
            Debug.Log("big");
            startseq();
        }
    }

    void startseq() {
        _currentState = State.Anim;
        
        if (!seq.IsActive()) {
            seq = DOTween.Sequence();
            Anim_1();
            Anim_2();
        }
        
        seq.AppendCallback(CheckExp);
    }
    




}
