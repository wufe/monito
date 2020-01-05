import { RouterState } from "connected-react-router"
import { ApplicationState } from "~/state/states/application-state"
import { JobState } from "~/state/states/job-state"

export type GlobalState = {
	application: ApplicationState;
	job        : JobState;
	router     : RouterState;
}