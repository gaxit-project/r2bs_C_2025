using UnityEngine;

public class TestExp : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            levelManager.AddExp(1);
            Debug.Log("�o���l+1");
        }
    }
}
