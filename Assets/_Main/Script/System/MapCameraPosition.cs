using UnityEngine;

public class MapCameraPosition : MonoBehaviour
{
    [SerializeField] public Camera mapCamera;
    void Start()
    {
        Vector3 mapCenter = MapManager.Instance.GetMapCenterPosition();
        // ƒJƒƒ‰‚ğ’†‰›‚ÉˆÚ“®
        mapCamera.transform.position = new Vector3(mapCenter.x, 31.5f, mapCenter.z-1f);
        // ƒJƒƒ‰‚ª^‰º‚ğŒü‚­‚æ‚¤‚É’²®
        mapCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

    }
}
