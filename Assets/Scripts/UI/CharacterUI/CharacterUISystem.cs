using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(PresentationSystemGroup))] // UI는 주로 PresentationSystemGroup에서 업데이트됨
public partial class UpdateCharacterUISystem : SystemBase
{
    protected override void OnUpdate()
    {
        // Entities
        //     .WithAll<CharacterUIController>()
        //     .ForEach((Entity entity, CharacterUIController uiController, in HealthComponent health, in ActGaugeComponent actGauge) =>
        //     {
        //         uiController.UpdateHealthBar(health.currentHealth, health.maxHealth);
        //         uiController.UpdateActGaugeBar(actGauge.currentGauge, actGauge.maxGauge);
        //     }).WithoutBurst().Run(); // UI 업데이트는 메인 스레드에서 실행되어야 하므로 WithoutBurst와 Run을 사용
    }
}
