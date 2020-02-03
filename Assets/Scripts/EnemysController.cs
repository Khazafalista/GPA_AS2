using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemysController : MonoBehaviour
{
    //parameters
    public float speed = 3.0f;
    public float rotateSpeed = 2.0f;

    public float attackDistance = 1.5f;
    public int hitOffset = 50;

    public int playerDamage = 50;

    [SerializeField]
    int health;

    [SerializeField]
    int maxHealth;

    Rigidbody rb;

    GameObject player;
    GameObject bloodSplatter;
    GameObject gameController;
    GameObject knightHealthBar;
    GameObject knightHealth;

    Animator animator;
    int idleState;
    int moveState;
    int attackState;

    //randomize action
    int modeCounter;
    int mode;

    //prevent moving
    bool lockMove;

    //true if hit
    bool isDamaging;

    //control hit
    int attackCounter;

    AnimatorStateInfo state;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        rb = gameObject.GetComponent<Rigidbody>();

        modeCounter = 0;
        mode = 0;

        lockMove = false;

        isDamaging = false;

        attackCounter = 0;
    }

    private void Awake()
    {
        player = GameObject.Find("Player");
        bloodSplatter = GameObject.Find("BloodSplatter");
        gameController = GameObject.Find("GameController");
        knightHealthBar = GameObject.Find("KnightHealthBar");
        knightHealth = GameObject.Find("KnightHealth");

        idleState = Animator.StringToHash("Base Layer.1HCombatIdle");
        moveState = Animator.StringToHash("Base Layer.move");
        attackState = Animator.StringToHash("Base Layer.1HAttack");

        maxHealth = health;
    }

    private void FixedUpdate()
    {
        //relationship with player
        float playerDistance = Vector3.Distance(player.transform.position, gameObject.transform.position);
        Vector3 playerDirection = (player.transform.position - gameObject.transform.position).normalized;

        //current animation state
        state = animator.GetCurrentAnimatorStateInfo(0);

        if (state.fullPathHash == idleState || state.fullPathHash == moveState)
        {
            lockMove = false;
            isDamaging = false;
            if (Vector3.Dot(transform.forward, playerDirection) > 0.5f && playerDistance < attackDistance)
            {
                animator.SetBool("Attack", true);
                isDamaging = true;
                attackCounter = 0;
            }
        }
        else if(state.fullPathHash == attackState)
        {
            animator.SetBool("Attack", false);
            lockMove = true;
            attackCounter++;
            //Debug.Log(attackCounter);
        }

        if (!lockMove)
        {
            if(modeCounter == 100)
            {
                mode = Random.Range(0, 100);
                modeCounter = 0;
            }
            else
            {
                modeCounter++;
            }

            if (mode >= 10)
            {
                Quaternion tempRotation = transform.rotation;
                transform.LookAt(player.transform.position);
                Quaternion targetRotation = transform.rotation;
                transform.rotation = Quaternion.Slerp(tempRotation, targetRotation, rotateSpeed * Time.deltaTime);
                rb.velocity = transform.forward * speed;
                rb.angularVelocity = Vector3.zero;
            }
            else
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

        animator.SetFloat("Speed", Vector3.Distance(rb.velocity, Vector3.zero));
    }

    private void OnCollisionStay(Collision other)
    {
        if(other.gameObject.name == "Player")
        {
            if(state.fullPathHash == attackState && isDamaging && attackCounter > hitOffset)
            {
                float playerDistance = Vector3.Distance(player.transform.position, gameObject.transform.position);
                Vector3 playerDirection = (player.transform.position - gameObject.transform.position).normalized;
                if (Vector3.Dot(transform.forward, playerDirection) > 0.5f && playerDistance < attackDistance)
                {
                    if (gameObject.name == "NinjaEnemy(Clone)")
                    {
                        //Debug.Log("NINJA ATTACK");
                        player.GetComponent<PlayerController>().TakeDamage(0);
                    }
                    else if (gameObject.name == "KnightEnemy(Clone)")
                    {
                        player.GetComponent<PlayerController>().TakeDamage(1);
                    }
                    isDamaging = false;
                    Debug.Log("ENEMY ATTACK!!");
                }    
            }
        }
    }

    public void TakeDamage()
    {
        health -= Random.Range(playerDamage - 10, playerDamage + 10);

        if(gameObject.name == "NinjaEnemy(Clone)")
        {
            GetComponentInChildren<HealthBarController>().ChangeHealthNinja(health, maxHealth);
        }
        else if(gameObject.name == "KnightEnemy(Clone)")
        {
            knightHealth.GetComponent<Image>().fillAmount = (float)health / (float)maxHealth;
        }
        //Debug.Log("ENEMY HURTED!!");
        //Debug.Log(health);
        gameObject.GetComponent<AudioSource>().Play();
        bloodSplatter.transform.position = new Vector3(transform.position.x, bloodSplatter.transform.position.y, transform.position.z);
        if(gameObject.name == "KnightEnemy(Clone)")
        {
            bloodSplatter.transform.localScale += new Vector3(9.0f, 9.0f, 9.0f);
        }
        bloodSplatter.GetComponent<ParticleSystem>().Play();

        if(health <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        Destroy(gameObject);
        //Debug.Log("ENEMY DESTROYED!!");

        if(gameController.GetComponent<GameController>().GetStage() == 1)
        {
            gameController.GetComponent<GameController>().GetScore();
        }
        else if(gameController.GetComponent<GameController>().GetStage() == 2)
        {
            knightHealth.SetActive(false);
            gameController.GetComponent<GameController>().GameClear();
        }
    }

    public void SetHealth(int _health)
    {
        health = _health;
        maxHealth = health;
    }
}
