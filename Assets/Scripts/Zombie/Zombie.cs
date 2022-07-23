using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour {
    public float maxHealth = 100f;

    private float health = 100f;

    [SerializeField]
    private float attackRange = 2f;

    [SerializeField]
    private float attackDamage = 15f;   // TODO: 13f~17f 사이의 값을 대미지로 사용하기

    [SerializeField]
    private float attackSpeed = 1.5f;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private int rewardGold = 100;

    private float attackCooldown = 0f;

    private Animator ani;
    private Rigidbody rig;
    private NavMeshAgent nav;

    private Player player;

    private GameObject playerObj;

    private void Start() {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();

        playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.GetComponent<Player>();

        health = maxHealth;
    }

    private void Update() {
        if (health > 0) {
            nav.enabled = true;         // 컴포넌트 활성화 여부
            nav.isStopped = false;      // NavMeshAgent의 활성화 여부 (true: 멈춤 / false: 움직임)
            nav.speed = speed;

            if (playerObj != null) {
                nav.SetDestination(playerObj.transform.position);
                ani.SetBool("Run", true);

                if (player.GetHealth() > 0) {
                    float dist = Vector3.Distance(playerObj.transform.position, transform.position);

                    if (dist <= attackRange) {
                        if (attackCooldown >= attackSpeed) {
                            attackCooldown = 0;

                            ani.SetTrigger("Attack");

                        }
                        else {
                            attackCooldown += Time.deltaTime;
                        }
                    }
                    else {
                        // 좀비가 더 이상 플레이어를 공격하지 않는 로직
                    }
                }
            }
            else {
                nav.SetDestination(Vector3.zero);

                nav.isStopped = true;
                nav.enabled = false;
            }
        }
        else {
            if (nav.enabled) {
                nav.SetDestination(Vector3.zero);

                nav.isStopped = true;
                nav.enabled = false;

                ani.SetTrigger("Die");
                StartCoroutine(OnZombieDie());
            }
        }

        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;
	}

    private IEnumerator OnZombieDie() {
        GetComponent<Collider>().enabled = false;
        player.nowGold += rewardGold;

        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }

	public void OnDamaged(float damage) {
        health -= damage;
	}

    public void OnPlayerAttack() {
        playerObj.GetComponent<PlayerController>().OnDamaged(attackDamage);
    }

    public float GetHealth() {
        return health;
    }
}