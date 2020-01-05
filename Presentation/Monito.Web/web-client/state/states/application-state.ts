export type ApplicationState = {
	loading: boolean;
	loadingCount: number;	
}

export const getInitialApplicationState = (): ApplicationState => ({
	loading: false,
	loadingCount: 0
});