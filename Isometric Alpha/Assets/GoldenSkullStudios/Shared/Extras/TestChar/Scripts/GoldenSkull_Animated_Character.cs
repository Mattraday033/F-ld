using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GoldenSkull_Animated_Character : MonoBehaviour
{
    [SerializeField]
    private float speed = 1;

    [SerializeField]
    Animator animator;

    private Vector3 startScale;

    [SerializeField]
    private SpriteRenderer characterSprite;

    // Start is called before the first frame update
    void Start()
    {
        characterSprite = GetComponent<SpriteRenderer>();
        startScale = this.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();
    }

    private Vector3 lastDirection = Vector3.zero;
    private Vector3 direction = Vector3.zero;

    void SetAnimatorStates(float _currSpeed)
    {
        if( animator!= null)
        {
            //animator.SetFloat("speed", _currSpeed);
        }

        lastDirection = direction;
        direction = Vector3.zero;
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            direction.x = -1;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            direction.x = 1;
        }
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            direction.y = -1;
        }
        else if (Input.GetAxisRaw("Vertical") > 0)
        {
            direction.y = 1;
        }
        
        //Debug.Log("Input.GetAxisRaw(Horizontal) " + Input.GetAxisRaw("Horizontal"));
        //Debug.Log("Input.GetAxisRaw(Vertical) " + Input.GetAxisRaw("Vertical"));
        //Debug.Log("Direction" + direction + "direction.magnitude:"+ direction.magnitude);
        

        if ( Input.GetAxisRaw("Horizontal")==0 && Input.GetAxisRaw("Vertical")==0 )
        {
            PlayAnimation("Idle", lastDirection);
        }
        else
        {
            PlayAnimation("Run", direction);
        }
    }
    
    void PlayAnimationAtTime(string _animName)
    {
        animator.PlayInFixedTime(_animName);
    }
    void PlayAnimation(string _animName , Vector3 _dir)
    {
        //UP
        if (_dir.y > 0 && _dir.x == 0)
        {
            PlayAnimationAtTime(_animName + "_Up");
        }
        //DOWN
        if (_dir.y < 0 && _dir.x == 0)
        {
            PlayAnimationAtTime(_animName + "_Down");
        }
        //LEFT
        if (_dir.y == 0 && _dir.x < 0)
        {
            PlayAnimationAtTime(_animName + "_Side");
        }
        //RIGHT
        if (_dir.y == 0 && _dir.x > 0)
        {
            PlayAnimationAtTime(_animName + "_Side");
        }
        //Up-Right
        if (_dir.y > 0 && _dir.x > 0)
        {
            PlayAnimationAtTime(_animName + "_Up-Side");
        }
        //Up-Left
        if (_dir.y > 0 && _dir.x < 0)
        {
            PlayAnimationAtTime(_animName + "_Up-Side");
        }
        //Down-Right
        if (_dir.y < 0 && _dir.x > 0)
        {
            PlayAnimationAtTime(_animName + "_Down-Side");
        }
        //Down-Left
        if (_dir.y < 0 && _dir.x < 0)
        {
            PlayAnimationAtTime(_animName + "_Down-Side");
        }
    }

    void MoveCharacter()
    {
        //I am putting these placeholder variables here, to make the logic behind the code easier to understand
        //we differentiate the movement speed between horizontal(x) and vertical(y) movement, since isometric uses "fake perspective"
        float horizontalMovement = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        //since we are using this with isometric visuals, the vertical movement needs to be slower
        //for some reason, 50% feels too slow, so we will be going with 75%
        float verticalMovement = Input.GetAxisRaw("Vertical") * speed * 0.5f * Time.deltaTime;

        float totalMovement = Mathf.Abs(horizontalMovement + verticalMovement);

        SetAnimatorStates(totalMovement * 100);

        if ( totalMovement != 0 )
        {
            FlipSpriteToMovement();
        }

        this.transform.Translate(horizontalMovement, verticalMovement, 0);
    }

    //if the player moves left, flip the sprite, if he moves right, flip it back, stay if no input is made
    void FlipSpriteToMovement()
    {
        Vector3 currScale = startScale;

        if (Input.GetAxisRaw("Horizontal") < 0)
            currScale.x = startScale.x;
        else if (Input.GetAxisRaw("Horizontal") > 0)
            currScale.x = -startScale.x;

        this.transform.localScale = currScale;

        return;
        /*
        if(characterSprite != null )
        {
            if (Input.GetAxisRaw("Horizontal") < 0)
                characterSprite.flipX = true;
            else if (Input.GetAxisRaw("Horizontal") > 0)
                characterSprite.flipX = false;
        }
        */
    }
}
