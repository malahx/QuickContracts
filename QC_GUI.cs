/* 
QuickContracts
Copyright 2015 Malah

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>. 
*/

using System;
using System.Linq;
using UnityEngine;

namespace QuickContracts {

	internal class QGUI : MonoBehaviour {

		internal static bool WindowSettings = false;
		private static string Settings_TexturePath = QuickContracts.MOD + "/Textures/Settings";
		private static Texture2D Settings_Texture {
			get {
				return GameDatabase.Instance.GetTexture (Settings_TexturePath, false);
			}
		}

		internal static bool isMissionControl {
			get {
				return MissionControl.Instance != null;
			}
		}

		public static Vector3 GetPosition(Transform trans) {
			EZCameraSettings _uiCam = UIManager.instance.uiCameras.FirstOrDefault(c => (c.mask & (1 << trans.gameObject.layer)) != 0);
			if (_uiCam != null) {
				Vector3 _screenPos = _uiCam.camera.WorldToScreenPoint (trans.position);
				_screenPos.y = Screen.height - _screenPos.y;
				return _screenPos;
			}
			return Vector3d.zero;
		}

		// AFFICHAGE DE L'INTERFACE
		internal static void OnGUI() {
			if (!isMissionControl) {
				return;
			}
			// Bouton de configuration
			Vector3 _posInfo = GetPosition(MissionControl.Instance.missionInfoPanelButtons.transform);
			Vector3 _posButton = GetPosition (MissionControl.Instance.missionPanelTabGroup.transform);
			GUILayout.BeginArea (new Rect (_posInfo.x - 22 - 14, _posButton.y - 22, 22, 22));
			if (GUILayout.Button (new GUIContent (Settings_Texture, "QuickContracts"), GUILayout.Width (22), GUILayout.Height (22))) {
				QuickContracts.CleanLists ();
				QuickContracts.Clear ();
				WindowSettings = !WindowSettings;
			}
			GUILayout.EndArea ();
			if (QuickContracts.isClean) {
				if (WindowSettings) {
					GUILayout.BeginArea (new Rect (_posInfo.x, _posInfo.y, 465, 45));
					GUILayout.Box ("QuickContracts", GUILayout.Height (30));
					GUILayout.EndArea ();
					GUILayout.BeginArea (new Rect (_posInfo.x, _posInfo.y + 45, 595 / 2, Screen.height - 5));
					GUILayout.BeginVertical ();
					GUILayout.BeginHorizontal ();
					GUILayout.Box ("Shortcuts", GUILayout.Height (30));
					GUILayout.EndHorizontal ();
					GUILayout.Space (5);
					QShortCuts.DrawConfigKey (QShortCuts.Key.AcceptSelectedContract);
					QShortCuts.DrawConfigKey (QShortCuts.Key.DeclineSelectedContract);
					QShortCuts.DrawConfigKey (QShortCuts.Key.DeclineAllContracts);
					QShortCuts.DrawConfigKey (QShortCuts.Key.DeclineAllTest);
					GUILayout.BeginHorizontal ();
					GUILayout.Box ("Options", GUILayout.Height (30));
					GUILayout.EndHorizontal ();
					GUILayout.Space (5);
					GUILayout.BeginHorizontal ();
					QSettings.Instance.EnableMessage = GUILayout.Toggle (QSettings.Instance.EnableMessage, "Enable message for declined contracts", GUILayout.Width (200));
					GUILayout.EndHorizontal ();
					GUILayout.Space (5);
					GUILayout.EndVertical ();
					GUILayout.EndArea ();
				}
			} else if (WindowSettings) {
				WindowSettings = false;
			}
		}
	}
}