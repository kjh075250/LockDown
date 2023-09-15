using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffect : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource _impSource;
    [SerializeField] private CinemachineImpulseSource _ExpimpSource;
    [SerializeField] private CinemachineImpulseSource _LaserimpSource;

    void Start()
    {
        EventManager.Instance.StopListening(EventManager.EventFlags.LaserAttack, CameraShake);
        EventManager.Instance.StopListening(EventManager.EventFlags.MissileExplosion, CameraExplosionShake);
        EventManager.Instance.StopListening(EventManager.EventFlags.LaserHit, CameraLaserShake);

        EventManager.Instance.AddListener(EventManager.EventFlags.LaserAttack, CameraShake);
        EventManager.Instance.AddListener(EventManager.EventFlags.MissileExplosion, CameraExplosionShake);
        EventManager.Instance.AddListener(EventManager.EventFlags.LaserHit, CameraLaserShake);
    }


    private void CameraShake()
    {
        _impSource.GenerateImpulse();
    }


    private void CameraExplosionShake()
    {
        _ExpimpSource.GenerateImpulse();
    }

    private void CameraLaserShake()
    {
        _LaserimpSource.GenerateImpulse();
    }
}
