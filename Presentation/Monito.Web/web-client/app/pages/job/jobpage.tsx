import * as React from 'react';
import { LogBoxContainer } from '~/app/pages/job/log-box/log-box-container';
import { useParams } from 'react-router-dom';
import { LinksBoxContainer } from './links/links-box-container';
import { useRealtimeJobUpdates, useJobFetch } from '~/app/pages/job/job-hooks';

export const JobPage = () => {

	const { userUUID, jobUUID } = useParams();

	useRealtimeJobUpdates(userUUID, jobUUID);
	useJobFetch(userUUID, jobUUID);

	return <div className="job-page__component">
		<LogBoxContainer />
		<LinksBoxContainer />
	</div>;
}

export default JobPage;