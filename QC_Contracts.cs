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
using Contracts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace QuickContracts {

	public partial class QuickContracts : MonoBehaviour {

		internal static float declineCost = 0;
		internal static float declineContracts = 0;

		internal static void Accept() {
			if (MissionControl.Instance == null) {
				return;
			}
			if (!MissionControl.Instance.toggleDisplayModeAvailable.isOn) {
				Log ("You are not on the Available contracts");
				return;
			}
			if (!MissionControl.Instance.btnAccept.IsInteractable()) {
				Log ("Can't accept this contract");
				return;
			}
			int _active = ContractSystem.Instance.Contracts.FindAll (c => c.ContractState == Contract.State.Active).Count;
			int _accept = GameVariables.Instance.GetActiveContractsLimit (ScenarioUpgradeableFacilities.GetFacilityLevel (SpaceCenterFacility.MissionControl));
			if (_active >= _accept) {
				Log ("You can't accept a new contract, you have " + _active + " active contracts and you can accept " + _accept + " contracts.");
				return;
			}
			MissionControl.Instance.btnAccept.onClick.Invoke ();
			Log ("Accepted a contract");
		}

		internal static void Decline() {
			if (MissionControl.Instance == null) {
				return;
			}
			if (!MissionControl.Instance.toggleDisplayModeAvailable.isOn) {
				Log ("You are not on the Available contracts");
				return;
			}
			if (!MissionControl.Instance.btnDecline.IsInteractable()) {
				return;
			}
			MissionControl.Instance.btnDecline.onClick.Invoke ();
			Log ("Declined a contract");
		}

		internal static void DeclineAll(Type ContractType) {
			if (MissionControl.Instance == null) {
				return;
			}
			if (!MissionControl.Instance.toggleDisplayModeAvailable.isOn) {
				Log ("You are not on the Available contracts");
				return;
			}
			if (ContractSystem.Instance == null) {
				return;
			}
			List<Contract> _contracts = ContractSystem.Instance.Contracts;
			for (int i = 0; i < _contracts.Count; i++) {
				Contract _contract = _contracts [i];
				if (_contract.ContractState == Contract.State.Offered && _contract.CanBeDeclined () && _contract.GetType() == ContractType) {
					_contract.Decline ();
				}
			}
			MissionControl.Instance.RebuildContractList ();
			Log ("Decline all: " + ContractType.Name);
		}

		internal static void DeclineAll() {
			if (MissionControl.Instance == null) {
				return;
			}
			if (!MissionControl.Instance.toggleDisplayModeAvailable.isOn) {
				Log ("You are not on the Available contracts");
				return;
			}
			if (ContractSystem.Instance == null) {
				return;
			}
			List<Contract> _contracts = ContractSystem.Instance.Contracts;
			for (int i = 0; i < _contracts.Count; i++) {
				Contract _contract = _contracts [i];
				if (_contract.ContractState == Contract.State.Offered && _contract.CanBeDeclined ()) {
					_contract.Decline ();
				}
			}
			MissionControl.Instance.RebuildContractList ();
			Log ("Decline all contracts");
		}
	}
}