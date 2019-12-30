import * as React from 'react';
import { useSelector } from 'react-redux';
import { GlobalState } from '~/state/state';
import { LoadingBar } from './loading-bar';

export const LoadingBarContainer = () => {
	const loading = useSelector<GlobalState, boolean>(state => state.application.loading);

	return <LoadingBar visible={loading} />;
}