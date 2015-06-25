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

	public partial class QuickContracts : Quick {

		internal static bool isClean {
			get {
				if (MissionControl.Instance == null) {
					return false;	
				}
				return MissionControl.Instance.scrollListAvailable.LastClickedControl == null && MissionControl.Instance.scrollListActive.LastClickedControl == null && MissionControl.Instance.scrollListCancelled.LastClickedControl == null && MissionControl.Instance.scrollListCompleted.LastClickedControl == null && MissionControl.Instance.scrollListFailed.LastClickedControl == null && MissionControl.Instance.scrollListFinished.LastClickedControl == null;
			}
		}

		internal static void Accept() {
			if (MissionControl.Instance == null) {
				return;
			}
			if (MissionControl.Instance.btnAccept.controlState != UIButton.CONTROL_STATE.NORMAL) {
				return;
			}
			POINTER_INFO _pinfo = new POINTER_INFO ();
			_pinfo.evt = POINTER_INFO.INPUT_EVENT.TAP;
			MissionControl.Instance.btnAccept.OnInput (_pinfo);
			Log ("Accepted a new contract");
		}

		internal static void Decline() {
			if (MissionControl.Instance == null) {
				return;
			}
			if (MissionControl.Instance.btnDecline.controlState != UIButton.CONTROL_STATE.NORMAL) {
				return;
			}
			POINTER_INFO _pinfo = new POINTER_INFO ();
			_pinfo.evt = POINTER_INFO.INPUT_EVENT.TAP;
			MissionControl.Instance.btnDecline.OnInput (_pinfo);
			Log ("Declined an old contract");
		}

		internal static void DeclineAll(Type ContractType) {
			if (ContractSystem.Instance == null) {
				return;
			}
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
			if (ContractSystem.Instance == null) {
				return;
			}
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
			if (MissionControl.Instance == null) {
				return;
			}
			MissionControl.Instance.UpdateInfoPanelContract (null);
			MissionControl.Instance.UpdateInfoPanelAgent (null);
			MissionControl.Instance.missionPanelManager.Dismiss (UIPanelManager.MENU_DIRECTION.Forwards);
		}

		internal static void CleanLists(bool force = false) {
			if (MissionControl.Instance == null) {
				return;
			}
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