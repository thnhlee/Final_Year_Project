using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
[AddComponentMenu("ThinhLe/ScreenShake")]

public class ScreenShake : Singleton<ScreenShake>
{
    private CinemachineImpulseSource source;

    protected override void Awake()
    {
        base.Awake();

        source = GetComponent<CinemachineImpulseSource>();
    }

    public void ShakeScreen()
    {
        source.GenerateImpulse();
    }
}
