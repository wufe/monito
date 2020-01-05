import { AnyAction } from "redux";
import { JobModel, JobStatus } from "~/types/job";
import { LinkModel } from "~/types/link";

export enum JobActions {
	AddLogMessage        = "@@Job/AddLogMessage",
	SetJob               = "@@Job/SetJob",
	SetStatusPolling     = '@@Job/SetStatusPolling',
	SetJobStatus         = '@@Job/SetStatus',
	AddLinksToCurrentJob = '@@Job/AddLinksToCurrentJob',
}

export const addLogMessageActionBuilder = (message: string): AnyAction => ({
	type: JobActions.AddLogMessage,
	payload: message
});

export const setJobActionBuilder = (job: JobModel): AnyAction => ({
	type: JobActions.SetJob,
	payload: job
});

export const setStatusPolling = (enable: boolean): AnyAction => ({
	type: JobActions.SetStatusPolling,
	payload: enable
});

export const setJobStatus = (status: JobStatus): AnyAction => ({
	type: JobActions.SetJobStatus,
	payload: status
});

export const addLinksToCurrentJob = (links: LinkModel[]): AnyAction => ({
	type: JobActions.AddLinksToCurrentJob,
	payload: links
});