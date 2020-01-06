import * as React from 'react';
import './homepage.scss';
import { InfoHeader } from '~/app/pages/home/info-header/info-header';
import { InputFormContainer } from '~/app/pages/home/input-form/input-form-container';
import { useDispatch } from 'react-redux';
import { ApplicationThunkDispatch } from '~/thunk/thunk';
import { resetJob } from '~/state/actions/job-actions';

export const HomePage = () => {

    const dispatch = useDispatch<ApplicationThunkDispatch>();
    dispatch(resetJob());

    return <div className="homepage__component">
        <InfoHeader />
        <InputFormContainer />
    </div>;
};

export default HomePage;