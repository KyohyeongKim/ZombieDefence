using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealth : MonoBehaviour {
    public GameObject healthBar;

    private RectTransform rect;

    private Zombie zombie;

    private float maxWidth = 0.35f;

    /*
        [ 팁 ]
        - 기본적으로 아웃렛 접속은 하이어라키 인스턴스 또는 애셋 폴더 프리팹으로만 연결이 가능함.
        - 아웃렛 접속을 만든 오브젝트가 인스턴스라면 삭제되고 다시 스폰했을 때 아웃렛 접속이 초기화
        - (프리팹) 자기 자신이 가지고 있는 자식 오브젝트가 아웃렛 접속으로 연결이 되면 그 때는 초기화되지 않음.
    */

    private void Start() {
        zombie = GetComponent<Zombie>();
        rect = healthBar.GetComponent<RectTransform>();
    }

    private void Update() {
        if (zombie.maxHealth != zombie.GetHealth()) {
            float nowWidth = maxWidth * (zombie.GetHealth() / zombie.maxHealth);

            Vector2 size = rect.sizeDelta;
            rect.sizeDelta = new Vector2(nowWidth, size.y);
        }
    }
}