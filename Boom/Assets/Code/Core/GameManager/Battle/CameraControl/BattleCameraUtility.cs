using UnityEngine;
using DG.Tweening;


public static class BattleCameraUtility
{
    public static void ShakeCamera(float duration = 0.3f, Vector3 _strength = default,int _vibrato = 20,float _randomness = 20f)
    {
        Camera mainCamera = Camera.main;
        //Debug.Log("ShakeCamera");
        Vector3 startPos = new Vector3(mainCamera.transform.position.x, 1, mainCamera.transform.position.z);
        mainCamera.transform.position = startPos;
        Vector3 endPos = new Vector3(startPos.x + 2f, startPos.y, startPos.z);
        // 创建一个序列
        Sequence cameraSequence = DOTween.Sequence();
        //参数构建
        if (_strength == default)
            _strength = new Vector3(1f, 1.5f, 0f);
        //震屏
        cameraSequence.Append(mainCamera.transform.DOShakePosition(duration, strength: _strength ,vibrato:_vibrato, randomness: _randomness));
        cameraSequence.Append(mainCamera.transform.DOMove(endPos, 3f));
    }
}