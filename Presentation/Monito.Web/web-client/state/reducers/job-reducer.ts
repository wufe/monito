import { Reducer, AnyAction } from "redux";
import { JobState, getInitialJobState } from "~/state/states/job-state";
import { JobActions } from "../actions/job-actions";

export const jobReducer: Reducer<JobState> =
	(state: JobState = getInitialJobState(), action : AnyAction) => {
		switch (action.type) {
			case JobActions.AddLogMessage:
				return {
					...state,
					log: {
						...state.log,
						messages: [
							...state.log.messages,
							action.payload
						]
					}
				};
			case JobActions.SetStatusPolling:
				return {
					...state,
					polling: {
						...state.polling,
						status: action.payload
					}
				};
			case JobActions.SetJob:
				return {
					...state,
					job: action.payload
				};
			case JobActions.SetJobStatus:
				return {
					...state,
					job: {
						...(state.job || {}),
						status: action.payload
					}
				};
			case JobActions.AddLinksToCurrentJob:
				if (!state.job)
					return state;
				return {
					...state,
					job: {
						...state.job,
						links: [
							...state.job.links,
							...action.payload
						]
					}
				};
			case JobActions.ResetJob:
				return {
					...state,
					job: null
				};
			default:
				return state;
		}
	};