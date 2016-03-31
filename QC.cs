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
	[KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
	public partial class QuickContracts : MonoBehaviour {

		public static QuickContracts Instance {
			get;
			private set;
		}

		private void Awake() {
			if (Instance != null || HighLogic.CurrentGame.Mode != Game.Modes.CAREER) {
				Warning ("Destroy");
				Destroy (this);
				return;
			}
			Instance = this;
			QShortCuts.Awake ();
		}

		private void Start() {
			QSettings.Instance.Load ();
			QShortCuts.VerifyKey ();
			GameEvents.onGUIMissionControlDespawn.Add (OnGUIMissionControlDespawn);
			GameEvents.Contract.onDeclined.Add (OnDeclined);
		}

		private void OnDeclined(Contracts.Contract contract) {
			if (MissionControl.Instance == null) {
				return;
			}
			declineCost += HighLogic.CurrentGame.Parameters.Career.RepLossDeclined;
			declineContracts++;
			Log ("A contract has been declined!");
		}

		private void OnDestroy() {
			GameEvents.onGUIMissionControlDespawn.Remove (OnGUIMissionControlDespawn);
		}

		private void OnGUIMissionControlDespawn() {
			QSettings.Instance.Save ();
			if (QSettings.Instance.EnableMessage && declineCost > 0 && declineContracts > 0 & MessageSystem.Ready) {
				string _string = string.Format ("You have declined <b><color=#FF0000>{0}</color></b> contract(s).\nIt has cost you <color=#E0D503>¡<b>{1}</b></color>", declineContracts, declineCost);
				MessageSystem.Instance.AddMessage (new MessageSystem.Message (MOD, _string, MessageSystemButton.MessageButtonColor.ORANGE, MessageSystemButton.ButtonIcons.ALERT));
				declineContracts = 0;
				declineCost = 0;
			}
		}

		private void Update() {
			QShortCuts.Update ();
		}
	}
}