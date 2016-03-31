/* 
QuickContracts
Copyright 2016 Malah

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

using KSP.UI.Screens;
using System;
using UnityEngine;

namespace QuickContracts {
	public class QShortCuts : QKey {

		internal static void Awake() {
			RectSetKey = new Rect ((Screen.width - 300) / 2, (Screen.height - 100) / 2, 300, 100);
			string[] _keys = Enum.GetNames (typeof(Key));
			int _length = _keys.Length;
			for (int _key = 1; _key < _length; _key++) {
				Key _GetKey = (Key)_key;
				SetCurrentKey (_GetKey, DefaultKey (_GetKey));
			}
		}

		internal static void Update() {
			if (MissionControl.Instance == null) {
				return;
			}
			/*if (SetKey != Key.None) {
				if (Event.current.isKey) {
					KeyCode _key = Event.current.keyCode;
					if (_key != KeyCode.None) {
						SetCurrentKey (SetKey, _key);
						QSettings.Instance.Save ();
						SetKey = Key.None;
						QGUI.WindowSettings = true;
					}
				}
				return;
			}*/
			if (Input.GetKeyDown (QSettings.Instance.KeyAcceptSelectedContract)) {
				QuickContracts.Accept ();
			}
			if (Input.GetKeyDown (QSettings.Instance.KeyDeclineSelectedContract)) {
				QuickContracts.Decline ();
			}					
			if (Input.GetKeyDown (QSettings.Instance.KeyDeclineAllContracts)) {
				QuickContracts.DeclineAll ();
			}
			if (Input.GetKeyDown (QSettings.Instance.KeyDeclineAllTest)) {
				QuickContracts.DeclineAll (typeof(Contracts.Templates.PartTest));
			}
		}

		/*internal static void OnGUI() {
			if (QGUI.isMissionControl) {
				return;
			}
			if (SetKey != Key.None) {
				RectSetKey = GUILayout.Window (1545146, RectSetKey, DrawSetKey, string.Format ("Set Key: {0}", GetText (SetKey)), GUILayout.Width (RectSetKey.width), GUILayout.ExpandHeight (true));
			}
		}*/
	}
}