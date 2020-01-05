import * as React from 'react';
import { LogBox } from './log-box';
import { useSelector, useDispatch } from 'react-redux';
import { GlobalState } from '~/state/state';

export const LogBoxContainer = () => {

	const messages = useSelector<GlobalState, string[]>(state => state.job.log.messages);

	return <LogBox messages={messages} />;
}