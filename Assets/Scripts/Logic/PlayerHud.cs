//
//  PlayerHud.cs
//
//  Author:
//       ${wuxingogo} <52111314ly@gmail.com>
//
//  Copyright (c) 2016 ly-user
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using UnityEngine;
using Scripts.UI;
using Scripts.Node;
using UnityEngine.Networking;


namespace Scripts.Logic
	{

	public class PlayerHud : NetworkBehaviour
	{
		[SerializeField] float speed = 0.5f;
		[SerializeField] Transform playerTransform = null;
		[SerializeField] Warlock playerUnit = null;
		Vector3 offsetVec = Vector3.zero;
		[SerializeField] JoystickUI joy = null;

		void Awake()
		{
			transform.position = new Vector3(0, 4.12f, 15.4f);
		}

		void Start()
		{
			if (isLocalPlayer)
			{
				CorrectCamera();
				joy = UIManager.GetInstance().joy;
				joy.OnJoystickMovement += CmdOnMovement;
				joy.OnStartJoystickMovement += CmdOnStartment;
				joy.OnEndJoystickMovement += CmdOnEndment;
				joy.gameObject.SetActive(true);
			}
		}
		[Command]
		void CmdSynch()
		{
			
		}
		[Command]
		void CmdOnStartment(Vector2 vector2)
		{
			offsetVec = new Vector3(vector2.x, 0, vector2.y);
			playerUnit.TwistHead(offsetVec);
			playerUnit.OnStartMovement();
		}
		[Command]
		void CmdOnEndment()
		{
			offsetVec = Vector3.zero;
			playerUnit.OnEndMovement();

		}
		[Command]
		void CmdOnMovement(Vector2 vector2)
		{
			offsetVec = new Vector3(vector2.x, 0, vector2.y).normalized;
			
			playerUnit.TwistHead(offsetVec);
		}

		void CorrectCamera()
		{
			var cameraGo = GameObject.Find("CameraContainer");
			cameraGo.transform.SetParent(transform);
			cameraGo.transform.localPosition = Vector3.zero;
		}
	}
	}

