using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f;

    public int enemy1Damage = 10;
    public int enemy2Damage = 50;

    public AudioSource sePlayer;
    public AudioClip hurtedSE;

    public float sprintForce = 1000.0f;

    public GameObject mainCamera;
    public GameObject bloodSplatter;
    public GameObject gameController;
    public GameObject healthBar;

    Rigidbody rb;

    Animator animator;
    int idleState;
    int moveState;
    int attackState;
    int painState;
    int sprintState;
    AnimatorStateInfo state;

    bool lockMove;

    bool isDamaging;

    bool sprint;

    int health;
    int maxHealth;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        animator = gameObject.GetComponent<Animator>();

        lockMove = false;

        isDamaging = false;

        Cursor.visible = false;

        health = 100;
        maxHealth = health;

        sprint = false;
    }

    private void Awake()
    {
        idleState = Animator.StringToHash("Base Layer.1HCombatIdle");
        moveState = Animator.StringToHash("Base Layer.move");
        attackState = Animator.StringToHash("Base Layer.1HAttack");
        painState = Animator.StringToHash("Base Layer.1HPain");
        sprintState = Animator.StringToHash("Base Layer.Sprint");
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        state = animator.GetCurrentAnimatorStateInfo(0);
        
        if(state.fullPathHash == idleState || state.fullPathHash == moveState)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                animator.SetBool("Attack", true);
                lockMove = true;
                isDamaging = true;
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                animator.SetBool("isSprint", true);
                lockMove = true;
                isDamaging = false;
                sprint = true;
            }
            else
            {
                lockMove = false;
                isDamaging = false;
            }
        }
        else if(state.fullPathHash == attackState)
        {
            animator.SetBool("Attack", false);
            lockMove = true;
        }
        else if(state.fullPathHash == painState)
        {
            animator.SetBool("isHurted", false);
            lockMove = true;
        }
        else if(state.fullPathHash == sprintState)
        {
            animator.SetBool("isSprint", false);
            lockMove = true;
            if(sprint)
            {
                rb.AddForce(rb.transform.forward * sprintForce, ForceMode.VelocityChange);
                sprint = false;
            }
        }

        animator.SetFloat("Speed", Mathf.Abs(moveVertical) + Mathf.Abs(moveHorizontal));

        if(!lockMove)
        {
            Vector3 forward = mainCamera.transform.forward;
            Vector3 right = mainCamera.transform.right;

            forward.y = 0.0f;
            right.y = 0.0f;

            rb.velocity = speed * (forward.normalized * moveVertical + right.normalized * moveHorizontal).normalized;

            rb.angularVelocity = Vector3.zero;

            if (Mathf.Abs(moveVertical) > 0.1f || Mathf.Abs(moveHorizontal) > 0.1f)
            {
                rb.transform.rotation = Quaternion.LookRotation(forward.normalized * moveVertical + right.normalized * moveHorizontal);
            }
        }
        else
        {
            if(state.fullPathHash != sprintState)
            {
                rb.velocity = Vector3.zero;
            }
            else
            {
                rb.velocity = rb.velocity;
            }
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //Debug.Log(other.gameObject.name);
            Vector3 dir = (other.gameObject.transform.position - transform.position).normalized;

            if(Vector3.Dot(transform.forward, dir) > 0.5f)
            {
                if (state.fullPathHash == attackState && isDamaging)
                {
                    other.gameObject.GetComponent<EnemysController>().TakeDamage();
                    isDamaging = false;
                    Debug.Log("PLAYER ATTACK!!");
                }
            }
        }
    }

    public void TakeDamage(int enemy)
    {
        //Debug.Log("PLAYER HURTED!!");
        if(state.fullPathHash != sprintState)
        {
            if (enemy == 0)
            {
                health -= Random.Range(enemy1Damage - 5, enemy1Damage + 5);
                healthBar.GetComponent<Image>().fillAmount = (float)health / (float)maxHealth;
            }
            else if (enemy == 1)
            {
                health -= Random.Range(enemy2Damage - 5, enemy2Damage + 5);
                healthBar.GetComponent<Image>().fillAmount = (float)health / (float)maxHealth;
            }

            if (health <= 0)
            {
                Dead();
            }
            else
            {
                if(state.fullPathHash != attackState)
                {
                    animator.SetBool("isHurted", true); 
                }
                bloodSplatter.transform.position = new Vector3(transform.position.x, bloodSplatter.transform.position.y, transform.position.z);
                bloodSplatter.GetComponent<ParticleSystem>().Play();

                sePlayer.PlayOneShot(hurtedSE);
            }
        }
    }

    private void Dead()
    {
        Debug.Log("GAME OVER!!");
        healthBar.SetActive(false);
        animator.SetBool("isDead", true);
        gameController.GetComponent<GameController>().GameLost();
    }
}
