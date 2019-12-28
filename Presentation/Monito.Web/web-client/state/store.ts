import { createStore, compose, applyMiddleware, Middleware } from "redux";
import { createRootReducer } from "~/state/reducer";
import { createBrowserHistory } from "history";
import { routerMiddleware } from "connected-react-router";
import ThunkMiddleware from 'redux-thunk';

export const history = createBrowserHistory();

const middlewares: Middleware[] = [
    routerMiddleware(history),
    ThunkMiddleware
];

const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;
export const store = createStore(createRootReducer(history), /* preloadedState, */composeEnhancers(
    applyMiddleware(...middlewares)
));