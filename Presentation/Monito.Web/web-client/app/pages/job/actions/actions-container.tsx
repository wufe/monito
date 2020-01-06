import * as React from 'react';
import { Actions } from './actions';
import { useSelector, useDispatch } from 'react-redux';
import { GlobalState } from '~/state/state';
import { JobStatus, JobModel } from '~/types/job';
import { ApplicationThunkDispatch } from '~/thunk/thunk';
import { useParams } from 'react-router-dom';
import { downloadCSV } from '~/thunk/job-thunk';

export const ActionsContainer = () => {

	const dispatch = useDispatch<ApplicationThunkDispatch>();
	const { userUUID, jobUUID } = useParams();
	const [downloading, setDownloading] = React.useState({
		CSV : false,
		JSON: false,
	});

	const job = useSelector<GlobalState, JobModel>(state => {
		if (!state.job.job)
			return null;
		return state.job.job;
	});

	const status = job && job.status;

	const showTruncatedMessage = job === null ? false : (job.linksCount && job.linksCount > job.links.length);

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

	return status === JobStatus.DONE && <Actions
		isDownloadingCSV={downloading.CSV}
		isDownloadingJSON={downloading.JSON}
		onDownloadCSVClick={onDownloadCSVClick}
		onDownloadJSONClick={onDownloadJSONClick}
		showTruncatedMessage={showTruncatedMessage}
		showAvatar={showAvatar}
		 />;
}