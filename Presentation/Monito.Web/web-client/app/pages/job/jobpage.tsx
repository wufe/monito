import * as React from 'react';
import { LogBoxContainer } from '~/app/pages/job/log-box/log-box-container';
import { useParams } from 'react-router-dom';
import { LinksBoxContainer } from '~/app/pages/job/links/links-box-container';
import { useRealtimeJobUpdates, useJobFetch } from '~/app/pages/job/job-hooks';
import { ActionsContainer } from '~/app/pages/job/actions/actions-container';

export const JobPage = () => {

	const { userUUID, jobUUID } = useParams<{ userUUID: string; jobUUID: string; }>();

	// useRealtimeJobUpdates(userUUID, jobUUID);
	useJobFetch(userUUID, jobUUID);

	return <div className="job-page__component">
		<LogBoxContainer />
		<ActionsContainer />
		<LinksBoxContainer />
	</div>;
}

export default JobPage;