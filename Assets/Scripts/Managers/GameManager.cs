using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    TODO
    - 좀비 스폰 관리
        - 스폰 위치 4곳 중 랜덤한 곳 한 곳에서 한 마리씩 스폰시키기
        - 최대 20마리

    - 플레이어 돈 관리
        - 좀비를 죽이면 돈을 얻는다
        - 총알을 사면 돈을 잃는다

    - 게임 오버
        - 플레이어의 체력이 0 이하가 되면 게임오버 화면을 띄운다
*/

public class GameManager : MonoBehaviour {
    [Header("좀비 스폰 관리")]
    [SerializeField]
    private GameObject[] spawnPoints = new GameObject[4];

    [SerializeField]
    private GameObject zombiePrefab;

    [SerializeField]
    private GameObject zombieParent;

    [SerializeField]
    private float maxSpawnCooldown = 2f;

    [SerializeField]
    private int maxSpawnCount = 20;

    private float nowSpawnCooldown = 0f;
    private int nowSpawnCount = 0;

    private void Start() {
        
    }

    private void Update() {
        ZombieSpawn();
    }

    private void ZombieSpawn() {
        if (nowSpawnCount < maxSpawnCount) {
            if (nowSpawnCooldown >= maxSpawnCooldown) {
                nowSpawnCooldown = 0f;

                int random = Random.Range(0, spawnPoints.Length);

                GameObject zombie = Instantiate(zombiePrefab, spawnPoints[random].transform.position, Quaternion.identity, zombieParent.transform);
                nowSpawnCount++;
            }
            else {
                nowSpawnCooldown += Time.deltaTime;
            }
        }
    }
}