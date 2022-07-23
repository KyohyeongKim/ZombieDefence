using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Image playerHealth;

    public Text playerGold;

    public Text nowBulletInfo;

    public GameObject buyBullet;

    public int bulletPrice = 500;

    private Player player;
    private Gun playerGun;

    private Image buyBulletImage;
    private Button buyBulletButton;

    private Color activeColor = new Color(0.6470588f, 0.6094604f, 0.3921569f);
    private Color deactiveColor = new Color(0.4f, 0.4f, 0.4f);

    private RectTransform playerHealthTr;

    private float maxWidth = 10f;

    private void Start() {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        player = playerObj.GetComponent<Player>();
        playerGun = playerObj.GetComponentInChildren<Gun>();

        buyBulletImage = buyBullet.GetComponent<Image>();
        buyBulletButton = buyBullet.GetComponent<Button>();

        playerHealthTr = playerHealth.gameObject.GetComponent<RectTransform>();
        maxWidth = playerHealthTr.rect.width;
    }

    private void Update() {
        OnHealthUpdate();
        OnGoldUpdate();
        OnBulletUpdate();
    }

    private void OnHealthUpdate() {
        // Stretch 방식 = 화면 전체 크기 - 원하는 크기만큼 잘라낸 크기
        float nowWidth = maxWidth * (player.GetHealth() / player.maxHealth);

        Vector2 size = playerHealthTr.sizeDelta;
        playerHealthTr.sizeDelta = new Vector2(nowWidth, size.y);
    }

    private void OnGoldUpdate() {
        int gold = player.nowGold;

        playerGold.text = gold.ToString();

        if (gold >= bulletPrice) {
            buyBulletButton.interactable = true;
            buyBulletImage.color = activeColor;
        }
        else {
            buyBulletButton.interactable = false;
            buyBulletImage.color = deactiveColor;
        }
    }

    private void OnBulletUpdate() {
        int nowAmmo = playerGun.GetNowAmmo();
        int maxAmmo = player.maxAmmo;
        int nowReloading = playerGun.GetNowReloading();

        nowBulletInfo.text = string.Format("{0} / {1} ({2})",
            nowAmmo, maxAmmo, nowReloading);
    }

    public void OnBuyBulletButtonClicked() {
        player.nowGold -= bulletPrice;
        playerGun.AddNowReloading(1);
    }
}