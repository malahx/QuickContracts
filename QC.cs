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
using System.IO;
using UnityEngine;

namespace QuickContracts {

	public class Quick : MonoBehaviour {
		public readonly static string VERSION = "0.10";
		public readonly static string MOD = "QuickContracts";
		private static bool isdebug = true;

		// Afficher les messages sur la console
		internal static void Log(string _string) {
			if (isdebug) {
				Debug.Log (MOD + "(" + VERSION + "): " + _string);
			}
		}
		internal static void Warning(string _string) {
			if (isdebug) {
				Debug.LogWarning (MOD + "(" + VERSION + "): " + _string);
			}
		}
	}

	[KSPAddon(KSPAddon.Startup.EveryScene, false)]
	public class QuickContracts : Quick {

		// CHARGER LA CONFIGURATION ET DESACTIVER LES CONTRATS
		private void Start() {
			Settings.Instance.Load ();
			GameEvents.onGUIMissionControlSpawn.Add (OnGUIMissionControlSpawn);
			GameEvents.Contract.onContractsLoaded.Add (OnContractsLoaded);
		}

		private void OnDestroy() {
			GameEvents.onGUIMissionControlSpawn.Remove (OnGUIMissionControlSpawn);
			GameEvents.Contract.onContractsLoaded.Remove (OnContractsLoaded);
		}

		private void OnGUIMissionControlSpawn() {
			Settings.Instance.Load ();
			OnContractsLoaded ();
		}

		private void OnContractsLoaded() {
			if (!Settings.Instance.TestContracts) {
				QContracts.DisableContract(typeof(Contracts.Templates.PartTest));
			}
			if (!Settings.Instance.RescueContracts) {
				QContracts.DisableContract(typeof(Contracts.Templates.RescueKerbal));
			}
			if (!Settings.Instance.ScienceDataContracts) {
				QContracts.DisableContract(typeof(Contracts.Templates.CollectScience));
			}
			if (!Settings.Instance.SurveyContracts) {
				QContracts.DisableContract(typeof(FinePrint.Contracts.SurveyContract));
			}
			if (!Settings.Instance.StationContracts) {
				QContracts.DisableContract(typeof(FinePrint.Contracts.StationContract));
			}
			if (!Settings.Instance.BaseContracts) {
				QContracts.DisableContract(typeof(FinePrint.Contracts.BaseContract));
			}
			if (!Settings.Instance.SatelliteContracts) {
				QContracts.DisableContract(typeof(FinePrint.Contracts.SatelliteContract));
			}
			if (!Settings.Instance.ISRUContracts) {
				QContracts.DisableContract(typeof(FinePrint.Contracts.ISRUContract));
			}
			if (!Settings.Instance.ARMContracts) {
				QContracts.DisableContract(typeof(FinePrint.Contracts.ARMContract));
			}
		}

		private void Update() {
			if (HighLogic.LoadedScene == GameScenes.SPACECENTER) {
				if (MissionControl.Instance != null) {
					bool _accept = Input.GetKeyDown (Settings.Instance.KeyAcceptSelectedContract);
					bool _decline = Input.GetKeyDown (Settings.Instance.KeyDeclineSelectedContract);
					if (_accept || _decline) {
						if (MissionControl.Instance.scrollListAvailable.LastClickedControl != null) {
							MissionControl.MissionSelection _mission = (MissionControl.MissionSelection)MissionControl.Instance.scrollListAvailable.LastClickedControl.Data;
							Contract _contract = _mission.contract;
							MissionControl.Instance.scrollListAvailable.RemoveItem (_mission.listItem.container, false);
							if (_accept) {
								_contract.Accept ();
								MissionControl.Instance.AddItemActive (_contract);
								Log ("Accept: " + _contract.Title);
							} else if (_decline) {
								_contract.Decline ();
								Log ("Decline: " + _contract.Title);
							}
							QContracts.Clear ();
						}
					}
					if (Input.GetKeyDown (Settings.Instance.KeyDeclineAllContracts)) {
						QContracts.DeclineAll ();
					}
					if (Input.GetKeyDown (Settings.Instance.KeyDeclineAllTest)) {
						QContracts.DeclineAll (typeof(Contracts.Templates.PartTest));
					}
				}
			}
		}
	}
}