using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;


namespace Scripts.UI
{

	[RequireComponent( typeof( RectTransform ) )]
	public class JoystickUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
	{
		public RectTransform handle;
		public Vector2 autoReturnSpeed = new Vector2( 4.0f, 4.0f );
		public float radius = 40.0f;

		public event Action<Vector2> OnStartJoystickMovement;
		public event Action<Vector2> OnJoystickMovement;
		public event Action OnEndJoystickMovement;

		private bool _returnHandle;
		private RectTransform _canvas;

		public Vector2 Coordinates {
			get {
				if( handle.anchoredPosition.magnitude < radius )
					return handle.anchoredPosition / radius;
				return handle.anchoredPosition.normalized;
			}
		}

		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			_returnHandle = false;
			var handleOffset = GetJoystickOffset( eventData );
			handle.anchoredPosition = handleOffset;
			if( OnStartJoystickMovement != null )
				OnStartJoystickMovement( Coordinates );
		}

		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			var handleOffset = GetJoystickOffset( eventData );
			handle.anchoredPosition = handleOffset;
			if( OnJoystickMovement != null )
				OnJoystickMovement( Coordinates );
		}

		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			_returnHandle = true;
			if( OnEndJoystickMovement != null )
				OnEndJoystickMovement( );
		}

		private Vector2 GetJoystickOffset(PointerEventData eventData)
		{
			Vector3 globalHandle;
			if( RectTransformUtility.ScreenPointToWorldPointInRectangle( _canvas, eventData.position, eventData.pressEventCamera, out globalHandle ) )
				handle.position = globalHandle;
			var handleOffset = handle.anchoredPosition;
			if( handleOffset.magnitude > radius ) {
				handleOffset = handleOffset.normalized * radius;
				handle.anchoredPosition = handleOffset;
			}
			return handleOffset;
		}

		private void Start()
		{
			_returnHandle = true;
			var touchZone = GetComponent<RectTransform>();
			touchZone.pivot = Vector2.one * 0.5f;
			handle.transform.SetParent( transform );
			var curTransform = transform;
			do {
				if( curTransform.GetComponent<Canvas>() != null ) {
					_canvas = curTransform.GetComponent<RectTransform>();
					break;
				}
				curTransform = curTransform.parent;
			} while ( curTransform != null );
		}

		private void Update()
		{
			if( _returnHandle )
			if( handle.anchoredPosition.magnitude > Mathf.Epsilon )
				handle.anchoredPosition -= new Vector2( handle.anchoredPosition.x * autoReturnSpeed.x, handle.anchoredPosition.y * autoReturnSpeed.y ) * Time.deltaTime;
			else
				_returnHandle = false;
		}
	}
}