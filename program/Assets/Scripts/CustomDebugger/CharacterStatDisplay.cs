using Character;
using TMPro;
using UnityEngine;

public class CharacterStatDisplay : MonoBehaviour
{
    public TMP_Text statText; // TextMeshPro 텍스트 컴포넌트를 연결
    public UnitController unitController; // 캐릭터 스탯을 저장하는 스크립트
	private CharacterStat characterStat; // 캐릭터 스탯을 저장하는 스크립트

    private void Start()
    {
        // 캐릭터의 스탯 컴포넌트를 가져옵니다
        characterStat = unitController.CharacterStat; 
		if (unitController == null)
		{
			unitController = GetComponent<UnitController>(); // UnitController 컴포넌트를 가져옵니다
		}	
        if (statText == null)
        {
            statText = GetComponentInChildren<TMP_Text>(); // 자식에서 TextMeshPro 오브젝트 찾기
        }
    }

    private void Update()
    {
		// 스탯을 텍스트로 업데이트합니다 (예: HP와 공격력 표시)
		statText.text = $"HP: {unitController.GetCurrentHP()}\nAttack: {unitController.GetAttackDamage()}\n";
		statText.text += $"State: {unitController.CharacterActionState}";
    }
}