import { ThunkAction } from "redux-thunk";
import { ApplicationState } from "~/state/state";
import { AnyAction, Action } from "redux";

export type ApplicationThunkAction<R = any> =
    ThunkAction<R, ApplicationState, unknown, AnyAction>;