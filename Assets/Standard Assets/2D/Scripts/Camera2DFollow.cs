using System;
using UnityEngine;

// Modified from standard assets
namespace UnityStandardAssets._2D
{
    public class Camera2DFollow : MonoBehaviour
    {
        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;

		private Transform target;
        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;

		public void SetTarget (Transform newTarget) {
			target = newTarget;
			m_OffsetZ = (transform.position - target.position).z;
			m_LastTargetPosition = transform.position - new Vector3(0, 0, m_OffsetZ);
		}
			
        private void Start() {
            transform.parent = null;
        }
			
        private void Update() {
			if (target) {
				// only update lookahead pos if accelerating or changed direction
				float xMoveDelta = (target.position - m_LastTargetPosition).x;

				bool updateLookAheadTarget = Mathf.Abs (xMoveDelta) > lookAheadMoveThreshold;

				if (updateLookAheadTarget) {
					m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign (xMoveDelta);
				} else {
					m_LookAheadPos = Vector3.MoveTowards (m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
				}

				Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward * m_OffsetZ;
				Vector3 newPos = Vector3.SmoothDamp (transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

				transform.position = newPos;

				m_LastTargetPosition = target.position;
			}
        }
    }
}
