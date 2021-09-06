using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAnim
{
    public AnimationClip idle;
    public AnimationClip runF;
    public AnimationClip runB;
    public AnimationClip runL;
    public AnimationClip runR;
}

public class PlayerAnimation : MonoBehaviour
{
    public PlayerAnim playerAnim;
    
    [HideInInspector]
    public Animation animation;
    private PlayerInput playerInput;
    private CharacterController playerCC;

    private void Awake()
    {
        animation = GetComponent<Animation>();
        playerInput = GetComponent<PlayerInput>();
        playerCC = GetComponent<CharacterController>();
    }

    private void Start()
    {
        animation.clip = playerAnim.idle;
        animation.Play();
    }

    private void Update()
    {
        float v = playerInput.frontMove;
        float h = playerInput.rightMove;

        Vector3 moveDir = playerCC.velocity.normalized;
        float angle = Vector3.Angle(transform.forward, moveDir);

        Debug.Log(angle);

        if(angle >= 1 && angle <= 50){
            animation.CrossFade(playerAnim.runF.name, 0.3f);
        }else if(angle >= 150 && 180 < angle){
            animation.CrossFade(playerAnim.runB.name, 0.3f);
        }else if(angle > 50 && angle > 150f){
            if(Vector3.Angle(transform.right, moveDir) <= 50){
                animation.CrossFade(playerAnim.runR.name, 0.3f);
            }
            else{
                animation.CrossFade(playerAnim.runL.name, 0.3f);
            }
        }else {
            animation.CrossFade(playerAnim.idle.name, 0.3f);
        }
        
    }
}
