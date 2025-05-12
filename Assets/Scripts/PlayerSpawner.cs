using UnityEngine;
using UnityEngine.XR;

public class PlayerSpawner : MonoBehaviour
{
    public Transform xrRigRoot;

    public Transform xrCamera;

    public Transform spawnPoint;

    void Start()
    {
        SpawnPlayerAtPoint();
    }

    public void SpawnPlayerAtPoint()
    {
        if (xrRigRoot == null || xrCamera == null || spawnPoint == null)
        {
            return;
        }

        Vector3 cameraOffset = xrCamera.position - xrRigRoot.position;

        xrRigRoot.position = spawnPoint.position - new Vector3(cameraOffset.x, 0, cameraOffset.z);

        xrRigRoot.rotation = Quaternion.Euler(0, spawnPoint.rotation.eulerAngles.y, 0);

    }
}
