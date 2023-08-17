using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Rendering;

public class RhinoAnimating : MonoBehaviour
{
    
    public GameObject player;
    JunglePlayerController controller;
    Animator animatior;
    SpriteRenderer spriteRenderer;

    float horizonInput; 

    int stateNum = 4;
    bool[] isStates;

    bool isStanding = true;
    bool isJumping = false;
    bool isWalking = false;
    bool isDashing = false;
    AnimatingState currentState;
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
        Debug.Log(controller.OnGround);
        if(controller.velocity.x != 0){
            if(controller.velocity.x > 0) spriteRenderer.flipX = false;
            else spriteRenderer.flipX = true;

            if(currentState == AnimatingState.Stand && controller.OnGround){
                setState(AnimatingState.Walk);
            } 
        }
        else if(currentState != AnimatingState.Stand && controller.velocity.y == 0){
            setState(AnimatingState.Stand);
        }

        if(currentState !=  AnimatingState.Jump && controller.velocity.y >= 1) {
            setState(AnimatingState.Jump);
        }
        else if(currentState ==  AnimatingState.Jump && controller.velocity.y == 0){
            setState(AnimatingState.Stand);
        }

        if(currentState !=  AnimatingState.Grab && (controller.isGriping || controller.OnSteep)) {
            setState(AnimatingState.Grab);
        }
        else if(currentState ==  AnimatingState.Grab && !controller.isGriping && !controller.OnSteep) {
            setState(AnimatingState.Stand);
        }
        else if(!isDashing && controller.isDashing){
            animatior.SetTrigger("Dash");
            isDashing = true;
        }
        else if(isDashing && !controller.isDashing) {
            isDashing = false;
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
