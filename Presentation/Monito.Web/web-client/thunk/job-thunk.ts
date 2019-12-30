import { ApplicationThunkAction } from "./thunk";
import { JobRequestFormFields } from "~/types/form";
import Axios from 'axios';
import { push } from "connected-react-router";
import { SetLoadingActionBuilder } from "~/state/action";

type SaveJobResponse = {
    userUUID: string;
    requestUUID: string;
}

export const saveJobThunk = (requestFields: JobRequestFormFields): ApplicationThunkAction =>
    dispatch => {
        dispatch(SetLoadingActionBuilder(true));
        return Axios.post<SaveJobResponse>(`/api/job/save`, requestFields)
            .then(({ data }) => {
                const newRoute = `/job/${data.userUUID}/${data.requestUUID}`;
                dispatch(push(newRoute));
            })
            .finally(() => dispatch(SetLoadingActionBuilder(false)));
    };