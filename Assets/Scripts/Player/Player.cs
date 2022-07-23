using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [Header("캐릭터 설정")]
    public float maxHealth = 100f;
    public int nowGold = 0;

    [Header("총알 설정")]
    public int maxAmmo = 10;
    public int maxReloading = 10;

    private float nowHealth = 100f;

    public float GetHealth() {
        return nowHealth;
    }

    public void ResetHealth() {
        nowHealth = maxHealth;
    }

    public void RemoveHealth(float damage) {
        nowHealth -= damage;
    }
}