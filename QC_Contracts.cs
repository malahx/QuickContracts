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
using System.Text.RegularExpressions;
using UnityEngine;

namespace QuickContracts {

	public class QContracts : Quick {

		internal static bool isClean {
			get {
				if (MissionControl.Instance != null) {
					return MissionControl.Instance.scrollListAvailable.LastClickedControl == null && MissionControl.Instance.scrollListActive.LastClickedControl == null && MissionControl.Instance.scrollListCancelled.LastClickedControl == null && MissionControl.Instance.scrollListCompleted.LastClickedControl == null && MissionControl.Instance.scrollListFailed.LastClickedControl == null && MissionControl.Instance.scrollListFinished.LastClickedControl == null;
				}
				return false;	
			}
		}

		internal static int MaxContractCount (MissionControlBuilding MCBuilding) {
			//int tier1, tier2, tier3;
			//ContractSystem.GetContractCounts (Reputation.CurrentRep, ContractSystem.Instance.GetActiveContractCount ()+1, out tier1, out tier2, out tier3);
			return (MCBuilding.Facility.FacilityLevel == MCBuilding.Facility.MaxLevel ? ContractSystem.Instance.GetActiveContractCount ()+1 : (MCBuilding.Facility.FacilityLevel == MCBuilding.Facility.MaxLevel -1 ? 7 : 2));
		}

		internal static void AcceptOrDecline(bool accept) {
			if (MissionControl.Instance.scrollListAvailable.LastClickedControl != null) {
				MissionControl.MissionSelection _mission = (MissionControl.MissionSelection)MissionControl.Instance.scrollListAvailable.LastClickedControl.Data;
				Contract _contract = _mission.contract;
				MissionControl.Instance.scrollListAvailable.RemoveItem (_mission.listItem.container, false);
				if (accept) {
					_contract.Accept ();
					MissionControl.Instance.AddItemActive (_contract);
					RefreshUIControls ();
					Log ("Accept: " + _contract.Title);
				} else {
					_contract.Decline ();
					Log ("Decline: " + _contract.Title);
				}
				QContracts.Clear ();
			}
		}

		internal static void RefreshUIControls() {
			MissionControlBuilding[] _MCBuildings = (MissionControlBuilding[])Resources.FindObjectsOfTypeAll(typeof(MissionControlBuilding));
			MissionControlBuilding _MCBuilding = _MCBuildings[0];
			int _ActiveContractCount = ContractSystem.Instance.GetActiveContractCount ();
			if (_MCBuilding.Facility.FacilityLevel == _MCBuilding.Facility.MaxLevel) {
				MissionControl.Instance.statsTextField.Text = string.Format("<b><#DB8310>Active Contracts: </></b> {0}", _ActiveContractCount);
				MissionControl.Instance.btnAccept.SetControlState (UIButton.CONTROL_STATE.NORMAL);
			} else {
				int _MaxContractCount = MaxContractCount (_MCBuilding);
				MissionControl.Instance.statsTextField.Text = string.Format ((_MaxContractCount > _ActiveContractCount ? "<b><#DB8310>Active Contracts: </></b> {0}\t[Max: {1}]" : "<#f97306><b>Active Contracts: {0}\t[Max: {1}]</b> </>"), _ActiveContractCount, _MaxContractCount);
				if (_MaxContractCount <= _ActiveContractCount) {
					MissionControl.Instance.btnAccept.SetControlState (UIButton.CONTROL_STATE.DISABLED);
				}
			}
		}

		internal static void DisableContract(Type ContractType) {
			if (ContractSystem.ContractTypes.Contains (ContractType)) {
				ContractSystem.ContractTypes.Remove (ContractType);
				Log ("Disable: " + ContractType.Name);
				DeclineAll (ContractType);
			}
		}

		internal static void DeclineAll(Type ContractType) {
			List<Contract> _contracts = ContractSystem.Instance.Contracts;
			foreach (Contract _contract in _contracts) {
				if (_contract.ContractState == Contract.State.Offered && _contract.CanBeDeclined () && _contract.GetType() == ContractType) {
					_contract.Decline ();
				}
			}
			Clear ();
			CleanLists (true);
			Log ("Decline all: " + ContractType.Name);

		}

		internal static void DeclineAll() {
			List<Contract> _contracts = ContractSystem.Instance.Contracts;
			foreach (Contract _contract in _contracts) {
				if (_contract.ContractState == Contract.State.Offered && _contract.CanBeDeclined ()) {
					_contract.Decline ();
				}
			}
			Clear ();
			CleanLists (true);
			Log ("Decline all contracts");
		}

		internal static void Clear() {
			if (MissionControl.Instance != null) {
				MissionControl.Instance.UpdateInfoPanelContract (null);
				MissionControl.Instance.UpdateInfoPanelAgent (null);
				MissionControl.Instance.missionPanelManager.Dismiss (UIPanelManager.MENU_DIRECTION.Forwards);
			}
		}

		internal static void CleanLists(bool force = false) {
			if (MissionControl.Instance != null) {
				if (force || MissionControl.Instance.scrollListAvailable.LastClickedControl != null || MissionControl.Instance.scrollListActive.LastClickedControl != null) {
					MissionControl.Instance.scrollListAvailable.ClearList (true);
					MissionControl.Instance.scrollListActive.ClearList (true);
					List<Contract> _contracts = ContractSystem.Instance.Contracts;
					foreach (Contract _contract in _contracts) {
						if (_contract.ContractState == Contract.State.Active) {
							MissionControl.Instance.AddItemActive (_contract);
						} else if (_contract.ContractState == Contract.State.Offered) {
							MissionControl.Instance.AddItemAvailable (_contract);
						}
					}
				}
				if (force || MissionControl.Instance.scrollListCancelled.LastClickedControl != null || MissionControl.Instance.scrollListCompleted.LastClickedControl != null || MissionControl.Instance.scrollListFailed.LastClickedControl != null || MissionControl.Instance.scrollListFinished.LastClickedControl != null) {
					MissionControl.Instance.scrollListCancelled.ClearList (true);
					MissionControl.Instance.scrollListCompleted.ClearList (true);
					MissionControl.Instance.scrollListFailed.ClearList (true);
					MissionControl.Instance.scrollListFinished.ClearList (true);
					List<Contract> _contracts = ContractSystem.Instance.ContractsFinished;
					foreach (Contract _contract in _contracts) {
						if (_contract.ContractState == Contract.State.Completed) {
							MissionControl.Instance.AddItemCompleted (_contract);
						} else if (_contract.ContractState == Contract.State.Failed) {
							MissionControl.Instance.AddItemFailed (_contract);
						} else if (_contract.ContractState == Contract.State.Cancelled) {
							MissionControl.Instance.AddItemCancelled (_contract);
						}
						MissionControl.Instance.AddItemFinished (_contract, _contract.Title);
					}
				}
			}
		}
	}
}