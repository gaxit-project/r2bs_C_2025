using UnityEngine;

public class BloomEffect : MonoBehaviour
{
    public Material[] flowerMaterial;
    [Header("爆発エフェクト")]
    public float bloomDuration = 2f;
    public float bloomSpeed = 1.0f;
    public float bloomSize = 0.75f;
    public int bloomPetals = 20;
    [Space(10)]

    [Header("ワープゲートエフェクト")]
    public float warpDuration = 2f;
    public float warpSpeed = 1.0f;
    public float warpSize = 0.75f;
    public int warpPetals = 20;
    [Space(10)]

    [Header("死亡エフェクト")]
    public float deadDuration = 2f;
    public float deadSpeed = 1.0f;
    public float deadSize = 0.75f;
    public int deadPetals = 20;
    [Space(10)]



    Material selectMaterial;


    public static BloomEffect Instance;
    private void Awake()
    {
        Instance = this;
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            CreateSpiralEffect(this.transform.position);
        }
        
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
        main.startLifetime = bloomDuration;         // 粒子の寿命
        main.startSpeed = bloomSpeed;          // 飛ぶスピード
        main.startSize = bloomSize;                 // 粒子の大きさ
        main.loop = false;                     // ループさせず、1回だけ出す
        main.maxParticles = bloomPetals;            // 最大粒子数
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
        ps.Emit(bloomPetals);

        // エフェクトを一定時間後に削除
        Destroy(bloomObj,bloomDuration);
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
        main.startLifetime = deadDuration;
        main.startSpeed = deadSpeed;
        main.startSize = 0.4f;
        main.loop = false;
        main.maxParticles = deadPetals;
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


        ps.Emit(deadPetals);
        Destroy(bloomObj, deadDuration + 1f); // 自動破棄
    }


    void CreateSpiralEffect(Vector3 position)
    {
        GameObject spiral = new GameObject("SpiralEffect");
        spiral.transform.position = position;

        var ps = spiral.AddComponent<ParticleSystem>();
        var main = ps.main;
        main.startLifetime = 2f;
        main.startSpeed = warpSpeed;
        main.startSize = warpSize;
        main.loop = false;
        main.maxParticles = warpPetals;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.playOnAwake = false;

        // 外周から発生
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 2f;
        shape.arc = 360f;

        // 回転運動
        var velocity = ps.velocityOverLifetime;
        velocity.enabled = true;
        velocity.orbitalY = 2f; // ぐるぐる回る（中心軸：Y）

        // 吸い込む力
        var force = ps.forceOverLifetime;
        force.enabled = true;
        force.x = new ParticleSystem.MinMaxCurve(-position.x * 0.5f);
        force.z = new ParticleSystem.MinMaxCurve(-position.z * 0.5f);

        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.material = flowerMaterial[0];

        ps.Emit(warpPetals);
        Destroy(spiral, warpDuration);
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
