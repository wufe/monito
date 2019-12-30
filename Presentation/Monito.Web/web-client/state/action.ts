import { AnyAction } from "redux";

export enum ApplicationActions {
	SetLoading = "@@Application/SetLoading",
	AddLogMessage = "@@Application/AddLogMessage",
};

export const setLoadingActionBuilder = (loading: boolean): AnyAction => ({
	type: ApplicationActions.SetLoading,
	payload: loading
});

export const addLogMessageActionBuilder = (message: string): AnyAction => ({
	type: ApplicationActions.AddLogMessage,
	payload: message
});