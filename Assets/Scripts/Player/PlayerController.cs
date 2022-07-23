using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("권총 견착 위치 오브젝트")]
    [SerializeField]
    private Transform gunPivot; // 권총 자체의 기준점

    [SerializeField]
    private Transform leftHand; // 왼손의 기준점

    [SerializeField]
    private Transform rightHand; // 오른손의 기준점

    [Space] // 인스펙터에 작은 빈 공간을 생성하는 속성
    [Header("캐릭터 설정")] // 인스펙터에 제목을 생성하는 속성(Attribute)
    [SerializeField]
    private float moveSpeed = 2f;

    [SerializeField]
    private float turnSpeed = 2f;

    private Animator ani;
    private Rigidbody rig;

    private Gun gun;

    private void Start() {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();

        gun = GetComponentInChildren<Gun>();
    }

    private void Update() {
        MoveControl();

        if (Input.GetMouseButtonDown(0)) {
            gun.Fire();
		}

        if (Input.GetKeyDown(KeyCode.R)) {
            gun.ReloadAmmo();
        }
    }

	private void OnAnimatorIK(int layerIndex) {
        // 총의 기준점의 위치를 아바타의 오른쪽 팔꿈치를 기준으로 잡는다
        gunPivot.position = ani.GetIKHintPosition(AvatarIKHint.RightElbow);

        // Tranform = 위치(Position), 회전(Rotation), 크기(Scale)
        // SetIKPositionWeight(목표IK, 가중치);
        // -> IK(역운동학)을 계산할 목표IK를 지정하고 가중치를 설정한다. (가중치와 우선순위는 비슷하다고 보면 됨)
        // SetIKPosition(목표IK, 목표위치);
        // -> 관절을 목표 위치로부터 역계산을 해서 목표IK가 해당 좌표에 위치할 수 있도록 함.
        ani.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        ani.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);

        ani.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
        ani.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation);

        // [위쪽] 왼손(LeftHand) / [아래쪽] 오른손(RightHand)

        ani.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        ani.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);

        ani.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
        ani.SetIKRotation(AvatarIKGoal.RightHand, rightHand.rotation);
    }

	private void MoveControl() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

		Vector3 movePos = transform.forward.normalized * moveSpeed * z * Time.deltaTime;

        rig.MovePosition(transform.position + movePos);
        rig.rotation = rig.rotation * Quaternion.Euler(0, x * turnSpeed, 0);

        // ani.SetBool("Run", true);

        if (z == 0) {
            // 정지
            ani.SetBool("Run_Forward", false);
            ani.SetBool("Run_Backward", false);
        }
        else if (z > 0) {
            // 앞
            ani.SetBool("Run_Forward", true);
            ani.SetBool("Run_Backward", false);
		}
        else {
            // 뒤
            ani.SetBool("Run_Forward", false);
            ani.SetBool("Run_Backward", true);
        }   
	}

    public void OnDamaged(float damage) {
        ani.SetTrigger("Damaged");
        GetComponent<Player>().RemoveHealth(damage);
    }
}