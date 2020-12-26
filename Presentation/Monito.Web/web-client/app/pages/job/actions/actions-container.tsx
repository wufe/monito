import * as React from 'react';
import { Actions } from './actions';
import { useSelector, useDispatch } from 'react-redux';
import { GlobalState } from '~/state/state';
import { JobStatus, JobModel } from '~/types/job';
import { ApplicationThunkDispatch } from '~/thunk/thunk';
import { useParams } from 'react-router-dom';
import { downloadCSV, abortJob } from '~/thunk/job-thunk';

export const ActionsContainer = () => {

	const dispatch = useDispatch<ApplicationThunkDispatch>();
	const { userUUID, jobUUID } = useParams<{ userUUID: string; jobUUID: string; }>();
	const [downloading, setDownloading] = React.useState({
		CSV : false,
		JSON: false,
	});

	const [aborting, setAborting] = React.useState(false);

	const job = useSelector<GlobalState, JobModel>(state => {
		if (!state.job.job)
			return null;
		return state.job.job;
	});

	const status = job && job.status;

	const showTruncatedMessage = job === null ? false : (job.doneLinksCount && job.doneLinksCount > job.links.length);

	let showAvatar = true;
	if (showTruncatedMessage)
		showAvatar = false;

	const onDownloadCSVClick = () => {
		if (downloading.CSV)
			return;
		dispatch(downloadCSV(userUUID, jobUUID));
		setDownloading({
			...downloading,
			CSV: true
		});
	}

	const onDownloadJSONClick = () => {
		if (downloading.JSON)
			return;
		// dispatch download event
		setDownloading({
			...downloading,
			JSON: true
		});
	}

	const onAbortClick = () => {
		setAborting(true);
		dispatch(abortJob(userUUID, jobUUID));
	}

	if (status === JobStatus.DONE || status === JobStatus.ABORTED) {
		return <Actions
			downloadCSV
			isDownloadingCSV={downloading.CSV}
			onDownloadCSVClick={onDownloadCSVClick}
			downloadJSON
			isDownloadingJSON={downloading.JSON}
			onDownloadJSONClick={onDownloadJSONClick}
			showTruncatedMessage={showTruncatedMessage}
			showAvatar={showAvatar}
		/>;
	}
	if (status === JobStatus.INPROGRESS || status === JobStatus.READY) {
		return <Actions
			abortJob
			isAbortingJob={aborting}
			onAbortJobClick={onAbortClick}
			showTruncatedMessage={showTruncatedMessage}
			showAvatar={false}
		/>;
	}
	return null;

}