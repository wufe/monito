import * as React from 'react';
import { useDispatch } from 'react-redux';
import { LogBoxContainer } from '~/app/pages/job/log-box/log-box-container';
import { loadJobThunk } from '~/thunk/job-thunk';
import { useParams } from 'react-router-dom';

export const JobPage = () => {

	const dispatch = useDispatch();

	const { userUUID, jobUUID } = useParams();

	dispatch(loadJobThunk(userUUID, jobUUID));

	return <div className="job-page__component">
		<LogBoxContainer />
	</div>;
}

export default JobPage;