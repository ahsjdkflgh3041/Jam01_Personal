using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HumanAnimating : MonoBehaviour
{
    public GameObject player;
    JunglePlayerController controller;

    Animator animatior;
    SpriteRenderer spriteRenderer;

    float horizonInput; 

    int stateNum = 3;
    bool[] isStates;

    bool isStanding = true;
    bool isJumping = false;
    bool isWalking = false;
    // Start is called before the first frame update
    void Start()
    {
        isStates = new bool[stateNum];
        animatior = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        controller = player.GetComponent<JunglePlayerController>();
        setState(AnimatingState.Stand);
    
    }

    // Update is called once per frame
    void Update()
    {
        if(controller.velocity.x != 0){
            if(controller.velocity.x > 0) spriteRenderer.flipX = false;
            else spriteRenderer.flipX = true;

            if(!isJumping && controller.OnGround){
                setState(AnimatingState.Walk);
                isStanding = false;
            } 
        }
        else if(!isJumping && controller.velocity.y == 0){
            setState(AnimatingState.Stand);
        }

        if(!isJumping && controller.velocity.y >= 1) {
            setState(AnimatingState.Jump);
            isJumping = true;
            isStanding = false;
        }
        else if(isJumping && controller.velocity.y == 0){
            setState(AnimatingState.None);
            isJumping = false;
            isStanding = true;
        }

    }

    void setState(AnimatingState state){
        for(int i = 0;i < stateNum; ++i){
            isStates[i] = false;
        }
        if(state != AnimatingState.None){
            isStates[(int)state] = true;
        }
        for(int i = 0;i < stateNum; ++i){
            animatior.SetBool(Enum.GetName(typeof(AnimatingState),i), isStates[i]);
        }
    }
}
