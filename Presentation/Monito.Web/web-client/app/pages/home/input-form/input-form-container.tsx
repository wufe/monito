import * as React from 'react';
import { InputForm } from './input-form';
import { useDispatch, useSelector } from 'react-redux';
import { saveJobThunk } from '~/thunk/job-thunk';
import { ApplicationState, GlobalState } from '~/state/state';

export const InputFormContainer = () => {

    const dispatch = useDispatch();
    const loading = useSelector<GlobalState, boolean>(state => state.application.loading);

    return <InputForm
        onSubmit={e => dispatch(saveJobThunk(e))}
        disabled={loading} />;
}
    