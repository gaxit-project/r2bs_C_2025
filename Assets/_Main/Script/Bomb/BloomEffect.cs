using UnityEngine;

public class BloomEffect : MonoBehaviour
{
    public Material[] flowerMaterial;
    public float duration = 2f;
    public float bloomSpeed = 1.0f;
    public float size = 0.75f;
    public int petals = 20;

    Material selectMaterial;


    public static BloomEffect Instance;
    private void Awake()
    {
        Instance = this;
    }



    /// <summary>
    /// 爆発エフェクトを再生
    /// </summary>
    /// <param name="position"></param>
    public void CreateFlowerEffect(Vector3 position, Team teamName)
    {
        GameObject bloomObj = new GameObject("FlowerBloom");
        bloomObj.transform.position = position;

        ParticleSystem ps = bloomObj.AddComponent<ParticleSystem>();

        var main = ps.main;
        main.startLifetime = duration;         // 粒子の寿命
        main.startSpeed = bloomSpeed;          // 飛ぶスピード
        main.startSize = size;                 // 粒子の大きさ
        main.loop = false;                     // ループさせず、1回だけ出す
        main.maxParticles = petals;            // 最大粒子数
        main.simulationSpace = ParticleSystemSimulationSpace.World; // ワールド座標で表示
        main.playOnAwake = false;              // 自動再生しない（自分でEmitする）


        // 形状設定
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Cone; // 放射形状を円錐に
        shape.radius = 0.1f;                             // 放射の始点サイズ（小さく絞る）
        shape.arc = 360f;                                // 全方向に開く（半球的に）
        shape.angle = 20f;       // 完全に真上だけに出す（超直進）


        // 見た目の設定
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.material = ChangeMaterial(teamName, flowerMaterial); // 使用するマテリアル（色・画像）
        renderer.renderMode = ParticleSystemRenderMode.Billboard; // 常にカメラに正面を向ける
        shape.rotation = new Vector3(-90f, 0f, 0f);


        // 実際に粒子を出す
        ps.Emit(petals);

        // エフェクトを一定時間後に削除
        Destroy(bloomObj, duration);
    }


    /// <summary>
    /// やられふっとびエフェクトを再生
    /// </summary>
    /// <param name="position"></param>
    public void CreateDethEffect(Vector3 position, Team teamName)
    {
        GameObject bloomObj = new GameObject("FlowerBloom");
        bloomObj.transform.position = position;
        ParticleSystem ps = bloomObj.AddComponent<ParticleSystem>();
        var main = ps.main;
        main.startLifetime = duration;
        main.startSpeed = bloomSpeed;
        main.startSize = 0.4f;
        main.loop = false;
        main.maxParticles = petals;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.playOnAwake = false;

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.1f;
        shape.arc = 360f;

        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.material = ChangeMaterial(teamName, flowerMaterial);
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        shape.rotation = new Vector3(-90f, 0f, 0f);


        ps.Emit(petals);
        Destroy(bloomObj, duration + 1f); // 自動破棄
    }



    private Material ChangeMaterial(Team teamName, Material[] materialSet)
    {
        
        switch (teamName)
        {
            case Team.TeamOne:
                selectMaterial = materialSet[0];
                break;
            case Team.TeamTwo:
                selectMaterial = materialSet[1];
                break;
        }
        return selectMaterial;
    }
}
