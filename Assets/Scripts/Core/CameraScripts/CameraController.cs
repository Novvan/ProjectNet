using MoreMountains.Tools;
using Photon.Pun;
using ProjectNet.Utils;
using UnityEngine;

namespace ProjectNet.Core.CameraScripts
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] private Vector3 offset;
		private Transform _target;
		private Camera _cam;

		public Vector3 Offset => offset;

		private void Start()
		{
			_cam = Tools.Cam;
		}

		private void Update()
		{
			if (_target != null) FollowTarget(_target);
		}

		public void SetTarget(Transform target, Vector3 newOffset)
		{
			_target = target;
			offset = newOffset;
		}

		private void FollowTarget(Transform target)
		{
			var pos = target.position + offset;
			var camTransform = _cam.transform;
			pos.z = camTransform.position.z;
			camTransform.position = pos;
		}
	}
}
