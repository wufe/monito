import { useEffect } from "react";
import { useSelector, useDispatch } from "react-redux";
import { GlobalState } from "~/state/state";
import { JobStatus } from "~/types/job";
import { ApplicationThunkDispatch } from "~/thunk/thunk";
import { requestJobUpdates, deactivateJobUpdatesStream, loadJobThunk, refreshJobStatusThunk } from "~/thunk/job-thunk";

export const useJobFetch = (userUUID: string, jobUUID: string) => {
	const dispatch = useDispatch<ApplicationThunkDispatch>();

	useEffect(() => {
		const load = dispatch(loadJobThunk(userUUID, jobUUID));
		let refreshStatusTimer: any = null;
		load
			.then(response => {
				refreshStatusTimer = setInterval(() => {
					dispatch(refreshJobStatusThunk(userUUID, jobUUID));
				}, 5000);
			});

		return () => {
			clearInterval(refreshStatusTimer);
		};
	}, []);
};

export const useRealtimeJobUpdates = (userUUID: string, jobUUID: string) => {
	const dispatch = useDispatch<ApplicationThunkDispatch>();
	const jobStatus = useSelector<GlobalState, JobStatus | null>(state => {
		if (!state.job.job)
			return null;
		return state.job.job.status;
	});

	useEffect(() => {
		if (jobStatus === JobStatus.INPROGRESS || jobStatus === JobStatus.DONE) {
			dispatch(requestJobUpdates());
		}

		return () => {
			dispatch(deactivateJobUpdatesStream());
		};
	}, [jobStatus]);
}