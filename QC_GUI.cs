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

using Contracts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuickContracts {

	internal class QGUI : MonoBehaviour {

		internal static bool WindowSettings = false;
		private static string Settings_TexturePath = Quick.MOD + "/Textures/Settings";
		private static Texture2D Settings_Texture;

		internal static void Awake() {
			Settings_Texture = GameDatabase.Instance.GetTexture (Settings_TexturePath, false);
		}

		internal static bool isMissionControl {
			get {
				return HighLogic.LoadedSceneIsGame && HighLogic.CurrentGame.Mode == Game.Modes.CAREER && HighLogic.LoadedScene == GameScenes.SPACECENTER && MissionControl.Instance != null;
			}
		}

		// AFFICHAGE DE L'INTERFACE
		internal static void OnGUI() {
				if (isMissionControl) {
				// Bouton de configuration
				GUILayout.BeginArea (new Rect (478, 182, 22, 22));
				if (GUILayout.Button (new GUIContent (Settings_Texture, "QuickContracts"), GUILayout.Width(22), GUILayout.Height(22))) {
					QContracts.CleanLists ();
					QContracts.Clear();
					WindowSettings = !WindowSettings;
				}
				GUILayout.EndArea ();
				if (QContracts.isClean) {
					if (WindowSettings) {
						GUILayout.BeginArea (new Rect (515, 180, 465, 45));
						GUILayout.Box ("QuickContracts", GUILayout.Height (30));
						GUILayout.EndArea ();
						GUILayout.BeginArea (new Rect (515, 215, 595 / 2, Screen.height - 5));
						GUILayout.BeginVertical ();
						GUILayout.BeginHorizontal ();
						GUILayout.Box ("Shortcuts", GUILayout.Height (30));
						GUILayout.EndHorizontal ();
						GUILayout.Space (5);
						QShortCuts.DrawConfigKey (QShortCuts.Key.AcceptSelectedContract);
						QShortCuts.DrawConfigKey (QShortCuts.Key.DeclineSelectedContract);
						QShortCuts.DrawConfigKey (QShortCuts.Key.DeclineAllContracts);
						QShortCuts.DrawConfigKey (QShortCuts.Key.DeclineAllTest);
						GUILayout.EndVertical ();
						GUILayout.EndArea ();
					}
				} else if (WindowSettings) {
					WindowSettings = false;
				}
			}
		}
	}
}