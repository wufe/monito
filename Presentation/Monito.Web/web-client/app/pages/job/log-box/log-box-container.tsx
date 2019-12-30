import * as React from 'react';
import { LogBox } from './log-box';
import { useSelector, useDispatch } from 'react-redux';
import { GlobalState } from '~/state/state';
import { addLogMessageActionBuilder } from '~/state/action';

export const LogBoxContainer = () => {

	const messages = useSelector<GlobalState, string[]>(state => state.application.logMessages);

	return <LogBox messages={messages} />;
}