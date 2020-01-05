import { Reducer, combineReducers } from "redux";
import { connectRouter } from 'connected-react-router';
import { History } from 'history';
import { applicationReducer } from "~/state/reducers/application-reducer";
import { jobReducer } from "~/state/reducers/job-reducer";

export const createRootReducer = (history: History): Reducer =>
    combineReducers({
        application: applicationReducer,
        job: jobReducer,
        router: connectRouter(history)
    });