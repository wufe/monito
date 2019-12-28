import { ApplicationThunkAction } from "./thunk";
import { JobRequestFormFields } from "~/types/form";

export const saveJobThunk = (requestFields: JobRequestFormFields): ApplicationThunkAction => (dispatch, getState) => {
    return new Promise((resolve, reject) => {
        setTimeout(() => {
            dispatch({
                type: '@@Job/SaveSuccessful',
                payload: 'ciccio'
            });
            resolve();
        }, 1000);
    });
};