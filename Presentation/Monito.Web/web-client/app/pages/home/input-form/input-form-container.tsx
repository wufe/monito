import * as React from 'react';
import { InputForm } from './input-form';
import { useDispatch } from 'react-redux';
import { saveJobThunk } from '~/thunk/job-thunk';

export const InputFormContainer = () => {

    const dispatch = useDispatch();

    return <InputForm onSubmit={e => dispatch(saveJobThunk(e))} />;
}
    