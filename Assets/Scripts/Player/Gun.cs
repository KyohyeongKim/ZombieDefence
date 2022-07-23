using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Gun : MonoBehaviour {
    [Header("총알 설정")]
    public float ammoDamage = 25f;

    [Space]
    [Header("총알 효과 및 거리 설정")]
    [SerializeField]
    private float fireDist = 50f; // 총알의 최대 사정거리

    [SerializeField]
    private GameObject fireEffect;

    [SerializeField]
    private GameObject fireEffectPos; // 총알 발사 효과의 시작 지점

    [Space]
    [Header("총기 효과음")]
    [SerializeField]
    [Range(0f, 1f)]
    private float volume = 0.5f;

    [SerializeField]
    private AudioClip fire;

    [SerializeField]
    private AudioClip fireEmpty;

    [SerializeField]
    private AudioClip reloading;

    private AudioSource audioSource;
    private LineRenderer render;

    private Player player;

    private int nowAmmo = 0;
    private int nowReloading = 0;

    private void Start() {
        render = GetComponent<LineRenderer>();
        audioSource = fireEffectPos.GetComponent<AudioSource>();
        player = transform.parent.parent.GetComponent<Player>();

        nowAmmo = player.maxAmmo;
        nowReloading = player.maxReloading;

        render.enabled = false;

        // AudioListener = 게임 소리 마이크 -> 게임 프로그램과 연결된 스피커로 재생
        // AudioSource = 음악/소리 플레이어 (Clip을 넣고 재생시키는 역할)
        // AudioClip = 음악/소리 파일
        audioSource.volume = volume;

        fireEffect.SetActive(false);
    }

    private void FireGun() {
        GameObject target = null;
        // Raycast(빛을 쏴서 부딫힌 오브젝트를 찾는 방법)를 인식하는 센서(?) 클래스
        RaycastHit hit;

        Vector3 hitPos = Vector3.zero;

        if (Physics.Raycast(fireEffectPos.transform.position, fireEffectPos.transform.forward, out hit, fireDist)) {
            // Raycast를 쐈을 때 어떠한 물체(콜라이더가 있는 물체)에 부딫혔다면 해당 위치를 저장
            target = hit.collider.gameObject;   // 부딫힌 곳에 있던 게임오브젝트의 정보
            hitPos = hit.point;
		}
        else {
            // 만약 부딫히지 않았다면 앞(forward)방향으로 사정 거리만큼 곱해서 해당 위치 정보를 저장
            hitPos = fireEffectPos.transform.position + fireEffectPos.transform.forward * fireDist;
		}

        // Play() : 이전에 어떤 소리를 재생하고 있던 상관 없이 그 즉시 중단시키고 본인 소리를 재생
        // PlayOneShot() : 이전에 재생되던 소리 위에 추가로 본인 소리를 재생

        audioSource.PlayOneShot(fire, volume);
        StartCoroutine(FireEffectDraw(hitPos, target));
	}

    private IEnumerator FireEffectDraw(Vector3 hitPos, GameObject target) {
        // 총알의 궤적을 그릴 두 개의 점의 좌표를 설정
        render.SetPosition(0, fireEffectPos.transform.position);
        render.SetPosition(1, hitPos);

        // 궤적을 그리고
        render.enabled = true;
        fireEffect.SetActive(true);
        fireEffect.GetComponent<ParticleSystem>().Play();

        if (target != null && target.tag == "Zombie") {
            target.GetComponent<Zombie>().OnDamaged(ammoDamage);
        }

        // 0.02초 만큼 기다린 다음
        yield return new WaitForSeconds(0.02f);

        // 화면에서 지움
        render.enabled = false;
        fireEffect.GetComponent<ParticleSystem>().Stop();
        fireEffect.SetActive(false);
    }

    public void Fire() {
        if (nowAmmo > 0) {
            nowAmmo--;
            Debug.Log("총알을 사용했습니다. 현재 남은 총알: " + nowAmmo + " / " + player.maxAmmo
                + ", 남은 재장전 수: " + nowReloading + " / " + player.maxReloading);

            FireGun();
		}
        else {
            Debug.LogError("탄창에 남은 총알이 없습니다. 재장전을 먼저 해주세요!");
            audioSource.PlayOneShot(fireEmpty, volume);
		}
	}

    public void ReloadAmmo() {
        if (nowReloading > 0) {
            nowAmmo = player.maxAmmo;
            nowReloading--;

            Debug.LogWarning("재장전!");
            audioSource.PlayOneShot(reloading, volume);
        }
        else {
            Debug.LogError("더 이상 재장전 할 수 없습니다! 상점에서 총알을 더 구매해주세요!");
        }
    }

    public int GetNowAmmo() {
        return nowAmmo;
    }

    public void AddNowReloading(int amount) {
        nowReloading += amount;
    }

    public int GetNowReloading() {
        return nowReloading;
    }
}