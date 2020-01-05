import Axios, { AxiosResponse } from 'axios';
import Moment from 'moment';
import { push } from "connected-react-router";
import { ApplicationThunkAction } from "~/thunk/thunk";
import { JobRequestFormFields, SaveJobResponse, JobModel, JobStatuModel, JobStatus } from "~/types/job";
import { setLoadingActionBuilder } from "~/state/actions/application-actions";
import { addLogMessageActionBuilder, setJobActionBuilder, setStatusPolling, setJobStatus } from "~/state/actions/job-actions";
import { jobHubInstanceGetter, JobHub } from '~/signalr/jobhub';

export const getStatusLabel = (status: string) => {
    let label: string = "Waiting (queued)";
    switch (status) {
        case JobStatus.DONE:
            label = "Done";
            break;
        case JobStatus.INPROGRESS:
            label = "In progress";
            break;
    }
    return label;
}

export const saveJobThunk = (requestFields: JobRequestFormFields): ApplicationThunkAction<SaveJobResponse> =>
    dispatch => {
        dispatch(setLoadingActionBuilder(true));
        return Axios.post<SaveJobResponse>(`/api/job/save`, requestFields)
            .then(({ data }) => {
                const newRoute = `/job/${data.userUUID}/${data.requestUUID}`;
                dispatch(push(newRoute));
                return data;
            })
            .finally(() => dispatch(setLoadingActionBuilder(false)));
    };

export const loadJobThunk = (userUUID: string, jobUUID: string): ApplicationThunkAction<JobModel> =>
    dispatch => {
        dispatch(setLoadingActionBuilder(true));
        return Axios.get<JobModel>(`/api/job/${userUUID}/${jobUUID}`)
            .then(({data}) => {
                dispatch(addLogMessageActionBuilder(`${data.options.redirects} max redirects`));
                dispatch(addLogMessageActionBuilder(`${data.options.method} method`));
                dispatch(addLogMessageActionBuilder(`${data.options.timeout}ms timeout`));
                dispatch(addLogMessageActionBuilder(`${data.options.threads} threads`));
                dispatch(addLogMessageActionBuilder(`Last update ${Moment.utc(data.updatedAt).fromNow(false)}`));
                const status = getStatusLabel(data.status);
                dispatch(addLogMessageActionBuilder(`Status: ${status}`));
                dispatch(setJobActionBuilder(data));
                if (data.status === JobStatus.INCOMPLETE ||
                    data.status === JobStatus.READY) {
                    dispatch(setStatusPolling(true));
                }
                return data;
            })
            .catch(error => {
                const response: AxiosResponse<JobModel> = error.response;
                if (response && response.status === 404)
                    dispatch(addLogMessageActionBuilder("Request not found."));
                return null;
            })
            .finally(() => dispatch(setLoadingActionBuilder(false)));
    };

//
export const refreshJobStatusThunk =
    (userUUID: string, jobUUID: string): ApplicationThunkAction<JobStatuModel | null> =>
    (dispatch, getState) => {
        const state = getState();
        if (!state.job.polling.status)
            return Promise.resolve(null);
        return Axios.get<JobStatuModel>(`/api/job/${userUUID}/${jobUUID}/status`)
            .then(({data}) => {
                const newStatus = data.status;
                const savedJob = state.job.job;
                if (savedJob) {
                    if (savedJob.status !== newStatus) {
                        dispatch(addLogMessageActionBuilder(`Status: ${getStatusLabel(newStatus)}`));
                        dispatch(setJobStatus(newStatus));
                    }
                }
                if (newStatus !== JobStatus.INCOMPLETE &&
                    newStatus !== JobStatus.READY) {
                    dispatch(setStatusPolling(false));
                }
                return data;
            });
    };

export const requestJobUpdates =
    (): ApplicationThunkAction<any> =>
    (dispatch, getState) => {
        const state = getState();
        if (!state.job.job || state.job.job.status !== JobStatus.INPROGRESS)
            return Promise.resolve();
        const requestID = state.job.job.id;
        const jobHub = jobHubInstanceGetter.instance(() => new JobHub(dispatch));
        return jobHub
            .tryConnect()
            .then(instance => {
                jobHubInstanceGetter.register(instance);
                instance.hubConnection.invoke('RequestJobUpdates', requestID, -1);
            });
    };

export const deactivateJobUpdatesStream =
    (): ApplicationThunkAction<any> =>
    dispatch => {
        jobHubInstanceGetter.disconnect();
        return Promise.resolve();
    };