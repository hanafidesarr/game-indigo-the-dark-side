using UnityEngine;

public class LampuAntiTembus : MonoBehaviour
{
    public float panjangLampu = 1.0f;
    public LayerMask dindingLayer;

    private Vector3 posisiAwalLocal;

    void Start()
    {
        posisiAwalLocal = transform.localPosition;
    }

    void LateUpdate()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, panjangLampu, dindingLayer))
        {
            // Kalau kena tembok, tempel lampu ke depan tembok
            transform.position = hit.point - Camera.main.transform.forward * 0.1f;
        }
        else
        {
            // Kembalikan ke posisi lokal awal
            transform.localPosition = posisiAwalLocal;
        }
    }
}
