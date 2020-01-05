import { ThunkAction, ThunkDispatch } from "redux-thunk";
import { GlobalState } from "~/state/state";
import { AnyAction, Action } from "redux";

export type ApplicationThunkAction<R = any> =
    ThunkAction<Promise<R>, GlobalState, unknown, AnyAction>;

export type ApplicationThunkDispatch = ThunkDispatch<GlobalState, unknown, Action>;