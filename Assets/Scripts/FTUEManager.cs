using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FTUEManager : MonoBehaviour
{
    [SerializeField] private GameObject _rotationGuide = null;
    [SerializeField] private GameObject _tapScreenGuide = null;

    private GravityManager _gravityManager = null;
    private PlayerController _playerController = null;
    private LevelManager _levelManager = null;

    private float _rotationTimer = 5f;

    private IEnumerator Start()
    {
        _gravityManager = FindObjectOfType<GravityManager>();
        _playerController = FindObjectOfType<PlayerController>();
        _levelManager = FindObjectOfType<LevelManager>();

        yield return new WaitUntil(() => _levelManager.LevelStarted);

        StartCoroutine(RotationFTUERoutine());
        StartCoroutine(TapFTUERoutine());
    }

    private IEnumerator RotationFTUERoutine()
    {
        yield return new WaitForSeconds(_rotationTimer);

		if (!_gravityManager.MovedEnough)
		{
			_rotationGuide.SetActive(true);
		}
		else
			yield break;

        yield return new WaitUntil(() => _gravityManager.MovedEnough);
        _rotationGuide.SetActive(false);
    }

    private IEnumerator TapFTUERoutine()
    {
        yield return new WaitUntil(() => _levelManager.CurrentCollectableCount == 2);

		if (!_playerController.HasTapped)
		{
			_tapScreenGuide.SetActive(true);
		}
		else
			yield break;

        yield return new WaitUntil(() => _playerController.HasTapped);
        _tapScreenGuide.SetActive(false);
    }


}
