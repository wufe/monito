import { Reducer, combineReducers } from "redux";
import { connectRouter } from 'connected-react-router';
import { History } from 'history';


export const createRootReducer = (history: History): Reducer =>
    combineReducers({
        router: connectRouter(history)
    });