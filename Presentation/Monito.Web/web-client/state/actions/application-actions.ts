import { AnyAction } from "redux";

export enum ApplicationActions {
	SetLoading = "@@Application/SetLoading",
};

export const setLoadingActionBuilder = (loading: boolean): AnyAction => ({
	type: ApplicationActions.SetLoading,
	payload: loading
});