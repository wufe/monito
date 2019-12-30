import { RouterState } from "connected-react-router"

export type GlobalState = {
	application: ApplicationState;
	router: RouterState;
}

export type ApplicationState = {
	loading: boolean;
	loadingCount: number;
}

export const getInitialState = (): ApplicationState => ({
	loading: false,
	loadingCount: 0
})