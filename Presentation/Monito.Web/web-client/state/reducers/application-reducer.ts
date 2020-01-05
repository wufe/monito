import { Reducer, AnyAction } from "redux";
import { ApplicationState, getInitialApplicationState } from "~/state/states/application-state";
import { ApplicationActions } from "~/state/actions/application-actions";

export const applicationReducer: Reducer<ApplicationState> =
	(state: ApplicationState = getInitialApplicationState(), action: AnyAction) => {
		switch (action.type) {
			case ApplicationActions.SetLoading:
				let loadingCount = state.loadingCount;
				if (action.payload === true) {
					loadingCount++;
				} else {
					loadingCount--;
				}
				const loading = loadingCount !== 0;
				return { ...state, loading, loadingCount };
			default:
				return state;
		}
	};