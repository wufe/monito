import { Reducer, combineReducers, AnyAction } from "redux";
import { connectRouter } from 'connected-react-router';
import { History } from 'history';
import { ApplicationState, getInitialState } from "./state";
import { ApplicationActions } from "./action";

const applicationReducer: Reducer<ApplicationState> = (state: ApplicationState = getInitialState(), action: AnyAction) => {
    switch (action.type) {
        case ApplicationActions.SetLoading:
            let loadingCount = state.loadingCount;
            if (action.payload === true) {
                loadingCount++;
            } else {
                loadingCount--;
            }
            const loading = loadingCount !== 0;
            return { ...state, loading, loadingCount };
        case ApplicationActions.AddLogMessage:
            return {
                ...state,
                logMessages: [
                    ...state.logMessages,
                    action.payload
                ]
            };
        default:
            return state;
    }
};

export const createRootReducer = (history: History): Reducer =>
    combineReducers({
        application: applicationReducer,
        router: connectRouter(history)
    });