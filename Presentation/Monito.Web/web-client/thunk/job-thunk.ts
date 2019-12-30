import { ApplicationThunkAction } from "./thunk";
import { JobRequestFormFields, JobRequestType, JobRequestHTTPMethod, JobRequestStatus } from "~/types/job";
import Axios from 'axios';
import { push } from "connected-react-router";
import { setLoadingActionBuilder, addLogMessageActionBuilder } from "~/state/action";
import Moment from 'moment';

type SaveJobResponse = {
    userUUID: string;
    requestUUID: string;
}

type RetrieveJobResponse = {
    type     : JobRequestType;
    status   : JobRequestStatus;
    createdAt: string;
    updatedAt: string;
    options: {
        method   : JobRequestHTTPMethod;
        redirects: number;
        threads  : number;
        timeout  : number;
        userAgent: string;
    }
}

export const saveJobThunk = (requestFields: JobRequestFormFields): ApplicationThunkAction =>
    dispatch => {
        dispatch(setLoadingActionBuilder(true));
        return Axios.post<SaveJobResponse>(`/api/job/save`, requestFields)
            .then(({ data }) => {
                const newRoute = `/job/${data.userUUID}/${data.requestUUID}`;
                dispatch(push(newRoute));
            })
            .finally(() => dispatch(setLoadingActionBuilder(false)));
    };

export const loadJobThunk = (userUUID: string, jobUUID: string): ApplicationThunkAction =>
    dispatch => {
        dispatch(setLoadingActionBuilder(true));
        dispatch(addLogMessageActionBuilder("Loading your request"));
        return Axios.get<RetrieveJobResponse>(`/api/job/${userUUID}/${jobUUID}`)
            .then(({data}) => {
                dispatch(addLogMessageActionBuilder(`Status: ${data.status}`));
                dispatch(addLogMessageActionBuilder(`Options:`));
                dispatch(addLogMessageActionBuilder(`${data.options.threads} threads`));
                dispatch(addLogMessageActionBuilder(`${data.options.method} method`));
                dispatch(addLogMessageActionBuilder(`${data.options.redirects} max redirects`));
                dispatch(addLogMessageActionBuilder(`${data.options.timeout}ms timeout`));
                dispatch(addLogMessageActionBuilder(`Last update: ${Moment.utc(data.updatedAt).fromNow(false)}`));
            })
            .finally(() => dispatch(setLoadingActionBuilder(false)));
    };