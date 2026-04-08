using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Pest : MonoBehaviour
{
    //Pest needs: the ability to kill crops, move towards the player, get killed, and despawn (if the player goes far enough away (for larger maps))
    //need to have it so that large numbers of enemies don't lag the game
    [SerializeField] public int attackDamage = 4;
    [SerializeField] public float attackSpeed = 0.6f;
    [SerializeField] public int health = 10;
    private NavMeshAgent navAgent;
    private GameObject targetObject;
    //private int currentMoveSpeed;
    //private Rigidbody2D rigidbody;
    //private Vector2 movementDirection = Vector2.zero;

    //player vars
    private GameObject player;
    private PlayerController playerScript;
    private bool playerInRange = false;

    //crop vars
    private GameObject currentCrop;
    private Crop currentCropScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //currentMoveSpeed = moveSpeed;
        //rigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player"); //this should check if there are any crops left, then go after the player if there aren't
        playerScript = player.GetComponent<PlayerController>();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;
        GameManager.Instance.livingPests.Add(gameObject);
    }

    private void Update()
    {
        if (GameManager.Instance.livingCrops.Count > 0)
        {
            if (currentCrop == null)
            {
                float closestCropDistance = 100000;
                int closestCropIndex = -1;
                for (int i = 0; i < GameManager.Instance.livingCrops.Count; i++)
                {
                    if (!GameManager.Instance.livingCrops[i].GetComponent<Crop>().isOccupied)
                    {
                        float currentDistance = Vector3.Distance(transform.position, GameManager.Instance.livingCrops[i].transform.position);
                        if (currentDistance < closestCropDistance)
                        {
                            closestCropDistance = currentDistance;
                            closestCropIndex = i;
                        }
                    }
                }
                //movementDirection = GameManager.Instance.livingCrops[closestCropIndex].transform.position - transform.position;
                if (closestCropIndex != -1)
                {
                    targetObject = GameManager.Instance.livingCrops[closestCropIndex];
                }
                else
                {
                    targetObject = player;
                }
            }
        }
        else
        {
            targetObject = player;
        }

        if (targetObject != null)
        {
            navAgent.SetDestination(targetObject.transform.position);
        }
        else
        {
            targetObject = player;
            navAgent.SetDestination(targetObject.transform.position);
        }
    }

    void FixedUpdate()
    {
        //GetComponent<Rigidbody>().linearVelocity = movementDirection.normalized * currentMoveSpeed * Time.deltaTime;
    }

    //when the pest enters the trigger of a crop, it will check if the pest is currently killing a crop, if the crop is in livingCrops, and isn't currently being killed by a pest. Then, it will be moved to the middle, the crop will be labelled as occupied, and its movement speed will be set to zero. it will start a coroutine that waits a few seconds (killing the crop) and checks if the pest still has health before killing the crop. 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Crop")) //could be an issue where the pest touches a crop when they're already on one and it doesn't get destroyed
        {
            if (currentCrop == null && GameManager.Instance.livingCrops.Contains(collision.gameObject) && !collision.gameObject.GetComponent<Crop>().isOccupied)
            {
                currentCrop = collision.gameObject;
                currentCropScript = collision.gameObject.GetComponent<Crop>();
                currentCropScript.isOccupied = true;
                transform.position = new Vector3(currentCrop.transform.position.x, currentCrop.transform.position.y + (GetComponent<SpriteRenderer>().size.y / 2)); //would change the animation and play a sound
                //currentMoveSpeed = 0;
                StartCoroutine(KillingCrop());
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!playerInRange)
            {
                playerInRange = true;
                StartCoroutine(AttackingPlayer());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Crop"))
        {
            if (currentCrop != null && collision.gameObject == currentCrop)
            {
                currentCropScript.isOccupied = false;
                //currentMoveSpeed = moveSpeed;
                currentCrop = null;
                currentCropScript = null;
                
                StopCoroutine(KillingCrop());
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            StopCoroutine(AttackingPlayer());
        }
    }

    private IEnumerator KillingCrop()
    {
        yield return new WaitForSeconds(5);
        if (health > 0)
        {
            currentCropScript.Die();
            currentCropScript.isOccupied = false;
            //currentMoveSpeed = moveSpeed;
            currentCrop = null;
            currentCropScript = null;
        }
    }

    private IEnumerator AttackingPlayer()
    {
        if (health > 0 && playerInRange)
        {
            playerScript.TakeDamage(attackDamage);
            yield return new WaitForSeconds(attackSpeed);
            StartCoroutine(AttackingPlayer());
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    //when the pest is going to die, it must unoccupy any crops it is on and drop xp for the player.
    public void Die()
    {
        if (currentCrop != null)
        {
            currentCropScript.isOccupied = false;
            currentCrop = null;
            currentCropScript = null;
        }
        GameManager.Instance.livingPests.Remove(gameObject);
        GameManager.Instance.AddPestToCount();
        Destroy(gameObject);
    }
}
